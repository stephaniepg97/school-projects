<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use DateTime;

class Meal extends Model
{
	protected $fillable = [
		'state',
		'table_number',	
		'start',
		'end',
		'responsible_waiter_id',	
		'total_price_preview'
	];

	protected $table = 'meals';

    protected $dates = [
        'created_at',
        'updated_at'
    ];

	public function invoice()
    {
        return $this->hasOne('App\Invoice');
    }

    public function waiter()
    {
        return $this->belongsTo('App\User', 'responsible_waiter_id');
    }

    public function table()
    {
        return $this->belongsTo('App\Table', 'table_number', 'table_number');
    }

    public function orders()
    {
        return $this->hasMany('App\Order');
    }

    public function closeOrders()
    {
        $orders = $this->orders->where('state', '<>', 'delivered')->all();
        foreach ($orders as $order) 
        {
            $order->update(['state' => 'not delivered']);
        }
        return $orders;

    }

    public static function totalByYearMonth($date) 
    {
        $meals = Meal::where('start', 'like', $date.'%')->get();

        return $meals->count();
    }

    public static function averageByYearMonth($date) 
    {
        $avg = Meal::averageByYearMonthSeconds($date);

        return gmdate("H:i:s", $avg);
    }

    public static function averageByYearMonthSeconds($date) 
    {
        $meals = Meal::where('start', 'like', $date.'%')->get();

        if ($meals->isEmpty()) {
            return 0;
        }

        $seconds = 0;
        foreach ($meals as $key => $meal) {
            $end = new DateTime($meal->end);
            $start = new DateTime($meal->start);
            $diff = $end->diff($start);
            $seconds += $diff->h*3600 + $diff->i*60 + $diff->s;
        }

        return ceil($seconds/$meals->count());
    }

    public static function totals() {
        
        $first = new DateTime(Meal::min('start'));

        $last = new DateTime(Meal::max('start'));

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
            $total = Meal::totalByYearMonth($current_format);
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
    // the average time it takes to handle each meal (time between the moment a meal is created until the meal is closed – “paid” or “not paid”)
    public static function averages() {
        
        $first = new DateTime(Meal::min('start'));

        $last = new DateTime(Meal::max('start'));

        $averages = collect([]);
        $averages_per_month = collect([]);
        $current = $first;
        $year = $current->format('Y');

        while($current <= $last) {
            $new_year = $current->format('Y');
            if ($new_year != $year) {
                $averages = $averages->put($year, $averages_per_month);
                $averages_per_month = collect([]);
                $year = $new_year;
            }            
            $current_format = $current->format('Y-m');
            $month = $current->format('m');
            $average = Meal::averageByYearMonth($current_format);
            $averages_per_month = $averages_per_month->put($month, $average);
            if ($current_format == $last->format('Y-m')) {
                $averages = $averages->put($year, $averages_per_month);
                break;
            }
            $current = $current->modify( '+1 month' ); 
        }

        return $averages;
    }

    public static function graphicAverages() {
        
        $first = new DateTime(Meal::min('start'));

        $last = new DateTime(Meal::max('start'));

        $averages = collect([]);

        $current = $first;
        
        while($current <= $last) {
            $current_format = $current->format('Y-m');
            $average = Meal::averageByYearMonthSeconds($current_format);
            $averages = $averages->put($current_format, $average);
            $current = $current->modify( '+1 month' ); 
        }
        
        return $averages;
    }
}