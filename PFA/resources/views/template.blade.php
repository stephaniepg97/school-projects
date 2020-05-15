<!DOCTYPE html>
<html lang="en">
<head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>@yield('title')</title>

        <!-- Latest compiled and minified CSS & JS -->
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

        <!-- Scripts -->
        <script src="{{ asset('js/app.js') }}" defer></script>

        <!-- Fonts -->
        <link rel="dns-prefetch" href="https://fonts.gstatic.com">
        <link href="https://fonts.googleapis.com/css?family=Raleway:300,400,600" rel="stylesheet" type="text/css">

        <!-- Styles -->
        <link href="{{ asset('css/app.css') }}" rel="stylesheet">

        <!-- Chart -->
        <script src="{{ asset('js/chart/Chart.js') }}"></script>
        <script src="{{ asset('js/chart/Chart.min.js') }}"></script>
        <script src="{{ asset('js/chart/Chart.bundle.min.js') }}"></script>
        <script src="{{ asset('js/chart/Chart.bundle.js') }}"></script>
</head>
<body>
    <div class="container">
        @include('partials.menu')
            
        <div class="jumbotron">
            <h1>@yield('title')</h1>
        </div>
            
        @if(session('success'))
            @include('partials.success')
        @elseif(session('unsuccess'))
            @include('partials.unsuccess')
        @endif

        <main class="py-4">
            @yield('content')
        </main>
    </div>

    @yield('dashboard')
    
    <script src="https://code.jquery.com/jquery.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS" crossorigin="anonymous"></script>
</body>
</html>