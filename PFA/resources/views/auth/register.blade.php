@extends('template')

@section('title', 'Register')

@section('content')

    @if ($errors->any() > 0)
        @include('partials.errors')
    @endif

    <form method="POST" action="{{ route('register') }}" class="form-group" enctype=multipart/form-data>
        @csrf                        
        @include('users.partials.profile')
        @include('users.partials.password')
        <div class="form-group row mb-0">
            <div class="col-md-6 offset-md-4">
                <button type="submit" class="btn btn-primary">{{ __('Register') }}</button>
            </div>
        </div>
    </form>
                
@endsection
