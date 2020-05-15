@extends('template')

@section('title', 'Dashboard')

@section('dashboard')

<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <h4>{{__('Finances')}}</h4>
                </div>      
                <div class="card-body">
                    <table class="table table-striped">
                        <thead>
                        <tr>
                            <th>Users</th>
                            <th>Accounts</th> 
                            <th>Movements</th>
                            <th>Documents</th>
                        </tr>
                        </thead>

                        <tbody>
                        <tr>
                            <td>{{ $total_users }}</td>
                            <td>{{ $total_accounts }}</td>
                            <td>{{ $total_movements }}</td>
                            <td>{{ $total_documents }}</td>
                        </tr>
                        </tbody>
                    </table>
                </div>      

                <div class="card-header">
                    <h4>{{__('Search for a specific time frame')}}</h4>
                </div>
                
                <div class="card-body">
                    <p>{{ __('Existing data starting at: ') }} {{ $user->getFirstDate() }} {{ __(' and ending at:') }} {{ $user->getLastDate() }}</p>
                    <form action="{{route('dashboard', $user->id)}}" method="get" class="form-group">
                        @csrf
                        <div class="form-group">
                        <label for="input_start_date">{{ __('Start Date ') }}</label>
                        <input
                            type="date" class="form-control"
                            name="start_date"  id="input_start_date" 
                            value="{{ old('start_date', $start_date) }}" 
                            />
                        </div>

                        <div class="form-group">
                            <label for="input_end_date">{{ __('End Date ') }}</label>
                            <input
                                type="date" class="form-control"
                                name="end_date"  id="input_end_date" 
                                value="{{ old('end_date', $end_date) }}" 
                                />
                        </div>

                        <div class="form-group">
                            <button type="submit" class="btn btn-xs btn-success" name="ok">Get</button>
                        </div>
                    </form>
                </div>

                <div class="card-header">
                    <h4>{{__('Search monthly')}}</h4>
                </div>
                
                <div class="card-body">
                    <form action="{{route('dashboard', $user->id)}}" method="get" class="form-group">
                        @csrf
                        <div class="form-group">
                        <label for="input_month">{{ __('Month') }}</label>
                        <input
                            type="number" class="form-control"
                            name="month"  id="input_month" 
                            value="{{ old('month', $month) }}" 
                            required/>
                        </div>

                        <div class="form-group">
                        <label for="input_year">{{ __('year') }}</label>
                        <input
                            type="number" class="form-control"
                            name="year"  id="input_year" 
                            value="{{ old('year', $year) }}" 
                            required/>
                        </div>

                        <div class="form-group">
                            <button type="submit" class="btn btn-xs btn-success" name="ok">Get</button>
                        </div>
                    </form>
                </div>

                <div class="card-header">
                    <h1 class="row justify-content-center">{{$start_date}} to {{$end_date}}</h1>
                    <h4>{{__('Textual Information')}}</h4>
                </div>

                <div class="card-body">

                @if(count($accounts))

                <table class="table table-striped">
                
                <thead>
                <tr>
                    <th>Total Balance ($)</th> 
                    <th>{{ $user->getTotalBalance() }}</th>
                </tr>
                </thead>

                </table>


                <table class="table table-striped">
                
                <thead>

                <tr>
                    <th>Id</th> 
                    <th>Current Balance ($)</th>
                    <th>Weight over the total balance (%)</th>
                    <th>Total per category ($)</th>
                    @if(count($categories)) 
                    @foreach($categories as $category)
                    <th></th>
                    @endforeach
                    @endif
                </tr>
                <tr>
                    <th></th>
                    <th></th>
                    <th>
                    @if(count($categories)) 
                    @foreach($categories as $category)
                    <th>{{ $category->nameToStr() }}</th>
                    @endforeach
                    @else
                    <th>No moviments on every account</th>
                    @endif
                    </th>
                </tr>
                </thead>

                <tbody>
                @foreach($accounts as $account)
                <tr>
                    <td>{{ $account->id }}</td>
                    <td>{{ $account->current_balance }}</td>
                    <td>{{ $account->weight() }}</td>
                    @if(count($categories))
                    @foreach($categories as $category)
                    <td>{{ $category->total($start_date, $end_date, $account) }}</td>
                    @endforeach
                    @endif
                </tr>
                @endforeach
                </tbody>

                @if(count($categories))
                <thead>
                    <tr>
                    <th>TOTAL</th>
                    <th>{{ $user->getTotalBalance() }}</th>
                    <th>100</th>
                    @foreach($categories as $category)
                    <th>{{ $category->finalTotal($start_date, $end_date, $accounts) }}</th>
                    @endforeach
                    </tr>
                </thead>
                @endif
                </table>

                @else
                <br>
                <h2>No accounts found</h2>
                @endif

                </div>

                <div class="card-header">
                    <h4>{{__('Graphical Information')}}</h4>
                </div>

                <div class="card-body">

                <canvas id="myChart" width="400" height="400"></canvas>

                <script>
                var ctx = document.getElementById("myChart").getContext('2d');
                var categories = <?php echo $categories_names?>;
                var values = <?php echo $values?>;
                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: categories,
                        datasets: values
                    },
                    options: {
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero:true
                                }
                            }]
                        }
                    }
                });
                </script>

                <div class="row justify-content-center">{{ $categories->appends(['start_date' => $start_date, 'end_date' => $end_date])->links() }}</div>
                                
                </div>
            </div>
        </div>
    </div>
</div>
@endsection('dashboard')