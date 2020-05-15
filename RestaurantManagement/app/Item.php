<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Filesystem\Filesystem;
use App\Order;
use DateTime;

class Item extends Model
{
	use SoftDeletes;

	protected $fillable = [
		'name',	
		'type',		
		'description',	
		'photo_url',		
		'price'
	];

	protected $table = 'items';

	public $timestamps = false;

	protected $dates = ['created_at', 'deleted_at', 'updated_at'];

	public function path()
    {
    	return storage_path('app/public/items/'.$this->photo_url);
    }

	public function hasPhoto()
    {
        $photo = new Filesystem();

        return $photo->exists($this->path());
    }

    public function getPhoto()
    {
        $file = new Filesystem();

        return $file->get($this->path());
    }

    public function deletePhoto()
    {
         $file = new Filesystem();

         return $file->delete($this->path());
    }

    public function invoices()
    {
    	return $this->belongsToMany('App\Invoice', 'invoice_items', 'item_id', 'invoice_id')->withPivot('quantity', 'unit_price', 'sub_total_price');
    }

    public function orders()
    {
        return $this->hasMany('App\Order');
    }

    public function averageByYearMonth($date) 
    {
        $avg = $this->averageByYearMonthSeconds($date); 
        
        return gmdate("H:i:s", $avg);
    }

    public function averageByYearMonthSeconds($date) 
    {
        $orders = Order::where('start', 'like', $date.'%')->where('item_id', $this->id)->get();

        if ($orders->isEmpty()) {
            return 0;
        }

        $seconds = 0;
        foreach ($orders as $key => $order) {
            $end = new DateTime($order->end);
            $start = new DateTime($order->start);
            $diff = $end->diff($start);//->format('%H:%I:%S');
            $seconds += $diff->h*3600 + $diff->i*60 + $diff->s;
        }

        return ceil($seconds/$orders->count());
    }

    // the average time it takes to handle each meal (time between the moment a meal is created until the meal is closed – “paid” or “not paid”)
    public function averages() {
        
        $first = new DateTime(Order::min('start'));

        $last = new DateTime(Order::max('start'));

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
            $month = $current->format('n');
            $average = $this->averageByYearMonth($current_format);
            $averages_per_month = $averages_per_month->put($month, $average);
            if ($current_format == $last->format('Y-m')) {
                $averages = $averages->put($year, $averages_per_month);
                break;
            }
            $current = $current->modify( '+1 month' ); 
        }
        
        return $averages;
    }

    public function graphicAverages() {
        
        $first = new DateTime(Order::min('start'));

        $last = new DateTime(Order::max('start'));

        $averages = collect([]);

        $current = $first;
        
        while($current <= $last) {
            $current_format = $current->format('Y-m');
            $average = $this->averageByYearMonthSeconds($current_format);
            $averages = $averages->put($current_format, $average);
            $current = $current->modify( '+1 month' ); 
        }
        
        return $averages;
    }

}
