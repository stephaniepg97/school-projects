<?php
namespace App\Http\Controllers;
use Illuminate\Support\Facades\Auth;
use App\User;
use Illuminate\Http\UploadedFile;
use Illuminate\Support\Facades\Storage;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Hash;
use App\Http\Requests\UpdateProfileRequest;
use App\Http\Requests\UpdatePasswordRequest;
use Illuminate\Support\Collection;
use App\Http\Requests\StoreAssociateRequest;
use App\Associate;

class UserController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index(Request $request)
    {
        $this->authorize('index', User::class);

        $users = User::findPattern(User::All(), $request['name']);

        $associates = Auth::user()->getAssociates();

        $associatesOf = Auth::user()->getAssociatesOf();

        return view('users.index', compact('users', 'associates', 'associatesOf'));

    }

    public function profile()
    {
       $user = Auth::user();
        
        return view('users.me', compact('user'));
    }


    public function editProfile(User $user)
    {
        $title = 'Edit Profile';

        $this->authorize('editProfile', $user);

        return view('users.edit', compact('user', 'title'));
    }

    public function updateProfile(UpdateProfileRequest $request)
    {

        $user = User::findOrFail($request['userId']);

        $this->authorize('editProfile', $user);

        $data = $request->validated();

        $user->fill($data);

        if ($request->hasFile('photo') &&  $request->file('photo')->isValid()) 
        {
            //store uploaded photo
            $user->profile_photo = $request->file('photo')->storeAs('profiles', $user->id.'.'.$request->file('photo')->extension());
            //turn it public

            //give it a url
        }
        
        $user->update();

        return redirect()
            ->route('profiles')
            ->with('success', 'User updated successfully.');
    }

    public function editPassword(User $user)
    {
        $this->authorize('editPassword', $user);

        $title = 'Edit Password';
        return view('users.edit', compact('user', 'title'));
    }

    public function updatePassword(UpdatePasswordRequest $request)
    {
        $this->authorize('editPassword', $user);

        $user = User::findOrFail($request['user_id']);

        $user->password = Hash::make($data['password']);
        
        $user->update();

        return redirect()
            ->route('profiles')
            ->with('success', 'User updated successfully.');
    }

    public function getAssociates(User $user)
    {
        $this->authorize('viewAssociates', $user);

        $title = 'Associates';

        $associates =  $user->getAssociates();

        return view('associates.index', compact('associates', 'user', 'title'));   
    }

    public function editAssociates(User $user)
    {
        $this->authorize('editAssociates', $user);

        $associates = $user->getAssociates();

        $title = 'Edit Associates';

        return view('associates.index', compact('associates', 'user', 'title'));   
    }

    public function getAssociatesOf(User $user)
    {
        $this->authorize('viewAssociatesOf', $user);

       // $associates = User::getAssociatesOf($user->id);

        $title = 'Associates Of';

        $associates = $user->getAssociatesOf();

        return view('associates.index', compact('associates', 'user', 'title'));  
    }

    public function storeAssociate(StoreAssociateRequest $request)
    {
        $data = $request->validated();

        $user = User::findOrFail($data['main_user_id']);

        $this->authorize('storeAssociate', $user);

        $associated_user = User::findOrFail($data['associated_user_id']);

        if ($associated_user->isAssociated($user))
        {
            return back()->with('unsuccess', 'Is already an associate.');
        }

        if ($associated_user == $user)
        {
            return back()->with('unsuccess', 'Cannot associate with the same user.');
        }

        $user->associates()->attach($associated_user->id);

        return redirect()
            ->route('get-associates', $user->id)
            ->with('success', 'Association added successfully.');
    }

    public function destroyAssociate(Request $request, User $user)
    {
        $data = $request->validated();

        $user = User::findOrFail($data['main_user_id']);

        $associated_user = User::findOrFail($data['associated_user_id']);

        $this->authorize('deleteAssociate',  $associated_user);

        if (! $associated_user->isAssociated($user))
        {
            return back()->with('unsuccess', 'It is not an associate.');
        }

        $user->associates()->detach($associated_user->id);

        return redirect()
            ->route('get-associates', $user->id)
            ->with('success', 'Association deleted successfully.');
    }

    // ------------------------------
    // Private
    // ------------------------------


}
