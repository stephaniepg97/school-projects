@extends('template')


@section('title', $title)

@section('content')

@if(count($associates))    
    <table class="table table-striped">
        <thead>
            <tr>
                <th>Id Of Associated User</th>
                <th>Name</th>
                <th>Email</th>
            </tr>
        </thead>

        @foreach($associates as $associate)
        <tbody>
            <td>{{ $associate->id }}</td>
            <td>{{ $associate->name }}</td>
            <td>{{ $associate->email }}</td>
            @if(Route::currentRouteName()=='edit-associates')
            @can('deleteAssociate', $associate)
                <td>
                    <form action="{{route('destroy-associate', $associate->id)}}" method="POST" role="form" class="inline">
                    @method('delete')
                    @csrf
                        <input type="hidden" name="associated_user_id" value="{{$associate->id}}"/>  
                        <button type="submit" class="btn btn-xs btn-danger">Delete</button>
                    </form>
                </td>
            @endcan
            @endif
            @can('viewAccounts', Auth::user())
                <td><a class="btn btn-primary" href="{{route('accounts.index', $associate->id)}}">Accounts</a></td>
            @endcan
        </tbody>
        @endforeach
    </table>
@else
    <h2>No Associates found</h2>
@endif
    
    <tr><br>

    @if(Route::currentRouteName()=='get-associates')
    <a class="btn btn-primary" href="{{route('edit-associates', $user->id)}}">Edit Associates</a>
    @elseif(Route::currentRouteName()=='edit-associates')
    @can('storeAssociate', $user)
    <form action="{{route('add-associate')}}" method="post" class="form-group">
        @csrf
        <input type="hidden" name="main_user_id" value="{{$user->id}}" />  
        <div class="form-group row">
        <label for="associated_user" class="col-md-4 col-form-label text-md-right">{{ __('Id of Associated User') }}</label>

                <div class="col-md-6">
                    <input id="associated_user" type="text" class="form-control{{ $errors->has('associated_user_id') ? ' is-invalid' : '' }}" name="associated_user_id" 
                        value="{{ old('associated_user_id') }}" required autofocus>
                </div>
        </div>

        <div class="form-group">
            <button type="submit" class="btn btn-success" name="ok">Submit</button>
            <a class="btn btn-default" href="{{route('profiles')}}">Cancel</a>
        </div>
    </form>
    @endcan
    @endif
        
@endsection