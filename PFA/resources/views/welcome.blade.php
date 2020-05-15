<!doctype html>
<html lang="{{ config('app.locale') }}">
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">

        <title>Personal Finances Assistant</title>

        <!-- Fonts -->
        <link href="https://fonts.googleapis.com/css?family=Raleway:100,600" rel="stylesheet" type="text/css">

        <!-- Styles -->
        <style>
            html, body {
                background-color: #fff;
                color: #636b6f;
                font-family: 'Raleway', sans-serif;
                font-weight: 100;
                height: 100vh;
                margin: 0;
            }

            .full-height {
                height: 100vh;
            }

            .flex-center {
                align-items: center;
                display: flex;
                justify-content: center;
            }

            .position-ref {
                position: relative;
            }

            .top-right {
                position: absolute;
                right: 10px;
                top: 18px;
            }

            .content {
                text-align: center;
            }

            .title {
                font-size: 50px;
            }

            .dashboard {
                font-size: 20px;
                text-transform: uppercase;
                letter-spacing: .1rem;
            }

            .links > a {
                color: #636b6f;
                padding: 0 5px;
                font-size: 12px;
                font-weight: 100;
                text-decoration: none;
            }

            .m-b-md {
                margin-bottom: 30px;
            }
        </style>
    </head>
    <body>
        <div class="flex-center position-ref full-height">
            @if (Route::has('login'))
                <div class="top-right links">
                    @if (! Auth::check())
                        <a href="{{ url('/login') }}">Login</a>
                        <a href="{{ url('/register') }}">Register</a>
                    @endif
                </div>
            @endif

            <div class="content">
                <div class="title m-b-md">
                    Finances
                </div>
                @if (Auth::check())
                <div class="links">                  
                        <a href="{{route('get-associates', Auth::id())}}">{{ __('Associates') }}</a>
                        <a href="{{route('get-associatesOf', Auth::id())}}">{{ __('Associates Of') }}</a>
                        <a href="{{route('accounts.index', Auth::id())}}">{{ __('Accounts') }}</a>
                        <a href="{{route('accounts.opened', Auth::id())}}">{{ __('Opened Accounts') }}</a>
                        <a href="{{route('accounts.closed', Auth::id())}}">{{ __('Closed Accounts') }}</a>
                        <a href="{{route('edit-profile', Auth::id())}}">{{ __('Edit Profile') }}</a>
                        <a href="{{route('edit-password', Auth::id())}}">{{ __('Edit Password') }}</a>
                </div>
                <br>
                <div class="dashboard"><a href="{{ route('dashboard', Auth::id()) }}">Dashboard</a></div>
                @endif
            </div>
        </div>
    </body>
</html>
