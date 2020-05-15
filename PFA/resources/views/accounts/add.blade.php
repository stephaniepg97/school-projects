@extends('template')

@section('title', 'Add Account')

@section('content')

    @if ($errors->any() > 0)
        @include('partials.errors')
    @endif

    <form action="{{route('accounts.store')}}" method="post" class="form-group">
        @csrf
        @include('accounts.partials.add-edit')

        <div class="form-group row">
            <label for="date" class="col-md-4 col-form-label text-md-right">{{ __('Date') }}</label>
            <div class="col-md-6">
            <input
                id="date" type="date" class="form-control{{ $errors->has('date') ? ' is-invalid' : '' }}" name="date" value="{{ old('date') }}" >
            </div>
        </div>
        
        <input type="hidden" name="owner_id" value="{{ $user->id }}">
        <div class="form-group">
            <button type="submit" class="btn btn-success" name="ok">Concluir</button>
            <a class="btn btn-default" href="{{route('accounts.index', $user->id)}}">Cancel</a>
        </div>
    </form>
        
@endsection