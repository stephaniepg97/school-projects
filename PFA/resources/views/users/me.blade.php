@extends('template')

@section('title', 'Profile')

@section('content')

    <table class="table table-striped">
    <thead>
    <tr>
        <th>Name</th>
        <th>Email</th> 
        <th>Created</th> 
        <th>Updated</th>
        <th>Is admin</th> 
        <th>Is blocked</th>  
    </tr>
    </thead>                        
    <tbody>
        <tr>
            <td>{{$user->name}}</td>
            <td>{{$user->email}}</td>
            <td>{{$user->created_at}}</td>
            <td>{{$user->updated_at}}</td>
            <td>{{$user->adminToStr()}}</td>
            <td>{{$user->blockedToStr()}}</td>
        </tr>
    </tbody>

@endsection