<?php

namespace App;

use Illuminate\Notifications\Notifiable;
use Illuminate\Database\Eloquent\Model;
use App\MovementCategory;

class Movement extends Model
{
    protected $fillable = [
        'movement_category_id', 'date', 'value', 'created_at', 'account_id',
    ];

    const CREATED_AT = 'created_at';

    public $timestamps = false;

    protected $guarded = ['file', 'document_description'];

    protected $table = 'movements';


      /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */

    public function categoryToStr() 
    {
        return MovementCategory::findOrFail($this->movement_category_id)->name;
    }

    public function category() 
    {
        return MovementCategory::findOrFail($this->movement_category_id);
    }

    public function getLastMovement()
    {
        $movements = Movement::where('account_id', '=', $this->account_id)->where('date', '<=', $this->date)->orderBy('date', 'desc')->orderBy('id', 'desc')->get();

        $find_next = false;
        
        foreach ($movements as $movement) {

            if ($find_next)
            {
                //dd($movement, $this);
                return $movement;
            }

            if ($movement->id == $this->id)
            {
                $find_next = true;
            }   
        }
    }

    public function setValue($value)
    {
        if ($this->type == 'expense') 
        {
            $this->value = -$value;
            return;
        }
        $this->value = $value;
    }

    public static function orderByDateDesc($account_id)
    {
        return Movement::where('account_id', $account_id)->orderBy('date', 'desc')->orderBy('id', 'desc')->get();
    }

    public static function getFirstBalance($account_id)
    {
        $movement = Movement::where('account_id', $account_id)->orderBy('date', 'asc')->orderBy('id', 'asc')->first();

        if ($movement == null)
        {
            return Account::findOrFail($account_id)->start_balance;
        }
        
        return $movement->start_balance;
    }

    public static function getLastBalance($account_id)
    {
        $movement = Movement::where('account_id', $account_id)->orderBy('date', 'desc')->orderBy('id', 'desc')->first();

        if ($movement == null)
        {
            return Account::findOrFail($account_id)->start_balance;
        }
        return $movement->end_balance;
    }

    public static function setStartEndBalances($start_balance, $movements) 
    {
        if ($start_balance == null) 
        {
            $start_balance = $this->account()->start_balance;
        }
        
        foreach ($movements as $movement)
        {
            $movement->start_balance = $start_balance; 
            $movement->end_balance = $movement->start_balance + $movement->value; 
            $movement->update();  
            $start_balance = $movement->end_balance;
        }
    }

    public function getMovimentsSince()
    {
        $movements = Movement::where('account_id', $this->account_id)->where('date', '>', $this->date)->orderBy('date', 'asc')->orderBy('id', 'asc')->get();

        $movements_of_the_day = Movement::where('account_id', $this->account_id)->where('date', '=', $this->date)->orderBy('id', 'asc')->get();

        $movements_after_of_the_day = collect([]);

        $find_next = false;
            
        foreach ($movements_of_the_day as $movement) 
        {

            if ($find_next)
            {
                $movements_after_of_the_day = $movements_after_of_the_day->merge([$movement]);
            }

            if ($movement->id == $this->id)
            {
                $find_next = true;
            }
        }

        if ($movements_after_of_the_day->isEmpty())
        {
            return $movements;
        }
        
        return $movements_after_of_the_day->merge([$movements])->collapse();
    }

    public function getLastBalanceUntil()
    {
        $last_movement = $this->getLastMovement();

        if ($last_movement == null)
        {
            return $this->account()->start_balance;
        }
        return $last_movement->end_balance;
    }

    public function hasDocument()
    {
        return $this->document_id !== null;
    }

    public function account()
    {
        return Account::findOrFail($this->account_id);
    }

    public function document()
    {
        return Document::findOrFail($this->document_id);
    }

    public function user()
    {     
        return User::findOrFail($this->account()->owner_id);
    }
}
