<nav class="navbar navbar-expand-md navbar-light">
    <div class="container">
        <div class="collapse navbar-collapse" id="navbarSupportedContent">
                 
            <!-- Right Side Of Navbar -->
            <ul class="navbar-nav ml-auto">
                <!-- Authentication Links -->
                    @guest
                        <li><a class="nav-link" href="{{ route('login') }}">{{ __('Login') }}</a></li>
                        <li><a class="nav-link" href="{{ route('register') }}">{{ __('Register') }}</a></li>
                    @else
                        <li><a class="nav-link" href="{{route('get-associates', Auth::user()->id)}}">{{ __('Associates') }}</a></li>
                        <li><a class="nav-link" href="{{route('get-associatesOf', Auth::user()->id)}}">{{ __('Associates Of') }}</a></li>
                        <li><a class="nav-link" href="{{route('accounts.index', Auth::user()->id)}}">{{ __('Accounts') }}</a></li>
                        <li><a class="nav-link" href="{{route('accounts.opened', Auth::user()->id)}}">{{ __('Opened Accounts') }}</a></li>
                        <li><a class="nav-link" href="{{route('accounts.closed', Auth::user()->id)}}">{{ __('Closed Accounts') }}</a></li>
                        <li><a class="nav-link" href="{{route('edit-profile', Auth::user()->id)}}">{{ __('Edit Profile') }}</a></li>
                        <li><a class="nav-link" href="{{route('edit-password', Auth::user()->id)}}">{{ __('Edit Password') }}</a></li>
                        <li><a class="nav-link" href="{{ route('dashboard', Auth::user()->id) }}">{{ __('Dashboard') }}</a></li>

                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" v-pre><?= Auth::user()->name ?></a>

                                <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                    <a class="dropdown-item" href="{{ route('logout') }}"
                                       onclick="event.preventDefault();
                                                     document.getElementById('logout-form').submit();">
                                        {{ __('Logout') }}
                                    </a>

                                    <form id="logout-form" action="{{ route('logout') }}" method="POST" style="display: none;">
                                        @csrf
                                    </form>
                                    <a class="dropdown-item" href="{{ route('profile') }}">{{ __('Profile') }}</a>
                                </div>
                        </li>
                    @endguest
            </ul>
        </div>
    </div>
</nav>
