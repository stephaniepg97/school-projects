<?php

namespace App\Http\Controllers;
use App\Document;
use App\Movement;
use App\User;
use App\Account;
use App\MovementCategory;
use Illuminate\Http\Request;
use Illuminate\Pagination\Paginator;
use Route;
use DateTime;

class DashboardController extends Controller
{
    /**
     * Create a new controller instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('auth');
    }

    /**
     * Show the application dashboard.
     *
     * @return \Illuminate\Http\Response
     */
    public function index(Request $request, User $user)
    {
         //AUTH
        $this->authorize('dashboard', $user);

        //---------------- TOTALS ---------------------------------------------------------
        $total_users = User::all()->count();
        $total_accounts = Account::all()->count();
        $total_movements = Movement::all()->count();
        $total_documents = Document::all()->count();

        //US.26
        
        //---------------- USER TOTAL BALANCE OF ALL OF ACCOUNTS -------------------------- 
        $total_balance = $user->getTotalBalance();

        //summary information about each account including the relative weight (percentage) of each account over the total balance

        $accounts = $user->getOpenedAccounts();

        //---------------- START AND END DATES --------------------------

        $month = $request->input('month') ?? '';
        $year = $request->input('year') ?? '';
        $start_date = $request->input('start_date') ?? $user->getFirstDate();
        $end_date = $request->input('end_date') ?? $user->getLastDate();

        if ($month !== '')
        {
            $start_date = date("Y-m-d", strtotime(date($year."-".$month."-1")));
            $end_date = date("Y-m-t", strtotime($start_date));
        }

        //---------------- USER CATEGORIES PER PAGE --------------------------

        $result = $user->getCategories();

        $next_result = $result->chunk(9);

        $page = $request->input('page');

        if ($page == null || $page == 1)
        {
            $categories = new Paginator($result, 9);
        }
        else
        {
            $categories = new Paginator($next_result->get($page-1), $next_result->get($page-1)->count());    
        }
        
        $categories->setPath($request->url());

        //----------------- CHART ---------------------------------------------

        //1 - ARRAY OF CATEGORY NAMES
        $categories_names = '["'.$categories->implode('name', '","').'"]';

        
        //2 - ARRAY OF EACH CATEGORY VALUES
        $categories_values = collect([]);

        foreach ($accounts as $account) {
            $values = '[';
            $total = '';
            foreach ($categories as $category) { 
                $total = $category->total($start_date, $end_date, $account) ?? '';
                $values = $values.$total.',';
            }
            $values = $values.$total.']';
            $categories_values = $categories_values->merge([$values]);
        }

        $values = "";
        for ($i = 0; $i < $accounts->count(9); $i++) {
            $values = $values."{label: 'Account # ".$accounts->get($i)->id."', data: ".$categories_values[$i].", fill: false, borderColor: '".$this->getColor($i)."', borderWidth: 1},";
        }

        $values = "[".$values."]";

        //TEXTUAL DATA & GRAPHICAL DATA
        //1. the total revenues and expenses by categories, on a given time frame //US.27   
        //2. the evolution through time (monthly) of the revenues and expenses by categories, on a given time frame; //US 28. 

        return view('dashboard.index', compact('user', 'accounts', 'total_balance', 'categories', 'start_date', 'end_date', 'month', 'year', 'categories_names', 'values', 'total_users', 'total_accounts', 'total_movements', 'total_documents'));
    }

    //private functions 

    private function getColor($num) {
        $hash = md5('color' . $num); // modify 'color' to get a different palette
        $array = array(
            hexdec(substr($hash, 0, 2)), // r
            hexdec(substr($hash, 2, 2)), // g
            hexdec(substr($hash, 4, 2))); //b
        return 'rgba('.implode(',', $array).',1)';
    }
}
