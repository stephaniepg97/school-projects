<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Contracts\Support\Jsonable;

use App\Http\Resources\Invoice as InvoiceResource;
use Illuminate\Support\Facades\DB;
use Illuminate\Pagination\LengthAwarePaginator;

use PDF;
use Validator;
use App\Invoice;
use App\User;

class InvoiceControllerAPI extends Controller
{
    public function index(Request $request)
    {
        $query = [];
        if ($request->has('state')) {
            array_push($query, ['state', $request->state]);
        }
        if ($request->has('date')) {
            array_push($query, ['date', $request->date]);
        }
        $invoices = Invoice::where($query);
        if ($request->has('waiter')) {
            $name = $request->waiter;
            $invoices = $invoices->whereHas('meal', function ($query) use($name) {
                $query->whereHas('waiter', function ($query) use($name) {
                    $query->where('name', 'like', '%'.$name.'%');
                });
            });
        } 
        if ($request->has('page')) {
            $invoices = $invoices->paginate(5);
        }
        else {
            $invoices = $invoices->get();
        }
        if ($invoices->isEmpty()) {
            $message = "No invoices found.";
            return response()->json(compact('message'), 404);
        } 
        return InvoiceResource::collection($invoices);
    }

    public function show($id)
    {
        $invoice = Invoice::find($id);

        if (!$invoice) 
        {
            $message = 'Invoice no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        return response()->json(new InvoiceResource($invoice), 200);
    }

    public function store(Request $request)
    {
        $validator = Validator::make($request->all(), [
                'meal_id'=> [
                    'required',
                    'numeric',
                    function ($attribute, $value, $fail) {
                        if (!Meal::findOrFail($value)) {
                            $fail('Meal does not exist');
                        }
                    }
                ],
                'date' => 'required|date',
                'total_price' => 'required|regex:/^\d+\.?\d{2}$/'
            ]);
        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }
        $invoice = new Invoice();
        $invoice->fill($request->all());
        $invoice->state = 'pending';
        $invoice->save();
        return response()->json(new InvoiceResource($invoice), 201);
    }
    
    public function destroy($id)
    {
        $invoice = Invoice::find($id);

        if (!$invoice) 
        {
            $message = 'Invoice no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $invoice->delete();
        return response()->json(null, 204);
    }

    public function notPaid($id)
    {
        $invoice = Invoice::find($id);

        if (!$invoice) 
        {
            $message = 'Invoice no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        if ($invoice->state != 'pending') 
        {
            $message = 'Invoice no.: '.$id.' is not at state "pending" anymore.';
            return response()->json(compact('message'), 409);
        }

        $invoice->state = 'not paid';
        $invoice->update();

        $message = 'Invoice no.: '.$id.' state set as "not paid".';
        return response()->json(compact('message'), 202);
    }

    public function pay(Request $request, $id)
    {
        $validator = Validator::make($request->all(), [
                'name' => 'required|regex:/^[A-Za-záàâãéèêíóôõúçÁÀÂÃÉÈÍÓÔÕÚÇ\s]+$/',
                'nif' => 'required|digits:9'
            ]);

        if ($validator->fails()) {
            $message = implode("\n", $validator->errors()->all());
            return response()->json(compact('message'), 409);
        }

        $invoice = Invoice::find($id);

        if (!$invoice) 
        {
            $message = 'Invoice no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        if ($invoice->state != 'pending') 
        {
            $message = 'Invoice no.: '.$id.' is not at state "pending" anymore.';
            return response()->json(compact('message'), 409);
        }

        if (!$invoice->meal) 
        {
            return response()->json(['error' => 'Invoice does not has a Meal associated.'], 404);
        }

        $invoice->fill($request->all());
        $invoice->state = 'paid';
        $invoice->update();

        //SET ASSOCIATED MEAL AS "PAID"
        $invoice->meal->state = 'paid';
        $invoice->meal->update();

        if ($invoice->state != 'paid') 
        {
            $message = 'Invoice no.: '.$id.' is not set as "paid"';
            return response()->json(compact('message'), 409);
        }

       // dd(json_encode($invoice->receipt()));

        $pdf = PDF::loadView('invoices.pdf', compact('invoice'));

        $pdf->save($invoice->path());

        if (! $invoice->hasFile())
        {
            $message = 'Pdf document with information about Invoice no.: '.$id.' not created.';
            return response()->json(compact('message'), 409);
        }

        $message = 'Payment done successfully. Invoice created with no.: ' + $id + '. Pdf document with payment details generated.';
        return response()->json(compact('message'), 202);
    }

    public function pdf($id)
    {
        $invoice = Invoice::find($id);

        if (!$invoice) 
        {
            $message = 'Invoice no.: '.$id.' not found.';
            return response()->json(compact('message'), 404);
        }

        $pdf = PDF::loadView('invoices.pdf', compact('invoice'));

        return $pdf->download($invoice->filename());
    }
}
