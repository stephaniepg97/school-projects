<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Contracts\Support\Jsonable;

use App\Http\Resources\Order as OrderResource;
use Illuminate\Support\Facades\DB;
use DateTime;
use Validator;
use Illuminate\Validation\Rule;

use App\Order;
use App\Meal;
use App\Item;

class OrderControllerAPI extends Controller
{
    public function index(Request $request)
    {
        $query = [];
         
        if ($request->has('state')) {
            array_push($query, ['state', $request->state]);
        }

        if ($request->has('date')) {
            array_push($query, ['start', 'like', $request->date.'%']);
        }

        $orders = Order::where($query);

        if ($request->has('cook')) {
            $name = $request->cook;
            $orders = $orders->whereHas('cook', function ($query) use ($name) {
                $query->where('name', 'like', '%'.$name.'%');
            });
        } 

        if ($request->has('waiter')) {
            $name = $request->waiter;
            $meals = $orders->whereHas('meal', function ($query) use($name) { 
                $query->whereHas('waiter', function ($query) use($name) {
                    $query->where('name', 'like', '%'.$name.'%');
                });
            });
        }

        if ($request->has('page')) {
            $orders = $orders->paginate(5);
        } else {
            $orders = $orders->get();
        }

        if ($orders->isEmpty()) {
            $message = "No orders found.";
            return response()->json(compact('message'), 404);
        }

        return OrderResource::collection($orders); 
    }

    public function show($id)
    {
        $order = Order::find($id);
        if (!$order) 
        {
            $message = 'Order no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        return response()->json(new OrderResource($order), 200);
    }

    public function store(Request $request)
    {
        $validator = Validator::make($request->all(), [
            'item_id'=> [
                'required',
                'numeric',
                function ($attribute, $value, $fail) {
                    if (!Item::findOrFail($value)) {
                        $fail('Item does not exist');
                    }
                }
            ],
            'meal_id'=> [
                'required',
                'numeric',
                function ($attribute, $value, $fail) {
                    if (!Meal::findOrFail($value)) {
                        $fail('Meal does not exist');
                    }
                }
            ],
            'seasoning' => [
                'nullable',
                Rule::in(['unsalty', 'salty', 'sweet', 'spicy']),
            ]
        ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }
        
        $order = new Order();
        $order->fill($request->all());
        $order->state = "pending";
        $now = new DateTime();
        $order->start = $now->format('Y-m-d H:i:s');
        $order->save();

        //UPDATE TOTAL PRICE OF THE ASSOCIATED MEAL
        $item = Item::findOrFail($order->item_id);
        $meal = Meal::findOrFail($order->meal_id);
        $meal->total_price_preview += $item->price;
        $meal->update();

        return response()->json(new OrderResource($order), 201);
    }

    public function confirm($id)
    {
        $order = Order::find($id);
        if (!$order) 
        {
            $message = 'Order no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        $order->state = 'confirmed';
        $order->update();
        $message = 'Order no.: '.$id.' confirmed!';
        return response()->json(compact('message'), 202);
    }

    public function prepare($id)
    {
        $order = Order::find($id);
        if (!$order) 
        {
            $message = 'Order no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        $order->state = 'prepared';
        $order->update();
        $message = 'Order no.: '.$id.' prepared!';
        return response()->json(compact('message'), 202);
    }

    public function inPreparation($id)
    {
       $order = Order::find($id);
        if (!$order) 
        {
            $message = 'Order no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        $order->state = 'in preparation';
        $order->update();
        $message = 'Order no.: '.$id.' in preparation!';
        return response()->json(compact('message'), 202);
    }

    public function deliver($id)
    {
        $order = Order::find($id);
        if (!$order) 
        {
            $message = 'Order no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        $order->state = 'delivered';
        $order->update();
        $message = 'Order no.: '.$id.' delivered!';
        return response()->json(compact('message'), 202);
    }

    public function destroy($id)
    {
        $order = Order::find($id);
        
        if (!$order) 
        {
            $message = 'Order no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        //UPDATE TOTAL PRICE OF THE ASSOCIATED MEAL
        $item = Item::findOrFail($order->item_id);
        $meal = Meal::findOrFail($order->meal_id);
        $meal->total_price_preview -= $item->price;
        $meal->update();

        $order->delete();
        return response()->json(null, 204);
    }

    //total number of orders handled per month
    public function totals()
    {
        $totals = Order::totals();

        return response()->json($totals, 200);
    }
}
