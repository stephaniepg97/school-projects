<?php

namespace App;

use Illuminate\Notifications\Notifiable;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Support\Collection;

class User extends Authenticatable
{
    use Notifiable;

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = [
        'name', 
        'email',  
        'created_at',   
        'updated_at',  
        'admin',    
        'blocked', 
        'phone',    
        'profile_photo',      
        'profile_settings', 
        'remember_token',
        'password'  
    ];

    protected $table = 'users';

    /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'password', 'remember_token',
    ];

    public function associates()
    {
        return $this->belongsToMany('App\User', 'associate_members', 'main_user_id', 'associated_user_id');
    } 

   
    public function getAssociates()
    {
        return User::findOrFail($this->id)->associates;
    }

    public function getAssociatesOf()
    {
        $associates = $this->getAssociates();

        $associatesOf = collect([]);

        if ($associates !== null)
        {
            foreach ($associates as $associate) 
            {
                $associatesOfAssociate = $associate->getAssociates();
                if ( $associatesOfAssociate !== null)
                {
                    $associatesOf = $associatesOf->merge($associatesOfAssociate);
                }
                
            }
        }

        $associatesOf = $associatesOf->diffAssoc($associates);

        
        return $associatesOf;
    }


    public function getAssociation($associate_id)
    {
        return $this->getAssociates()->wherePivot('associated_user_id', $associate_id);
    } 

    public static function next()
    {
        return User::orderBy('id', 'desc')->first()->id += 1;
    }

    public function blockedToStr()
    {
        switch ($this->blocked) {
            case 0:
                return 'No';
            case 1:
                return 'Yes';
        }
    }

    public function adminToStr()
    {
        switch ($this->admin) {
            case 0:
                return 'No';
            case 1:
                return 'Yes';
        }
    }

    public function hasMovement($movement_id)
    {
        if ($this->hasOpenedAccounts())
        {
            $accounts = $this->getOpenedAccounts();
        
            foreach ($account as $account) 
            {
                if ($account->hasMovement($movement_id))
                {
                    return true;
                }
            } 
        }
        return false;
    }
    
    public static function findPattern(Collection $users, $name)
    {
        $result = collect([]);

        if ($name !== null && $users != null)
        {
            foreach ($users as $user) 
            {
                $keywords = preg_split("/[\s,]+/", $user->name);
                foreach ($keywords as $keyword) 
                {
                    if (preg_match("[$name]", $keyword, $matches)) 
                    {
                        $result = $result->concat([$user]);
                    }
                }  
            } 
        }

        if ($result->isEmpty()) 
        {
            return $users;
        }

        return $result;
    }

    public static function find($name, $admin, $blocked) 
    { 

        $users = User::All();

        if ($name !== null) 
        {
            $users = User::findPattern($users, $name);
        }

        if ($blocked !== null) 
        {
            $users = $users->where('blocked', '=', $blocked);
        }

        if ($admin !== null) 
        {
            $users = $users->where('admin', '=', $admin);
        }

        return $users;
    }

    public function getTotalBalance()
    {
        $accounts = $this->getOpenedAccounts();

        $total_balance = 0.00;

        if ($accounts == null)
        {
            return $total_balance;
        }

        foreach ($accounts as $account) 
        {
            $total_balance += $account->current_balance;
        }

        return $total_balance;
    }

    public function getFirstDate()
    {
        $accounts = $this->getOpenedAccounts();

        if ($accounts->isEmpty())
        {
            return '';
        }

        $first_date = $accounts->first()->date;

        foreach ($accounts as $account) 
        {
            $date = $account->getFirstDate();
            if ($date !== null && $first_date > $date)
            {
                $first_date = $date;
            }
        }

        return $first_date;
    }

    public function getLastDate()
    {
        $accounts = $this->getOpenedAccounts();

        if ($accounts->isEmpty())
        {
            return '';
        }

        $last_date = $accounts->first()->date;

        foreach ($accounts as $account) 
        {
            $date = $account->getLastDate();
            if ($date !== null && $last_date < $date)
            {
                $last_date = $date;
            }
        }

        return $last_date;
    }

    public function hasOpenedAccounts()
    {
        return $this->getOpenedAccounts() !== null; 
    }

    public function getAccounts()
    {
        return Account::where('owner_id', $this->id)->withTrashed()->get();
    }

    public function getAccountsByType($account_type_id)
    {
        return Account::where('owner_id', $this->id)->where('account_type_id', $account_type_id)->get();
    }

    public function getOpenedAccounts()
    {
        return Account::where('owner_id', $this->id)->get();
    }

    public function getTrashedAccounts()
    {
        return Account::where('owner_id', $this->id)->onlyTrashed()->get();
    }

    public function getCategories()
    {
        $accounts = $this->getOpenedAccounts(); 

        if ($this->hasOpenedAccounts())
        {
            $categories = collect([]);
            foreach ($accounts as $account) 
            {
                $categories = $categories->merge([$account->getCategories()]);

            }
            return $categories->collapse()->unique()->values();
        }
    }

    public function isAssociated(User $associate)
    {
        return $this->getAssociates()->where('id', $associate->id) || $associateOf->getAssociates()->where('id', $this->id);
    }

    public function isAssociatedOf(User $associateOf)
    {
        return $this->getAssociatesOf()->where('id', $associateOf->id) || $associateOf->getAssociatesOf()->where('id', $this->id);
    }
}
