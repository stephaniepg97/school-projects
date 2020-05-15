<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\User;
use Illuminate\Support\Facades\Auth;

class AdministratorController extends Controller
{
    public function __construct()
    {
        $this->middleware('admin');
    }

     public function index(Request $request)
    {
        $users = User::find($request['name'], $request['isAdmin'], $request['isBlocked']);       
       
        $associates = Auth::user()->getAssociates();

        $associatesOf = Auth::user()->getAssociatesOf();

        return view('users.index', compact('users', 'associates', 'associatesOf'));

    }

    public function promote($user_id)
    {
        $user = User::findOrFail($user_id);
        $user->admin = '1';
        $user->save();

         return redirect()
            ->route('admin.users')
            ->with('success', 'User updated successfully.');
    }

    public function demote($user_id)
    {
        $user = User::findOrFail($user_id);
        $user->admin = '0';
        $user->save();

         return redirect()
            ->route('admin.users')
            ->with('success', 'User updated successfully.');
    }

    public function block($user_id)
    {
        $user = User::findOrFail($user_id);
        $user->blocked = '1';
        $user->save();

         return redirect()
            ->route('admin.users')
            ->with('success', 'User updated successfully.');
    }

    public function unblock($user_id)
    {
        $user = User::findOrFail($user_id);
        $user->blocked = '0';
        $user->save();

         return redirect()
            ->route('admin.users')
            ->with('success', 'User updated successfully.');
    }
}
