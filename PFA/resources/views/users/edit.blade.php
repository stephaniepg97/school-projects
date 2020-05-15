@extends('template')

@section('title', $title)

@section('content')

    @if ($errors->any() > 0)
        @include('partials.errors')
    @endif

    @if(Route::currentRouteName()=='edit-password')
    <form action="{{route('update-password')}}" method="post" class="form-group">
        @method('patch')
        @csrf
        <input type="hidden" name="userId" value="{{$user->id}}"/>  
        @include('users.partials.password')
    @elseif(Route::currentRouteName()=='edit-profile')
    <form action="{{route('update-profile')}}" method="post" class="form-group" enctype=multipart/form-data>
        @method('put')
        @csrf 
        <input type="hidden" name="userId" value="{{$user->id}}"/> 
        @include('users.partials.profile')
    @endif
        <div class="form-group">
            <button type="submit" class="btn btn-success" name="ok">Concluir</button>
            <a class="btn btn-default" href="{{route('profiles')}}">Cancel</a>
        </div>
    </form>
        

@endsection
