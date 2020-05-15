<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Filesystem\Filesystem;
use Illuminate\Support\Facades\DB;
use App\Item;

class Invoice extends Model
{
	protected $fillable = [
		'state',
		'nif',
		'meal_id',
		'total_price',
		'date',
		'name'
	];

	protected $table = 'invoices';

	protected $dates = [
        'created_at',
        'updated_at'
    ];

	public function meal()
    {
        return $this->belongsTo('App\Meal');
    }

    public function table()
    {
        return ($this->meal->table != null) ? $this->meal->table->table_number : null;
    }

    public function waiter()
    {
        return ($this->meal->waiter != null) ? $this->meal->waiter->name : null;
    }

    public function items()
    {
        return $this->belongsToMany('App\Item', 'invoice_items');
    }

    public function item($id) 
    {
        return Item::find($id);
    } 

    public function receipt()
    {
        return DB::table('invoice_items')->where('invoice_id', $this->id)->get();
    }

    public function filename()
    {
        return $this->nif.'_'.$this->date.'.pdf';
    }

    public function path()
    {
        return storage_path('app/public/invoices/'.$this->filename());
    }

    public function hasFile()
    {
        $file = new Filesystem();

        return $file->exists($this->path());
    }

    public function getFile()
    {
        $file = new Filesystem();

        return $file->get($this->path());
    }

    public function deleteFile()
    {
         $file = new Filesystem();

         return $file->delete($this->path());
    }
}