@extends('template')

@section('title', 'Add Movement')

@section('content')

@if($errors->any())
    @include('partials.errors')
@endif

<form action="{{route('movements.store', $account->id)}}" method="post" class="form-group" enctype=multipart/form-data>
		@csrf
        @include('movements.partials.add-edit')
        @include('documents.partials.add-edit')
    <div class="form-group">
        <button type="submit" class="btn btn-success" name="ok">Submit</button>
        <a class="btn btn-default" href="{{route('movements.index', $account->id)}}">Cancel</a>
    </div>
</form>

@endsection('content')