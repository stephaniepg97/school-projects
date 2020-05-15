<?php

namespace App\Http\Controllers;

use App\Invoice;
use App\User;
use App\Item;
use Illuminate\Support\Facades\Storage;
use App\Http\Resources\Invoice as InvoiceResource;
use Illuminate\Support\Facades\DB;
use PDF;

class StorageController extends Controller
{
    /*public function invoice(Request $request)
    {
    	$invoice = Invoice::find($request->id);

        if (!$invoice) 
        {
            return response()->json(['error' => 'Invoice Not Found.'], 404);
        }

    	$pdf = PDF::loadView('pdf', $request);

    	return $pdf->download($request->nif.'_'.$request->date.'.pdf');
    }*/

    public function uploadInvoice(InvoiceResource $invoice)
    {
       
        $pdf = PDF::loadView('invoices.pdf', $invoice->all());

        $pdf->save($invoice->path());

        if (! $invoice->hasFile())
        {
            return response()->json(['error' => 'Pdf document not created.'], 409);
        }

        return response()->json(['success' => 'Pdf document created successfully.'], 202);
    }

    public function uploadProfilePhoto(Request $request, $id)
    {
        //store uploaded photo
        //$path = Storage::putFileAs('profiles', $request->file('photo'), $user->photo_url);
    }

    public function downloadInvoice($id)
    {
    	$invoice = Invoice::find($id);

        if (!$invoice) 
        {
            return response()->json(['error' => 'Invoice Not Found.'], 404);
        }

        if (! $invoice->hasFile())
		{
			return response()->json(['error' => 'Document not found in storage.'], 404);
		}

        return response()->download($invoice->path());
    }

    public function user($id)
    {
        $user = User::find($id);

        if (!$user) 
        {
            return response()->json(['error' => 'User Not Found.'], 404);
        }

        if (! $user->hasPhoto())
        {
            return response()->json(['error' => 'Profile photo not found in storage.'], 404);
        }

        return response()->file($user->path());
    }

    public function item($id)
    {
        $item = Item::find($id);

        if (!$item) 
        {
            return response()->json(['error' => 'Item Not Found.'], 404);
        }

        if (! $item->hasPhoto())
        {
            return response()->json(['error' => 'item photo not found in storage.'], 404);
        }

        return response()->file($item->path());
    }
}