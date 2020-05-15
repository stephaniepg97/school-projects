<?php

namespace App;

use Laravel\Passport\HasApiTokens;
use Illuminate\Notifications\Notifiable;
use Illuminate\Contracts\Auth\MustVerifyEmail;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Database\Eloquent\SoftDeletes;
use App\Http\Resources\Order as OrderResource;
use App\Order;
use DateTime;
use Illuminate\Http\UploadedFile;
use Illuminate\Support\Facades\Storage;
use Illuminate\Filesystem\Filesystem;
use Illuminate\Support\Collection;

class User extends Authenticatable implements MustVerifyEmail
{
    use HasApiTokens, Notifiable, SoftDeletes;

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    //protected $dates = ['deleted_at', 'created_at', 'updated_at'];
    
    protected $fillable = [
        'name', 
        'username',
        'email', 
        'email_verified_at',
        'password',
        'type',
        'blocked',
        'photo_url',
        'last_shift_start',
        'last_shift_end',
        'shift_active'
    ];

    /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'password', 'remember_token'
    ];

    public function path()
    {
        return storage_path('app/public/profiles/'.$this->photo_url);
    }
    
    public function hasPhoto()
    {
        $file = new Filesystem();

        return $file->exists($this->path());
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

    public static function isWaiter($id)
    {
        $user = User::findOrFail($id);
        return ($user->type == 'waiter');
    }

    public static function isCook($id)
    {
        $user = User::findOrFail($id);
        return ($user->type == 'cook');
    }

    public function meals()
    {
        return $this->hasMany('App\Meal', 'responsible_waiter_id');
    }

    public function orders()
    {
        return $this->hasMany('App\Order', 'responsible_cook_id');
    }

    public function numOrdersByDate($date) {
        $orders = [];

        if ($this->type == 'cook') {
            $orders = Order::where('start', 'like', $date.'%')->where('responsible_cook_id', $this->id)->get();
        }

        if ($this->type == 'waiter') {
            $id = $this->id;
            $orders = Order::whereHas('meal', function ($query) use ($id) {
                $query->where('responsible_waiter_id', $id);
            })->where('start', 'like', $date.'%')->get();
        } 

        return $orders->count();
    }

    public function averageOrdersByDate($date)
    {
        //the average number of orders handled by day:
        $total = Order::where('start', 'like', $date.'%')->get()->count();
        if ($total) {
            $sum = $this->numOrdersByDate($date);
            $average = round($sum*100/$total, 2);
            return $average;
        }
        return 0.00;
    }

    public function firstOrderDate()
    {
        if ($this->type == 'cook') {
            return $this->orders->min('start');
        }

        if ($this->type == 'waiter') {
            $id = $this->id;
            return Order::whereHas('meal', function ($query) use ($id) {
                $query->where('responsible_waiter_id', $id);
            })->min('start');
        }   

        return null;
    }

    public function lastOrderDate()
    {
        if ($this->type == 'cook') {
            return $this->orders->max('start');
        }

        if ($this->type == 'waiter') {
            $id = $this->id;
            return Order::whereHas('meal', function ($query) use ($id) {
                $query->where('responsible_waiter_id', $id);
            })->max('start');
        }   

        return null;
    }

    public function averages()
    {
        //array of all averages number of orders handled by day:
        
        $first = new DateTime($this->firstOrderDate());

        $last = new DateTime($this->lastOrderDate());

        $averages = collect([]);
        $averages_per_month = collect([]);
        $averages_per_day = collect([]);
        $current = $first;
        $year = $current->format('Y');
        $month = $current->format('n');

        while($current->format('Y-m-d') <= $last->format('Y-m-d')) {
            $new_year = $current->format('Y');
            $new_month = $current->format('n'); 
            if ($new_month != $month) {
                $averages_per_month = $averages_per_month->put($month, $averages_per_day);
                $averages_per_day = collect([]);
                $month = $new_month;
            }
            if ($new_year != $year) {
                $averages = $averages->put($year, $averages_per_month);
                $averages_per_month = collect([]);
                $year = $new_year;
            }    
            $current_format = $current->format('Y-m-d');
            $average = $this->averageOrdersByDate($current_format);
            $day = $current->format('j');
            $averages_per_day = $averages_per_day->put($day, $average);
            if ($current_format == $last->format('Y-m-d')) {
                $averages_per_month = $averages_per_month->put($month, $averages_per_day);
                $averages = $averages->put($year, $averages_per_month);
                break;
            }
            $current = $current->modify( '+1 day' ); 
        }

        return $averages;
    }

    public function graphicAverages()
    {
        //array of all averages number of orders handled by day:
        
        $first = new DateTime($this->firstOrderDate());

        $last = new DateTime($this->lastOrderDate());

        $averages = collect([]);

        $current = $first;

        while($current <= $last) {
            $current_format = $current->format('Y-m-d');
            $average = $this->averageOrdersByDate($current_format);
            $averages = $averages->put($current_format, $average);
            $current = $current->modify( '+1 day' ); 
        }

        return $averages;
    }
}
