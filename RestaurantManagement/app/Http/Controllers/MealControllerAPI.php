<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Contracts\Support\Jsonable;
use Illuminate\Support\Facades\DB;
use Illuminate\Pagination\LengthAwarePaginator;
use Illuminate\Validation\Rule;
use DateTime;
use Validator;

use App\Meal;
use App\Table;
use App\User;
use App\Item;
use App\Invoice;
use App\Order;
use App\Http\Resources\Meal as MealResource;
use App\Http\Resources\Invoice as InvoiceResource;
use App\Http\Resources\Order as OrderResource;

class MealControllerAPI extends Controller
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
        
        $meals =  Meal::where($query);

        if ($request->has('waiter')) {
            $name = $request->waiter;
            $meals =$meals->whereHas('waiter', function ($query) use($name) {
                    $query->where('name', 'like', '%'.$name.'%');
                });
        }
        
        if ($request->has('page')) {
            $meals = $meals->paginate(5);
        } 
        else {
            $meals = $meals->get();
        } 

        if ($meals->isEmpty()) {
            $message = "No meals found.";
            return response()->json(compact('message'), 404);
        }

        return MealResource::collection($meals);
    }

    public function show($id)
    {
        $meal = Meal::find($id);

        if (!$meal) 
        {
            $message = 'Meal no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        return response()->json(new MealResource($meal), 200);
    }

    public function store(Request $request)
    {
        $validator = Validator::make($request->all(), [
                'table_number' => [
                    'required',
                    'numeric',
                    function ($attribute, $value, $fail) {
                        if (!Table::findTable($value)) {
                            $fail('Table does not exist');
                        }
                    },
                    function ($attribute, $value, $fail) {
                        if (!Table::isAvaiable($value)) {
                            $fail('Table is not avaiable right now.');
                        }
                    }
                ],
                'responsible_waiter_id' => [
                    'required',
                    'numeric',
                    function ($attribute, $value, $fail) {
                        if (!User::findOrFail($value)) {
                            $fail('User does not exist');
                        }
                    },
                    function ($attribute, $value, $fail) {
                        if (!User::isWaiter($value)) {
                            $fail('User is not a waiter.');
                        }
                    }
                ]
            ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }

        $meal = new Meal();
        $meal->fill($request->all());
        $meal->state = "active";
        $now = new DateTime();
        $meal->start = $now->format('Y-m-d H:i:s');
        $meal->save();

        return response()->json(new MealResource($meal), 201);
    }

    public function terminate($id) {

        $meal = Meal::find($id);

        if (!$meal) 
        {
            $message = 'Meal no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        if ($meal->invoice || ($meal->state != 'active')) 
        {
            $message = 'Meal no.: '.$id.' already finished.';
            return response()->json(compact('message'), 409);
        }

        if (!$meal->orders) 
        {
            $message = 'Meal no.: '.$id.' does not have orders.';
            return response()->json(compact('message'), 409);
        }
        
        $now = new DateTime();
        $meal->end = $now->format('Y-m-d H:i:s');
        
        //Close orders
        $closed = $meal->closeOrders();
        
        $delivered = $meal->deliveredOrders();

        if ($delivered)
        {
            //CREATE INVOICE
            $invoice = Invoice::create(
                    [
                        'name' => null,
                        'state' => 'pending',
                        'nif' => null,
                        'meal_id' => $meal->id,
                        'total_price' => .00,
                        'date' => $meal->end,
                    ]
                );

            $grouped = $delivered->groupBy('item_id');
            $grouped->toArray();

            foreach ($grouped as $item_id => $orders) {

                $item = Item::find($item_id);

                if (!$item) 
                {
                    $message = 'Item no.: '.$item_id.' ordered does not exist anymore.';
                    return response()->json(compact('message'), 409);
                }

                $sub_total_price = .00;
                $sub_total_price = $orders->count()*$item->price;
                $item->invoices()->save($invoice, 
                    [
                        'quantity' => $orders->count(), 
                        'unit_price' => $item->price, 
                        'sub_total_price' => $sub_total_price
                    ]
                );
                $meal->total_price_preview += $sub_total_price;
            }

            //UPDATE total price
            $meal->update();
            $invoice->update(['total_price' => $meal->total_price_preview]);
        }

        $response = [
            'message' => 'Meal no.: '+ $meal->id +'terminated! New Invoice created (no.: '.$invoice->id.')',
            'invoice' => new InvoiceResource($invoice),
            'meal' => new MealResource($meal)
        ];
        
        return response()->json(compact('response'), 202);
    }

    public function update(Request $request, $id)
    {
        $validator = Validator::make($request->all(), [
                'responsible_waiter_id' => [
                    'required',
                    'numeric',
                    function ($attribute, $value, $fail) {
                        if (!User::findOrFail($value)) {
                            $fail('User does not exist');
                        }
                    },
                    function ($attribute, $value, $fail) {
                        if (!User::isWaiter($value)) {
                            $fail('User is not a waiter.');
                        }
                    }
                ],
                'table_number' => [
                    'required',
                    'numeric',
                    function ($attribute, $value, $fail) {
                        if (!Table::findTable($value)) {
                            $fail('Table does not exist');
                        }
                    },
                    function ($attribute, $value, $fail) {
                        if (!Table::isAvaiable($value)) {
                            $fail('Table is not avaiable right now.');
                        }
                    }
               ]
            ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }
        
        $meal = Meal::find($id);

        if (!$meal) 
        {
            $message = 'Meal no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $meal->fill($request->all());
        $meal->update();

        return response()->json(new MealResource($meal), 202);
    }
    
    public function destroy($id)
    {
        $meal = Meal::find($id);

        if (!$meal) 
        {
            $message = 'Meal no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        
        $meal->delete();
        return response()->json(null, 204);
    }

    public function notPaid($id)
    {
        $meal = Meal::find($id);

        if (!$meal) 
        {
            $message = 'Meal no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $meal->state = 'not paid';
        $meal->update();

        return response()->json(new MealResource($meal), 202);
    }

    public function orders(Request $request, $id)
    {
        $meal = Meal::find($id);

        if (!$meal) 
        {
            $message = 'Meal no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $query = [];
         
        if ($request->has('state')) {
            array_push($query, ['state', $request->state]);
        }

        if ($request->has('date')) {
            array_push($query, ['start', 'like', $request->date.'%']);
        }

        array_push($query, ['meal_id', $id]);

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
            $message = "No orders found on Meal no.: ".$id;
            return response()->json(compact('message'), 404);
        }

        return OrderResource::collection($orders); 
    }

    //total number of meals handled per month
    public function totals()
    {
        $totals = Meal::totals();

        return response()->json($totals, 200);
    }

    //the average time it takes to handle each meal (time between the moment a meal iscreated until the meal is closed – “paid” or “not paid”) per month
    public function averages(Request $request) {

        if ($request->has('graphic') && $request->graphic) {
            $averages = Meal::graphicAverages();
        } 
        else {
            $averages = Meal::averages();
        }

        if ($request->has('page')) {
            $page = intval($request->page);
            $perPage = 6;
            $parts = $averages->chunk($perPage);
            $averages = new LengthAwarePaginator($parts[$page-1], $averages->count(), $perPage, LengthAwarePaginator::resolveCurrentPage($page), [
                    'path' => $request->url()
                ]);
        }

        if ($averages->isEmpty()) {
            $message = "No averages found from handled orders of all meals.";
            return response()->json(compact('message'), 404);
        }
        
        return response()->json($averages, 200);
    }

}
