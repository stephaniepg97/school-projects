<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Collection;

class MovementCategory extends Model
{
    protected $table = 'movement_categories';

    protected $fillable = [
        'name',
        'type'
    ];

    public function typeToStr() 
    {
        return MovementCategory::findOrFail($this->id)->type;
    }

    public function nameToStr() 
    {
        return MovementCategory::findOrFail($this->id)->name;
    }

    public static function size()
    {
        return MovementCategory::orderBy('id', 'desc')->first()->id;
    }

    public function total($start_date, $end_date, Account $account)
    {
        $movements = Movement::where('account_id', $account->id)->where('movement_category_id', $this->id)->where('date', '>=', $start_date)->where('date', '<=', $end_date)->orderBy('date', 'asc')->orderBy('id', 'asc')->get();

        $total = 0.00;

        if ($movements->isEmpty())
        {
            return $total;
        }

        foreach ($movements as $movement) 
        {
            $total += $movement->value;    
        }
      
        return $total;
    }

    public function finalTotal($start_date, $end_date, Collection $accounts)
    {
        $total = 0.00;
        foreach ($accounts as $account) 
        {
            $total += $this->total($start_date, $end_date, $account);
        }
        return $total;
    }
}
