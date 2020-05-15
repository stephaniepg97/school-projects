<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Account;
use App\AccountType;
use App\User;
use App\Movement;
use App\Http\Requests\StoreAccountRequest;
use App\Http\Requests\UpdateAccountRequest;
use Illuminate\Support\Facades\Auth;

class AccountController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */


    public function __construct()
    {
        $this->middleware('auth');
    }


    public function index(User $user, Request $request)
    {
        $this->authorize('viewAccounts', $user);

        $account_type_id = $request->input('account_type_id');
        
        if ($account_type_id == null) 
        {
            $accounts = $user->getAccounts();
        }
        else 
        {
            $accounts = $user->getAccountsByType($account_type_id);
        }
        
        $types = AccountType::all();
        return view('accounts.index', compact('accounts', 'user', 'types'));
    }

    public function opened(User $user, Request $request)
    {
        $this->authorize('viewAccounts', $user);
        $accounts = $user->getOpenedAccounts();
        $types = AccountType::all();
        return view('accounts.index', compact('accounts', 'user', 'types'));
    }

    public function closed(User $user, Request $request)
    {
        $this->authorize('viewAccounts', $user);
        $accounts = $user->getTrashedAccounts();
        $types = AccountType::all();
        return view('accounts.index', compact('accounts', 'user', 'types'));
    }

    /**
     * Show the form for creating a new resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function create(User $user)
    {
        $this->authorize('createAccount', User::class);
        $account = new Account;
        $types = AccountType::all();
        return view('accounts.add', compact('account', 'user', 'types'));
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(StoreAccountRequest $request)
    {  
        $this->authorize('createAccount', User::class);
        $data = $request->validated();

        $account = new Account;
        $account->fill($data);
        $account->owner_id = $request['owner_id'];

        //save with current_balance
        $account->refreshCurrentBalance();

        return redirect()
            ->route('accounts.index', $account->owner_id)
            ->with('success', 'Account updated successfully.');
    }

    /**
     * Display the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        //
    }

    /**
     * Show the form for editing the specified resource.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function edit(Account $account)
    {
        $user = $account->user();
        $this->authorize('editAccount', $user);
        $types = AccountType::all();
        return view('accounts.edit', compact('account', 'types'));
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(UpdateAccountRequest $request, Account $account)
    {
        $user = $account->user();
        $this->authorize('editAccount', $user);

        $data = $request->validated();
        $account->start_balance = $data['start_balance'];
        if ($account->hasMovements()) 
        {
            $account->refreshAllMovementsStartEndBalances($data['start_balance']);
        }
        $account->refreshCurrentBalance();

        return redirect()
            ->route('accounts.index', $account->owner_id)
            ->with('success', 'Account updated successfully.');
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy(Account $account)
    {
        $user = $account->user();
        $this->authorize('editAccount', $user);

        if ($account->hasMovements())
        {
            return redirect()
            ->route('accounts.index', $account->owner_id)
            ->with('unsuccess', 'Account has movements. Impossible to delete.');
        }

        $account->forceDelete();

        return redirect()
            ->route('accounts.index', $account->owner_id)
            ->with('success', 'Account deleted successfully.');
        
    }

    public function close(Account $account)
    {
        $user = $account->user();
        $this->authorize('closeAccount', $user);

        $account->delete();

         if (! $account->trashed()) {
            return back()->with('unsuccess', 'Operation not done successfully.');
        }

        return redirect()
            ->route('accounts.index', $account->owner_id)
            ->with('success', 'Account closed successfully.');
        
    }

    public function reopen(Account $account)
    {
        $user = $account->user();
        $this->authorize('reopenAccount', $user);

        return redirect()
            ->route('accounts.index', $account->owner_id)
            ->with('success', 'Account restored successfully.');   
    }
}
