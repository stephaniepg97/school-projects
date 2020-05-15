<?php

namespace App\Http\Controllers\Auth;

use Illuminate\Http\Request;
use App\Http\Controllers\Controller;
use Illuminate\Validation\Rule;
use Validator;

use App\User;
use Hash;
use Illuminate\Http\UploadedFile;
use Illuminate\Support\Facades\Storage;
use Illuminate\Http\File;
use App\Http\Resources\User as UserResource;

class RegisterControllerAPI extends Controller
{
    public function register(Request $request)
    {
        $validator = Validator::make($request->all(), [
                'name' => 'required|regex:/^[A-Za-záàâãéèêíóôõúçÁÀÂÃÉÈÍÓÔÕÚÇ\s]+$/',
                'username' => 'required|alpha_num|unique:users,username',
                'type' => [
                    'required',
                    Rule::in(['waiter', 'cook', 'manager', 'cashier']),
                ],
                'photo_url' => 'required|regex:/^.+\.(jpe?g)?(png)?(bmp)?(svg)?(gif)?$/',
                'email' => 'required|email|unique:users,email',
            ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }
        $user = new User();
        $user->fill($request->except('photo_url'));
        $user->password = Hash::make('123'); //create with a default password
        
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

        $user->save();

        return response()->json(new UserResource($user), 201);
    }
}
