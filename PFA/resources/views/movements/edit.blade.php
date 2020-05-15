@extends('template')

@section('title', 'Edit Movement')

@section('content')

@if($errors->any())
    @include('partials.errors')
@endif

<form action="{{route('movements.update', $movement->id)}}" method="post" class="form-group" enctype=multipart/form-data>
		@method('put')
        @csrf
        @include('movements.partials.add-edit')
    <div class="form-group">
        <button type="submit" class="btn btn-success" name="ok">Submit</button>
        <a class="btn btn-default" href="{{route('movements.index', $movement->account_id)}}">Cancel</a>
    </div>
</form>

@endsection('content')