<?php

namespace App\Policies;

use App\User;
use App\Account;
use Illuminate\Auth\Access\HandlesAuthorization;
use Illuminate\Support\Facades\Auth;

class UserPolicy
{
    use HandlesAuthorization;

    public function viewAccounts(User $user, User $model)
    {
        return $model->isAssociatedOf($user) || $model->isAssociated($user) || $user->id === $model->id;
    }

    public function reopenAccount(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function closeAccount(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function createAccount(User $user)
    {
        return true;
    }

    public function addAccount(User $user, User $model)
    {
        return $user->id === $model->id;
    }

     public function addMovement(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function editAccount(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function destroyAccount(User $user, User $model)
    {
        return $user->id === $model->id;
    }


    public function index(User $user)
    {
        return true;
    }

    public function viewMovements(User $user, User $model)
    {
        return $model->isAssociated($user) || $user->id === $model->id;
    }

    public function viewDocument(User $user, User $model)
    {
        return $model->isAssociated($user) || $user->id === $model->id;
    }

    public function storeDocument(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function destroyDocument(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function createMovement(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function editMovement(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function dashboard(User $user, User $model)
    {
        return $model->isAssociated($user) || $user->id === $model->id;
    }

    public function editProfile(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function editPassword(User $user, User $model)
    {
        return $user->id === $model->id;
    }


    public function viewProfile(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function viewAssociates(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function viewAssociatesOf(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function editAssociates(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function storeAssociate(User $user, User $model)
    {
        return $user->id === $model->id;
    }

    public function deleteAssociate(User $user, User $model)
    {
        return $model->isAssociated($user);
    }
}
