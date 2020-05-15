<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <meta name="csrf-token" content="{{ csrf_token() }}">
        <title>Restaurant Management</title>

        <!-- CSS stylesheets -->
        <link rel="stylesheet" href="{{ URL::asset('css/app.css') }}">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.2.7/semantic.min.css" media="screen" title="no title" charset="utf-8">
    </head>
    
    <body>
        <div class="container" id="app">
            
        </div>

        <!-- javascript sources -->
        <script src="js/main.js"></script>
        <script src="{{ URL::asset('js/chart/Chart.js') }}"></script>  
        <script src="{{ URL::asset('js/chart/Chart.min.js') }}"></script>  
        <script src="{{ URL::asset('js/chart/Chart.bundle.js') }}"></script>   
        <script src="{{ URL::asset('js/chart/Chart.bundle.min.js') }}"></script>   
        <script src="https://cdnjs.cloudflare.com/ajax/libs/semantic-ui/2.2.7/semantic.min.js" charset="utf-8"></script>
    </body>
</html>


