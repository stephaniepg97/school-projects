<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\SoftDeletes;

class Table extends Model
{
	use SoftDeletes;
	
	protected $dates = ['created_at', 'deleted_at', 'updated_at'];

	protected $fillable = [
		'table_number'
	];

	protected $table = 'restaurant_tables';

	static function findTable($id) 
	{
		return Table::where('table_number', $id)->first();
	}

	static function isAvaiable($id) 
	{
		$table = Table::findTable($id);
		return ($table->meal() == null);
	}

	public function meals()
    {
        return $this->hasMany('App\Meal', 'table_number', 'table_number');
    }

    public function meal()
    {
    	$meal = $this->meals->where('state', 'active')->first();
        if ($meal == null)
        {
        	$meal = $this->meals->where('state', 'terminated')->first();	
        }
        return $meal;
    }
}