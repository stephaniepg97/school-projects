@extends('template')

@section('title', 'List users')

@section('content')

    @if(Auth::user()->admin)
    <form action="{{route('admin.users')}}" method="get">
    @else
    <form action="{{route('profiles')}}" method="get">
    @endif
        <div>
            <label>Name</label>
            <input type="text" name="name">
        </div>
        @if(Auth::user()->admin)
        <div>
            <label>Is admin?</label>
            <select name="isAdmin">
                <option disabled selected> -- select an option -- </option>
                <option value="0">No</option>
                <option value="1">Yes</option>
            </select>
        </div>
        <div>
            <label>Is blocked?</label>
            <select name="isBlocked">
                <option disabled selected> -- select an option -- </option>
                <option value="0">No</option>
                <option value="1">Yes</option>
            </select>
        </div>
        @endif
        <div>
            <button type="submit" class="btn btn-primary">Search</button>
        </div>
    </form>

    <tr>

    @if(count($users))
    <table class="table table-striped">
        <thead>
            <tr>
                <th>Name</th>
                <th>Profile</th>
                <th>Associate/Associate Of</th>
                @if(Auth::user()->admin)
                    <th>Email</th>
                    <th>Created</th> 
                    <th>Updated</th>
                    <th>Is admin</th> 
                    <th>Is blocked</th>  
                @endif
            </tr>
        </thead>
                            
        <tbody>
            @foreach($users as $user)
                <tr>
                    <td>{{$user->name}}</td>
                    <td><form action="{{route('profile')}}" method="post">
                    @csrf
                    <input type="hidden" name="user_id" value="<?= $user->id ?>"/>
                    <button type="submit" class="btn btn-primary">Profile</button></form></td>
                    <td>@include('users.partials.tag')</td>

                    @if(Auth::user()->admin)
                        <td>{{$user->email}}</td>
                        <td>{{$user->created_at}}</td>
                        <td>{{$user->updated_at}}</td>
                        <td>{{$user->adminToStr()}}</td>
                        <td>{{$user->blockedToStr()}}</td>
                    @endif

                    @if($user->id !== Auth::user()->id)
                        @if(Auth::user()->admin)    
                            @if($user->admin)
                                <td><form action="{{route('admin.demote', $user->id)}}" method="post">
                                @method('patch')
                                @csrf
                                <button type="submit" class="btn btn-danger">Remote</button></form></td>
                            @else
                                <td><form action="{{route('admin.promote', $user->id)}}" method="post">
                                @method('patch')
                                @csrf
                                <button type="submit" class="btn btn-success">Promote</button></form></td>
                            @endif

                            @if($user->blocked)
                                <td><form action="{{route('admin.unblock', $user->id)}}" method="post">
                                @method('patch')
                                @csrf
                                <button type="submit" class="btn btn-success">Unblock</button></form></td>
                            @else
                                <td><form action="{{route('admin.block', $user->id)}}" method="post">
                                @method('patch')
                                @csrf
                                <button type="submit" class="btn btn-danger">Block</button></form></td>
                            @endif
                        @endif
                    @else
                        @can('viewAccounts', $user)
                        <td><a class="btn btn-primary" href="{{route('accounts.index', $user->id)}}">Accounts</a></td>
                        @endcan
                    @endif
                </tr>
            @endforeach
        </tbody>
    </table>
    @else
    <br>
    <h2>No users found</h2>
    @endif

@endsection('content')