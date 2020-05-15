@extends('template')

@section('title', 'Movements')

@section('content')

@can('addMovement', $account->user())
<div>
    <a class="btn btn-primary" href="{{route('movements.create', $account->id)}}">Add movement</a>
</div>
@endcan

@if (count($movements))
    <table class="table table-striped">
    <thead>
        <tr>
            <th>id</th>
            <th>Category</th>
            <th>Value</th>
            <th>Date</th>
            <th>Type</th>
            <th>End Balance</th>
        </tr>
    </thead>
    <tbody>
    @foreach ($movements as $movement)
        <tr>
            <td>{{ $movement->id }}</td>
            <td>{{ $movement->categoryToStr() }}</td>
            <td>{{ $movement->value }}</td>
            <td>{{ $movement->date }}</td>
            <td>{{ $movement->type }}</td>
            <td>{{ $movement->end_balance }}</td>

            @can('editMovement', Auth::user())
            <td>
                <a class="btn btn-xs btn-primary" href="{{route('movements.edit', $movement->id)}}">Edit</a>
            </td>
            <td>
            @endcan
                @can('editMovement', Auth::user())
                <form action="{{route('movements.destroy', $movement->id)}}" method="post" role="form" class="inline">
                    @method('delete')
                    @csrf
                    <button type="submit" class="btn btn-xs btn-danger">Delete</button>
                </form>
                @endcan
            </td>
            @if($movement->hasDocument())
            <td><a class="btn btn-xs btn-primary" href="{{route('documents.show', $movement->document_id)}}">Details</a></td>

            @can('editMovement', Auth::user())
            <td>
                
                <form action="{{route('documents.destroy', $movement->document_id)}}" method="post" role="form" class="inline">
                    @method('delete')
                    @csrf
                    <button type="submit" class="btn btn-xs btn-danger">Delete Document</button>
                </form>
                
            </td>
            @endcan

            @can('viewDocument', Auth::user())
            <td><a class="btn btn-xs btn-primary" href="{{route('documents.download', $movement->document_id)}}">Download Document</a></td>
            @endcan
            
            @can('viewDocument', Auth::user())
            <td><a class="btn btn-xs btn-primary" href="{{route('documents.view', $movement->document_id)}}">View Document</a></td>
            @endcan

            @else
            <td><a class="btn btn-xs btn-success" href="{{route('documents.create', $movement->id)}}">Add Document</a></td>
            @endif
        </tr>
        @endforeach
    </table>
@else
    <h2>No movements found</h2>
@endif
@endsection('content')

