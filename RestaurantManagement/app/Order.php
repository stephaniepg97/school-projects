<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use DateTime;

class Order extends Model
{
	protected $fillable = [
		'state',
		'item_id',
		'meal_id',
		'responsible_cook_id',
		'start',
		'end',
        'seasoning'
	];

    protected $dates = [
        'created_at',
        'updated_at'
    ];

	protected $table = 'orders';

	public function item()
    {
        return $this->belongsTo('App\Item', 'item_id');
    }

    public function meal()
    {
        return $this->belongsTo('App\Meal', 'meal_id');
    }

    public function cook()
    {
        return $this->belongsTo('App\User', 'responsible_cook_id');
    }

    public static function totalByYearMonth($date) 
    {
        $orders = Order::where('start', 'like', $date.'%')->get();

        return $orders->count();
    }

    public static function totals() {
        
        $first = new DateTime(Order::min('start'));

        $last = new DateTime(Order::max('start'));

        $totals = collect([]);
        $totals_per_month = collect([]);
        $current = $first;
        $year = $current->format('Y');

        while($current <= $last) {
            $new_year = $current->format('Y');
            if ($new_year != $year) {
                $totals = $totals->put($year, $totals_per_month);
                $totals_per_month = collect([]);
                $year = $new_year;
            }            
            $current_format = $current->format('Y-m');
            $total = Order::totalByYearMonth($current_format);
            $month = $current->format('m');
            $totals_per_month = $totals_per_month->put($month, $total);
            if ($current_format == $last->format('Y-m')) {
                $totals = $totals->put($year, $totals_per_month);
                break;
            }
            $current = $current->modify( '+1 month' ); 
        }

        return $totals;
    }
}