@extends('layouts.app')

@section('content')
<div class="container">
    <div class="row">
        <div class="col-md-8 col-md-offset-2">
            <div class="panel panel-default">
                <div class="panel-heading">Dashboard</div>
                   <h4>- {{ Auth::user()->name }} </h4>
                   <h3>You are logged in!</h3>
                </div>
            </div>
        </div>
    </div>
</div>
@endsection
