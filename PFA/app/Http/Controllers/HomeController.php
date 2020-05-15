<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;

class HomeController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index(Request $request)
    {
     	return redirect()
            ->route('dashboard', Auth::user())
            ->with('success', 'You are logged in!');
    }

}
