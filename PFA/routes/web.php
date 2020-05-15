<?php

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::get('/', function () {
    return view('welcome');
})->name('/');

Auth::routes(); //US. 1-3

Route::middleware(['auth'])->group(function () {

	//HOME
	Route::get('/home', 'HomeController@index')->name('home');

	//DASHBOARD
	Route::get('/dashboard/{user}', 'DashboardController@index')->name('dashboard');

	//USERS
	Route::get('/profiles', 'UserController@index')->name('profiles'); //US.11

	Route::get('/me', 'UserController@profile')->name('profile'); //US.11

	// Renders the profile form	
	Route::get('/me/profile/{user}', 'UserController@editProfile')->name('edit-profile');
	// Posts the profile form
	Route::put('/me/profile', 'UserController@updateProfile')->name('update-profile');

	Route::get('/me/password/{user}', 'UserController@editPassword')->name('edit-password');
	
	Route::patch('/me/password', 'UserController@updatePassword')->name('update-password');  //US. 9

	Route::get('/me/associates/{user}', 'UserController@getAssociates')->name('get-associates'); //US.12

	Route::get('/me/associate-of/{user}', 'UserController@getAssociatesOf')->name('get-associatesOf'); //US.13

	Route::get('/me/associates/edit/{user}', 'UserController@editAssociates')->name('edit-associates');
	
	//ASSOCIATES
	Route::post('/me/associates', 'UserController@storeAssociate')->name('add-associate');

	Route::delete('/me/associates/{user}', 'UserController@destroyAssociate')->name('destroy-associate'); //US.30

	//ACCOUNTS
	Route::name('accounts.')->group(function () {

		Route::get('/accounts/{user}','AccountController@index')->name('index');
		
		Route::get('/accounts/{user}/opened', 'AccountController@opened')->name('opened');

		Route::get('/accounts/{user}/closed', 'AccountController@closed')->name('closed');

		Route::get('/account/{user}/create', 'AccountController@create')->name('create');

		Route::post('/account', 'AccountController@store')->name('store');

		Route::get('/account/{account}', 'AccountController@edit')->name('edit');
		
		Route::put('/account/{account}', 'AccountController@update')->name('update');

		Route::delete('/account/{account}','AccountController@destroy')->name('destroy');

		Route::patch('/account/{account}/close','AccountController@close')->name('close');

		Route::patch('/account/{account}/reopen','AccountController@reopen')->name('reopen');
	});

	//MOVEMENTS
	Route::name('movements.')->group(function () {
		//listar movimentos
		Route::get('/movements/{account}', 'MovementController@index')->name('index');

		//mostra form de adição
		Route::get('/movements/{account}/create', 'MovementController@create')->name('create');

		//criar um movimento
		Route::post('/movements/{account}/create', 'MovementController@store')->name('store');

		//mostra form de edição
		Route::get('/movement/{movement}', 'MovementController@edit')->name('edit');
		//atualiza
		Route::put('/movement/{movement}', 'MovementController@update')->name('update');

		//apagar um determinado movimento
		Route::delete('/movement/{movement}', 'MovementController@destroy')->name('destroy');
	});

	//DOCUMENTS
	Route::name('documents.')->group(function () {

		Route::get('/document/{document}', 'DocumentController@download')->name('download');

		Route::get('/document/{document}/view', 'DocumentController@view')->name('view');
		
		Route::get('/document/{document}/details', 'DocumentController@show')->name('show');

		Route::get('/documents/{movement}/create', 'DocumentController@create')->name('create');

		Route::post('/documents/{movement}', 'DocumentController@store')->name('store');

		Route::delete('/document/{document}', 'DocumentController@destroy')->name('destroy');
	});

	Route::middleware(['admin'])->name('admin.')->group(function () {

		Route::get('/users', 'AdministratorController@index')->name('users'); //US. 6

		Route::patch('/users/{user_id}/block', 'AdministratorController@block')->name('block'); //US. 7

		Route::patch('/users/{user_id}/unblock', 'AdministratorController@unblock')->name('unblock'); //US. 7

		Route::patch('/users/{user_id}/promote', 'AdministratorController@promote')->name('promote'); //US. 7

		Route::patch('/users/{user_id}/demote', 'AdministratorController@demote')->name('demote'); //US. 7
	});

});