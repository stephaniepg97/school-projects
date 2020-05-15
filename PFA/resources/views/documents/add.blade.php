@extends('template')

@section('title', 'Add Document')

@section('content')

@if($errors->any())
    @include('partials.errors')
@endif

<form action="{{route('documents.store', $movement->id)}}" method="post" class="form-group" enctype=multipart/form-data>
		@csrf
        @include('documents.partials.add-edit')
    <div class="form-group">
        <button type="submit" class="btn btn-success" name="ok">Submit</button>
        <a class="btn btn-default" href="{{route('movements.index', $movement->account_id)}}">Cancel</a>
    </div>
</form>
@endsection('content')