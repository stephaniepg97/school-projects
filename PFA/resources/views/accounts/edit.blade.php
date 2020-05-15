@extends('template')

@section('title', 'Edit Account')

@section('content')

    @if ($errors->any() > 0)
        @include('partials.errors')
    @endif

    <form action="{{route('accounts.update', $account->id)}}" method="post" class="form-group">
        @method('put')
        @csrf
        @include('accounts.partials.add-edit')
        <div class="form-group">
            <button type="submit" class="btn btn-success" name="ok">Concluir</button>
            <a class="btn btn-default" href="{{route('accounts.index', $account->owner_id)}}">Cancel</a>
        </div>
    </form>
        

@endsection