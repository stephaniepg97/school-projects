<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Contracts\Support\Jsonable;
use Illuminate\Support\Facades\Storage;
use App\Http\Resources\User as UserResource;
use App\Http\Resources\Order as OrderResource;
use App\Http\Resources\Meal as MealResource;
use Illuminate\Support\Facades\DB;
use Illuminate\Validation\Rule;
use Illuminate\Http\File;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Order;

use App\User;
use Hash;
use Validator;
use DateTime;

class UserControllerAPI extends Controller
{
	public function index(Request $request)
    {
        $query = [];
        if ($request->has('type')) {
            array_push($query, ['type', $request->type]);
        }
        if ($request->has('name')) {
            array_push($query, ['name', 'like', '%'.$request->name.'%']);
        } 
        $users = User::where($query);
        if ($request->has('page')) {
            $users = $users->paginate(5);
        } 
        else {
            $users = $users->get();
        }
        if ($users->isEmpty()) {
            $message = "No users found.";
            return response()->json(compact('message'), 404);
        }
        return UserResource::collection($users);
    }

    public function show($id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        return response()->json(new UserResource($user), 200);
    }

    public function update(Request $request, $id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $validator = Validator::make($request->all(), [
                'name' => 'required|regex:/^[A-Za-záàâãéèêíóôõúçÁÀÂÃÉÈÍÓÔÕÚÇ\s]+$/',
                'username' => 'required|alpha_num|unique:users,username,'.$id,
                'type' => [
                    'required',
                    Rule::in(['waiter', 'cook', 'manager', 'cashier']),
                ],
                'photo_url' => 'required|regex:/^.+\.(jpe?g)?(png)?(bmp)?(svg)?(gif)?$/',
                'email' => 'required|email|unique:users,email,'.$id,
            ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }

        if ($request->has('image')) 
        {
            $image_parts = explode(";base64,", $request->image);
            $image_base64 = base64_decode($image_parts[1]);
            file_put_contents('C:\temp\\'.$request->photo_url, $image_base64);
            //store uploaded photo
            $path = Storage::putFile('public/profiles', new File('C:\temp\\'.$request->photo_url));
            $path_parts = explode("/", $path);
            $user->photo_url = $path_parts[2];
        }
        
        $user->update($request->except('photo_url'));
        
        return response()->json(new UserResource($user), 202);
    }

    public function destroy($id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $user->forceDelete();

        return response()->json(null, 204);
    }

    public function trash($id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $user->delete();

        if (!$user->trashed()) 
        {
            $message = 'User no.: '.$id.' was not soft deleted.';
            return response()->json(compact('message'), 409);
        }

        $message = 'User no.: '.$id.' was soft deleted.';
        return response()->json(compact('message'), 202);
    }

    public function emailAvailable(Request $request)
    {
        $totalEmail = 1;
        if ($request->has('email') && $request->has('id')) {
            $totalEmail = DB::table('users')->where('email', '=', $request->email)->where('id', '<>', $request->id)->count();
        } else if ($request->has('email')) {
            $totalEmail = DB::table('users')->where('email', '=', $request->email)->count();
        }
        return response()->json($totalEmail == 0);
    }

    public function block($id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $user->blocked = '1';
        $user->update();

        $message = 'User no.: '.$id.' is now blocked.';
        return response()->json(compact('message'), 202);
    }

    public function unblock($id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $user->blocked = '0';
        $user->update();

        $message = 'User no.: '.$id.' is now unblocked.';
        return response()->json(compact('message'), 202);
    }

    public function password(Request $request, $id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        
        $validator = Validator::make($request->all(), [
            'password' => 'required|min:3',
            'confirm' => 'required|same:password',
        ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }

        $user->password = bcrypt($request->password);
        $user->update();

        $message = 'Password from User no.: '.$id.' updated successfully.';
        return response()->json(compact('message'), 202);
    }

    public function me(Request $request)
    {
        if (! $request->user()) 
        {
            $message = 'Current user\'s credential information not found.';
            return response()->json(compact('message'), 409);
        }

        return new UserResource($request->user());
    }

    public function end($id)
    {
        $user = User::find($id);
        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        
        $now = new DateTime();
        $end = $now->format('Y-m-d H:i:s');

        $user->shift_active = '0';
        $user->last_shift_end = $end;

        $user->update();
        $message = 'User no.: '.$id.' ended his shift now';
        return response()->json(compact('message'), 202);
    }

    public function start($id)
    {
        $user = User::find($id);
        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $now = new DateTime();
        $start = $now->format('Y-m-d H:i:s');

        $user->shift_active = '1';
        $user->last_shift_start = $start;
        
        $user->update();
        $message = 'User no.: '.$id.' started his shift now';
        return response()->json(compact('message'), 202);
    }

    public function orders(Request $request, $id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        
        if ( ($user->type != 'cook') && ($user->type != 'waiter') )
        {
            $message = 'User no.: '.$id.' is neither a "cook" or a "waiter".';
            return response()->json(compact('message'), 409);     
        }

        $query = [];
         
        if ($request->has('state')) {
            array_push($query, ['state', $request->state]);
        }

        if ($request->has('date')) {
            array_push($query, ['start', 'like', $request->date.'%']);
        }

        if ($user->type == 'cook') {
            array_push($query, ['responsible_cook_id', $id]);
            $orders = Order::where($query);
        }
        else {
            $orders = Order::where($query)->whereHas('meal', function ($query) use ($user) {
                $query->where('responsible_waiter_id', $user->id);
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

    public function meals(Request $request, $id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        
        if ($user->type != 'waiter')
        {
            $message = 'User no.: '.$id.' is not a "waiter".';
            return response()->json(compact('message'), 409);  
        }

        $query = [];
            
        if ($request->has('state')) {
            array_push($query, ['state', $request->state]);
        }
        if ($request->has('date')) {
            array_push($query, ['start', 'like', $request->date.'%']);
        }

        array_push($query, ['responsible_waiter_id', $id]);

        $meals = Meal::where($query);

        if ($request->has('page')) {
            $meals = $meals->paginate(5);
        } 
        else {
            $meals = $meals->get();
        } 

        if ($meals->isEmpty()) {
            $message = "No meals found on User no.: ".$id;
            return response()->json(compact('message'), 404);
        }
        
        return MealResource::collection($meals);
    }

    public function averages(Request $request, $id) {

        $user = User::find($id);

        if (!$user) 
        {
            $message = 'User no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        
        if ( ($user->type != 'cook') && ($user->type != 'waiter') )
        {
            $message = 'User no.: '.$id.' is neither a "cook" or a "waiter".';
            return response()->json(compact('message'), 409);     
        }

        if ($request->has('graphic') && $request->graphic) {
            $averages = $user->graphicAverages();
        } 
        else {
            $averages = $user->averages();
        }

        if ($averages->isEmpty()) {
            $message = "No averages from handled orders found on User no.: ".$id;
            return response()->json(compact('message'), 404);
        }

        if ($request->has('page')) {
            $page = intval($request->page);
            $perPage = 6;
            $parts = $averages->chunk($perPage);
            $averages = new LengthAwarePaginator($parts[$page-1], $averages->count(), $perPage, LengthAwarePaginator::resolveCurrentPage($page), [
                    'path' => $request->url()
                ]);
        }
        
        return response()->json($averages, 200);
    }
}
