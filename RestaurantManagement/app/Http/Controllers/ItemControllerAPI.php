<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Contracts\Support\Jsonable;
use Illuminate\Support\Facades\Storage;
use App\Http\Resources\Item as ItemResource;
use Illuminate\Validation\Rule;
use Illuminate\Http\File;
use Illuminate\Pagination\LengthAwarePaginator;
use App\Item;
use Validator;

class ItemControllerAPI extends Controller
{
    public function index(Request $request)
    {
        $query = [];
        
        if ($request->has('type')) {
            array_push($query, ['type', $request->type]);
        }
        
        if ($request->has('name')) {
            array_push($query, ['name', 'like', '%'.$request->name.'%']);
        } 
        
        $items = Item::where($query);
        
        if ($request->has('page')) {
            $items = $items->paginate(5);
        } else {
            $items = $items->get();
        }

        if ($items->isEmpty()) {
            $message = "No items found.";
            return response()->json(compact('message'), 404);
        } 
        
        return ItemResource::collection($items);
    }

    public function show($id)
    {
        $item = Item::find($id);

        if (!$item) 
        {
            $message = 'Item no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        return response()->json(new ItemResource($item), 200);
    }

    public function store(Request $request)
    {
        $validator = Validator::make($request->all(), [
                'name' => 'required|string|unique:items,name',
                'description' => 'required|string',
                'type' => [
                    'required',
                    Rule::in(['dish', 'drink']),
                ],
                'photo_url' => 'required|regex:/^.+\.(jpe?g)?(png)?(bmp)?(svg)?(gif)?$/',
                'price' => 'required|regex:/^\d+\.?\d{2}$/'
            ]);
        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }
        $item = new Item();
        $item->fill($request->except('photo_url'));

        if ($request->has('image')) 
        {
            $image_parts = explode(";base64,", $request->image);
            $image_base64 = base64_decode($image_parts[1]);
            file_put_contents('C:\temp\\'.$request->photo_url, $image_base64);
            //store uploaded photo
            $path = Storage::putFile('public/items', new File('C:\temp\\'.$request->photo_url));
            $path_parts = explode("/", $path);
            $item->photo_url = $path_parts[2];
        }

        $item->save();
        
        return response()->json(new ItemResource($item), 201);
    }

    public function update(Request $request, $id)
    {
        $validator = Validator::make($request->all(), [
                'name' => 'required|string|unique:items,name,'.$id,
                'description' => 'required|string',
                'type' => [
                    'required',
                    Rule::in(['dish', 'drink']),
                ],
                'photo_url' => 'required|regex:/^.+\.(jpe?g)?(png)?(bmp)?(svg)?(gif)?$/',
                'price' => 'required|regex:/^\d+\.?\d{2}$/'
            ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }

        $item = Item::find($id);

        if (!$item) 
        {
            $message = 'Item no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        if ($request->has('image')) 
        {
            $image_parts = explode(";base64,", $request->image);
            $image_base64 = base64_decode($image_parts[1]);
            file_put_contents('C:\temp\\'.$request->photo_url, $image_base64);
            //store uploaded photo
            $path = Storage::putFile('public/items', new File('C:\temp\\'.$request->photo_url));
            $path_parts = explode("/", $path);
            $item->photo_url = $path_parts[2];
        }
        
        $item->update($request->except('photo_url'));

        return response()->json(new ItemResource($item), 202);
    }
    
    public function destroy($id)
    {
        $item = Item::find($id);

        if (!$item) 
        {
            $message = 'Item no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $item->forceDelete();
        
        return response()->json(null, 204);
    }

    public function trash($id)
    {
        $item = Item::find($id);

        if (!$item) 
        {
            $message = 'Item no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $item->delete();

        if (! $item->trashed()) 
        {
            $message = 'Item no.: '.$id.' was not soft deleted.';
            return response()->json(compact('message'), 409);
        }
        
        $message = 'Item no.: '.$id.' was soft deleted.';
        return response()->json(compact('message'), 202);
    }

    public function averages(Request $request, $id) {
        
        $item = Item::find($id);

        if (!$item) 
        {
            $message = 'Item no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        if ($request->has('graphic') && $request->graphic) {
            $averages = $item->graphicAverages();
        } 
        else {
            $averages = $item->averages();
        }

        if ($request->has('page')) {
            $page = intval($request->page);
            $perPage = 6;
            $parts = $averages->chunk($perPage);
            $averages = new LengthAwarePaginator($parts[$page-1], $averages->count(), $perPage, LengthAwarePaginator::resolveCurrentPage($page), [
                    'path' => $request->url()
                ]);
        }

        if ($averages->isEmpty()) {
            $message = "No averages from handled orders found on Item no.: ".$id;
            return response()->json(compact('message'), 404);
        }
        
        return response()->json($averages, 200);
    }
}
