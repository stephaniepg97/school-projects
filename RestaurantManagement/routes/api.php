<?php

use Illuminate\Http\Request;
use App\User;
use App\Item;
use App\Http\Resources\User as UserResource;
use App\Http\Resources\Item as ItemResource;

/*
|--------------------------------------------------------------------------
| API Routes
|--------------------------------------------------------------------------
|
| Here is where you can register API routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| is assigned the "api" middleware group. Enjoy building your API!
|
*/

Route::namespace('Auth')->group(function () {
    // Controllers Within The "App\Http\Controllers\Admin" Namespace
    Route::middleware('auth:api')->post('logout','LoginControllerAPI@logout');
    Route::post('login', 'LoginControllerAPI@login')->name('login');
    Route::post('register','RegisterControllerAPI@register')->name('register');
    Route::get('resend','RegisterControllerAPI@resend')->name('resend');
});

Route::post('items', 'ItemControllerAPI@store');
Route::put('items/{id}', 'ItemControllerAPI@update');
Route::delete('items/{id}', 'ItemControllerAPI@destroy');
Route::patch('items/{id}/trash', 'ItemControllerAPI@trash');
Route::get('items', 'ItemControllerAPI@index');
Route::get('items/{id}/orders/averages', 'ItemControllerAPI@averages');
Route::get('items/{id}', 'ItemControllerAPI@show');


Route::middleware('auth:api')->get('users/me', 'UserControllerAPI@me');
Route::post('users', 'UserControllerAPI@create');
Route::delete('users/{id}', 'UserControllerAPI@destroy');
Route::patch('users/{id}/password', 'UserControllerAPI@password');
Route::patch('users/{id}/trash', 'UserControllerAPI@trash');
Route::patch('users/{id}/block', 'UserControllerAPI@block');
Route::patch('users/{id}/unblock', 'UserControllerAPI@unblock');
Route::patch('users/{id}/shift-off', 'UserControllerAPI@end');
Route::patch('users/{id}/shift-on', 'UserControllerAPI@start');
Route::put('users/{id}', 'UserControllerAPI@update');

Route::get('users/emailavailable', 'UserControllerAPI@emailAvailable');
Route::get('users', 'UserControllerAPI@index');
Route::get('users/{id}/orders/averages', 'UserControllerAPI@averages');
Route::get('users/{id}/orders', 'UserControllerAPI@orders');
Route::get('users/{id}/meals', 'UserControllerAPI@meals');
Route::get('users/{id}', 'UserControllerAPI@show');

Route::post('tables', 'TableControllerAPI@store');
Route::delete('tables', 'TableControllerAPI@destroy');
Route::patch('tables/{id}/trash', 'TableControllerAPI@trash');
Route::get('tables', 'TableControllerAPI@index');
Route::get('tables/{id}', 'TableControllerAPI@show');

Route::post('meals', 'MealControllerAPI@store');
Route::patch('meals/{id}/terminated', 'MealControllerAPI@terminate');
Route::patch('meals/{id}/not-paid', 'MealControllerAPI@notPaid');
Route::delete('meals/{id}', 'MealControllerAPI@destroy');
Route::get('meals/orders/averages', 'MealControllerAPI@averages');
Route::get('meals/totals', 'MealControllerAPI@totals');
Route::get('meals', 'MealControllerAPI@index');
Route::get('meals/{id}/orders', 'MealControllerAPI@orders');
Route::get('meals/{id}', 'MealControllerAPI@show');

Route::post('invoices', 'InvoiceControllerAPI@store');
Route::delete('invoices/{id}', 'InvoiceControllerAPI@destroy');
Route::patch('invoices/{id}/not-paid', 'InvoiceControllerAPI@notPaid');
Route::patch('invoices/{id}/do-payment', 'InvoiceControllerAPI@pay');
Route::get('invoices', 'InvoiceControllerAPI@index');
Route::get('invoices/{id}/items', 'InvoiceControllerAPI@items');
Route::get('invoices/{id}/pdf', 'InvoiceControllerAPI@pdf');
Route::get('invoices/{id}', 'InvoiceControllerAPI@show');

Route::post('orders', 'OrderControllerAPI@store');
Route::delete('orders/{id}', 'OrderControllerAPI@destroy');
Route::patch('orders/{id}/in-preparation', 'OrderControllerAPI@inPreparation');
Route::patch('orders/{id}/prepared', 'OrderControllerAPI@prepare');
Route::patch('orders/{id}/delivered', 'OrderControllerAPI@deliver');
Route::patch('orders/{id}/confirmed', 'OrderControllerAPI@confirm');
Route::patch('orders/{id}', 'OrderControllerAPI@update');
Route::get('orders/totals', 'OrderControllerAPI@totals');
Route::get('orders', 'OrderControllerAPI@index');
Route::get('orders/{id}', 'OrderControllerAPI@show');
