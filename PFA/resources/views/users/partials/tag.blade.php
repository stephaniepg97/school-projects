@if($associates !== null)
    @foreach($associates as $associate)
        @if($user->id === $associate->id)
            {{'Associate'}}
        @endif
     @endforeach
@endif

@if($associatesOf !== null)
    @foreach($associatesOf as $associateOf)
        @if($user->id === $associateOf->id)
            {{'Associate Of'}}
        @endif
    @endforeach
@endif