<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\SoftDeletes;

class Account extends Model
{
	use SoftDeletes;

    protected $table = 'accounts';

     protected $fillable = [
        'start_balance',
        'account_type_id', 
        'code',
  		'description',
        'date'
    ];

    const CREATED_AT = 'created_at';

    public $timestamps = false;

    protected $dates = ['created_at', 'deleted_at'];

	public function typeToStr()
	{
		return AccountType::findOrFail($this->account_type_id)->name;
	}

	public function refreshCurrentBalance()
	{
		if($this->hasMovements())
		{
			$this->current_balance = Movement::getLastBalance($this->id);
		}
		else
		{
			$this->current_balance = $this->start_balance;
		}
		$this->save();
	}

	public function refreshAllMovementsStartEndBalances($new_start_balance)
	{
		Movement::setStartEndBalances($new_start_balance, Movement::where('account_id', $this->id)->orderBy('date', 'asc')->orderBy('id', 'asc')->get());
	}

	public function hasMovements()
	{
		return (! Movement::orderByDateDesc($this->id)->isEmpty());
	}

	public function hasMovement($movement_id)
	{
		if ($this->hasMovements())
		{
			$movements = Movement::orderByDateDesc($this->id);
			return (! $movements->where('id', '=', $movement_id)->isEmpty());
		}
		return false;
	}


	public function weight()
	{
		$user = User::findOrFail($this->owner_id);
		$total_balance = $user->getTotalBalance();

		return round($this->current_balance/$total_balance, 2)*100;
	}

	public function getLastDate()
	{
		if ($this->hasMovements())
		{
			return Movement::where('account_id', $this->id)->orderBy('date', 'desc')->orderBy('id', 'desc')->first()->date;
		}
	}

	public function getFirstDate()
	{
		if ($this->hasMovements())
		{
			return Movement::where('account_id', $this->id)->orderBy('date', 'asc')->orderBy('id', 'asc')->first()->date;
		}
	}

	public function getCategories()
    {
        if ($this->hasMovements())
		{
			$movements = Movement::where('account_id', $this->id)->get();

			$account_categories = collect([]);

			foreach ($movements as $movement)
			{
				$account_categories = $account_categories->merge([$movement->category()]);
			}
			return $account_categories->unique()->values();
		}
    }

    public function user()
    {
    	return User::findOrFail($this->owner_id);
    }
}