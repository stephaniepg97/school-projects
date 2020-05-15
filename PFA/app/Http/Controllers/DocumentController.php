<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Document;
use App\Http\Requests\StoreDocumentRequest;
use App\Movement;

class DocumentController extends Controller
{
	public function __construct()
    {
        $this->middleware('auth');
    }

     public function show(Document $document)
    {
        $user = $document->user();
        $this->authorize('viewDocument',  $user);
    	$movement = Movement::where('document_id', $document->id)->first();
    	$url = $document->url();
        return view('documents.show', compact('movement', 'document', 'url'));
    }

    public function create(Movement $movement)
    {
        $user = $document->user();
        $document = new Document;
        return view('documents.add', compact('document', 'movement'));
    }

    public function store(StoreDocumentRequest $request, Movement $movement)
    {
        $user = $movement->user();
        $this->authorize('storeDocument',  $user);
        $data = $request->validated();
        $document = null;
        $stored_file = null; 

        if ($request->hasFile('file') && $request->file('file')->isValid()) 
        {
            $type = $data['file']->extension();
            $document = new Document;
            $document->fill([
                'type' => $type,     
                'original_name' => $movement->id,
                'description' => $data['document_description'],
            ]);

            //save new document
            $document->save();

           	//update movement
            $movement->document_id = $document->id;
            $movement->update();

            //store document
            $stored_file = $request->file('file')->storeAs('documents/'.$movement->account_id, $movement->id.'.'.$document->type);
        }

        $message = 'Document added successfully.';

        if ($stored_file !== null)
        {
            $message = $message.' File stored successfully.';
        }

        return redirect()
            ->route('movements.index', $movement->account_id)
            ->with('success', $message);
    }

    public function destroy(Document $document)
    {
        $user = $document->user();
        $this->authorize('destroyDocument', $user);
        $movement = $document->movement();

        $file_deleted = false;

        if ($document->hasFile())
        {
            $file_deleted = $document->deleteFile();
        }

        //set document_id due to CONSTRAINT: fk from movements table
        $movement->document_id = null;
        $movement->update();

        $document->delete();

        $message = 'Document deleted successfully.';

        if ($file_deleted)
        {
            $message.' File deleted from storage';
        }

        return redirect()
            ->route('movements.index', $movement->account_id)
            ->with('success', $message);
    }

    public function download(Document $document)
    {
        $user = $document->user();
        $this->authorize('viewDocument', $user);
    	$path = $document->path();

    	if (! $document->exists($path))
		{
			$movement = $document->movement();
    	 	return redirect()
            ->route('movements.index', $movement->account_id)
            ->with('unsuccess', 'Document does not have content to download!');
		}

        return response()->download($path);
    }

	public function view(Document $document)
    {
        $user = $document->user();
        $this->authorize('viewDocument', $user);
		if (! $document->hasFile())
		{
			$movement = $document->movement();

    	 	return redirect()
            ->route('movements.index', $movement->account_id)
            ->with('unsuccess', 'Document does not have content to see!');
		}
       	return response()->file($document->path());
    }
}
