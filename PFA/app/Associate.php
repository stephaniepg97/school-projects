<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Associate extends Model
{
    protected $table = 'associate_members';

    protected $fillable = [
        'associated_user_id', 
        'main_user_id',  
    ];

    const CREATED_AT = 'created_at';

    public $timestamps = false;

    public static function findAssociates($user_id) 
    { 
    	return Associate::where('main_user_id', $user_id)->get();
    }

    public static function getAssociation($main_user_id, $associated_user_id) 
    {
    	return Associate::where('main_user_id', $main_user_id)->where('associated_user_id', $associated_user_id)->get();
    }

    public static function deleteAssociation($main_user_id, $associated_user_id) 
    {
        Associate::where('main_user_id', $main_user_id)->where('associated_user_id', $associated_user_id)->delete();
    }
}
