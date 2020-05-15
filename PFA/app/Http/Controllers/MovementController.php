<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreMovementRequest;
use App\Http\Requests\UpdateMovementRequest;
use App\Movement;
use App\Account;
use App\Document;
use App\User;
use App\MovementCategory;
use Illuminate\Http\Request;
use Illuminate\Support\Collection;
use Illuminate\Filesystem\Filesystem;

class MovementController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index(Account $account)
    {
        $user = $account->user();
        $this->authorize('viewMovements',  $user);

        $movements = Movement::orderByDateDesc($account->id);
        return view('movements.index', compact('movements', 'account'));
    }

    public function create(Account $account)
    {
        $user = $account->user();
        $this->authorize('createMovement',  $user);

        $movement = new Movement;
        $categories = MovementCategory::all();
        return view('movements.add', compact('movement', 'categories', 'account'));
    }

    public function store(StoreMovementRequest $request, Account $account)
    {
        $user = $account->user();
        $this->authorize('createMovement',  $user);

        $data = $request->validated();

        $movement = new Movement;
        $movement->fill($data);
        $movement->account_id = $account->id;
        $movement->type = MovementCategory::findOrFail($data['movement_category_id'])->type;
        $movement->setValue($data['value']); 

        $movement->start_balance = $movement->end_balance = 0;

        //save instance
        $movement->save();

        $movement->start_balance = $movement->getLastBalanceUntil();
        $movement->end_balance = $movement->start_balance + $movement->value; 

        $movements = $movement->getMovimentsSince();
        Movement::setStartEndBalances($movement->end_balance, $movements);
        $movement->account()->refreshCurrentBalance();

        $stored_file = null;

        if ($request->hasFile('file') && $request->file('file')->isValid()) 
        {
            $type = $data['file']->extension();
            $document = new Document;
            $document->fill([
                'type' => $type,     
                'original_name' => $movement->id,
                'description' => $data['document_description'],
            ]);

            //name document with new movement id
            $document->original_name = $movement->id;

            //save new document
            $document->save();

            //define document_id
            $movement->document_id = $document->id;

            //store document
            $stored_file = $request->file('file')->storeAs('documents/'.$movement->account_id, $movement->id.'.'.$document->type);
        }

        //save instance
        $movement->update();

        $message = 'Movement added successfully.';

        if ($stored_file !== null)
        {
            $message = $message.' File stored successfully.';

        }

        return redirect()
            ->route('movements.index', $account->id)
            ->with('success', $message);
    }

     public function edit(Movement $movement)
    {
        $categories = MovementCategory::all();
        return view('movements.edit', compact('movement', 'categories'));
    }

     public function update(UpdateMovementRequest $request, Movement $movement)
    {
        $user = $movement->user();
        $this->authorize('editMovement',  $user);

        $data = $request->validated();

        $file_moved = false;

        $old_movement = $movement;
        $movement = new Movement;

        $movement->account_id = $old_movement->account_id;
        $movement->movement_category_id = $data['movement_category_id'];
        $movement->type = MovementCategory::findOrFail($data['movement_category_id'])->type;
        $movement->setValue($data['value']);
        $movement->date = $data['date']; 

        $movement->start_balance = $movement->end_balance = 0;

        //save instance
        $movement->save();

        $movement->start_balance = $movement->getLastBalanceUntil();
        $movement->end_balance = $movement->start_balance + $movement->value; 

        if ($old_movement->date > $movement->date) {
            $movements = $movement->getMovimentsSince();
            Movement::setStartEndBalances($movement->end_balance, $movements);
        }

        if ($old_movement->hasDocument())
        {
            //find document
            $document = $old_movement->document();

            if ($document->hasFile())
            {
                //get file
                $old_path = $document->path();

                //update document_id
                $movement->document_id = $document->id;

                //rename document with new movement id
                $document->original_name = $movement->id;

                //update document
                $document->update();

                //move file on the right place on storage (with the old id of the movement) 
                $file = new Filesystem;

                $file_moved = $file->move($old_path, $document->path());
            }
        }        
        
        $account = $movement->account();
        $account->refreshCurrentBalance();

        //save instance
        $movement->update();

        $message = 'Movement updated successfully.';

        if ($file_moved)
        {
            $message = $message.' File moved from storage.';
        }

        //delete old moviment
        $movements = $old_movement->getMovimentsSince();
        $last_movement = $old_movement->getLastMovement();

        if ($last_movement == null)
        {
            $start_balance = $account->start_balance;
        }
        else
        {
            $start_balance = $last_movement->end_balance;
        }


        Movement::setStartEndBalances($start_balance, $movements);
        $account->refreshCurrentBalance();
        
        $old_movement->delete();

        return redirect()
            ->route('movements.index', $movement->account_id)
            ->with('success', $message);
    }

    public function destroy(Movement $movement)
    {
        $user = $movement->user();
        $this->authorize('editMovement',  $user);

        $document_deleted = false;
        $file_deleted = false;

        if ($movement->hasDocument())
        {
            $document = $movement->document();

            if ($document->hasFile())
            {
                $file_deleted = $document->deleteFile();
            }

            $movement->document_id = null;
            $movement->update();

            $document_deleted = $document->delete();
        }

        $message = 'Movement deleted successfully.';

        if ($document_deleted)
        {
            $message = $message.' Document deleted successfully.';
        }

        if ($file_deleted)
        {
            $message = $message.' File deleted from storage';
        }

        $movements = $movement->getMovimentsSince();
        $last_movement = $movement->getLastMovement();

        $account = $movement->account();

        if ($last_movement == null)
        {
            
            $start_balance = $account->start_balance;
        }
        else
        {
            $start_balance = $last_movement->end_balance;
        }

        Movement::setStartEndBalances($start_balance, $movements);
        $account->refreshCurrentBalance();
        
       
        $movement->delete();

        return redirect()
            ->route('movements.index', $movement->account_id)
            ->with('success', $message);
    }
}
