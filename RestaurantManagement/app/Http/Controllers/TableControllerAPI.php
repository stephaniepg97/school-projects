<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Contracts\Support\Jsonable;

use App\Http\Resources\Table as TableResource;

use App\Table;
use Validator;

class TableControllerAPI extends Controller
{
    public function index(Request $request)
    {
        if ($request->has('page')) {
            return TableResource::collection(Table::paginate(5));
        } else {
            return TableResource::collection(Table::all());
        }
    }

    public function show($id)
    {
        $table = Table::findTable($id);
        if (!$table) 
        {
            $message = 'Table no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        return response()->json(new TableResource($table), 200);
    }

    public function store(Request $request)
    {
        $validator = Validator::make($request->all(), [
                'table_number' => 'required|numeric|unique:restaurant_tables,table_number',
            ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }

        $table = new Table();
        $table->fill($request->all());
        $table->save();
        return response()->json(new TableResource($table), 201);
    }
    
    public function destroy($id)
    {
        $table = Table::findTable($id);
        if (!$table) 
        {
            $message = 'Table no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        
        $table->forceDelete();
        return response()->json(null, 204);
    }

    public function trash($id)
    {
        $table = Table::findTable($id);
        if (!$table) 
        {
            $message = 'Table no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }
        $table->delete();
        if (!$table->trashed()) 
        {
            $message = 'Table no.: '.$id.' was not soft deleted.';
            return response()->json(compact('message'), 409);
        }
        return response()->json(null, 204);
    }
}
