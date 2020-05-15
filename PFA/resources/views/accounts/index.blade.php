@extends('template')

@section('title', 'Accounts')

@section('content')


    <form action="{{route('accounts.index', $user->id)}}" method="get">
    @csrf
        <div class="form-group">
        <label for="input_account_type_id">{{ __('Account Type') }}</label>

        <select name="account_type_id" id="input_account_type_id">
            <option disabled selected> -- select an option -- </option>
                @foreach($types as $type)
            <option value="{{ $type->id }}">{{ $type->name }}</option>
                @endforeach
        </select>
        </div>
        
        <div>
            <button type="submit" class="btn btn-primary">Search</button>
        </div>
    </form>

    <br>

    @can('addAccount', $user)
    <div>
        <a class="btn btn-primary" href="{{route('accounts.create', $user->id)}}">Add Account</a>
    </div>
    @endcan

    @if (count($accounts))
    <table class="table table-striped">
    <thead>
        <tr>
            <th>Code</th>
            <th>Type</th>
            <th>Current Balance</th>
        </tr>
    </thead>
    <tbody>
        @foreach ($accounts as $account)
        <tr>
            <td>{{ $account->code }}</td>
            <td>{{ $account->typeToStr() }}</td>
            <td>{{ $account->current_balance }}</td>
            <td>
                @can('editAccount', $user)
                <form action="{{route('accounts.destroy', $account->id)}}" method="post">
                    @method('delete')
                    @csrf
                    <button type="submit" class="btn btn-xs btn-danger">Delete</button>
                </form>
                @endcan
            </td>
            @if($account->trashed())
            <td>
                @can('reopenAccount', $user)
                <form action="{{route('accounts.reopen', $account->id)}}" method="post">
                    @method('patch')
                    @csrf
                    <button type="submit" class="btn btn-xs btn-success" onclick="{{$account->restore()}}">Reopen</button>
                </form>
                @endcan
            </td>
            @else
            <td>
                @can('closeAccount', $user)
                <form action="{{route('accounts.close', $account->id)}}" method="post">
                    @method('patch')
                    @csrf
                    <button type="submit" class="btn btn-xs btn-danger">Close</button>
                </form>
                @endcan
            </td>
            <td>
                @can('editAccount', $user)
                <a class="btn btn-xs btn-primary" href="{{route('accounts.edit', $account->id)}}">Edit</a>
                @endcan
            </td>
            <td>
                @can('viewMovements', $user)
                <a class="btn btn-xs btn-primary" href="{{route('movements.index', $account->id)}}">Movements</a>
                @endcan
            </td>
            @endif
        </tr>
         @endforeach
    </tbody>
    </table>
@else
    <h2>No accounts found</h2>
@endif
@endsection('content')

