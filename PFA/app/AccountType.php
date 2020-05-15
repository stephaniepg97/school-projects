<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class AccountType extends Model
{
    protected $table = 'account_types';

    protected $fillable = [
        'name'
    ];

    public static function typeToStr($id) 
    {
        return AccountType::findOrFail($id)->name;
    }

    public static function size()
    {
    	return AccountType::orderBy('id', 'desc')->first()->id;
    }
}
