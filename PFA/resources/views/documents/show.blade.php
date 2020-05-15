@extends('template')

@section('title', 'Details')

@section('content')

<table class="table table-striped">
    <thead>
        <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Created At</th>
        </tr>
    </thead>
    <tbody>
    	<tr>
            <td>{{ $document->original_name }}</td>
            <td>{{ $document->description }}</td>
            <td>{{ $document->created_at }}</td>
            <td>
                <form action="{{route('documents.destroy', $document->id)}}" method="post" role="form" class="inline">
                    @method('delete')
                    @csrf
                    <button type="submit" class="btn btn-xs btn-danger">Delete Document</button>
                </form>
            </td>
            <td><a class="btn btn-xs btn-primary" href="{{route('documents.download', $document->id)}}">Download Document</a></td>
            <td><a class="btn btn-xs btn-primary" href="{{route('documents.view', $document->id)}}">View Document</a>
        </tr>
    </tbody>
</table>

<img src="{{$url}}" alt="{{ $document->original_name }}" width="300">

@endsection('content')