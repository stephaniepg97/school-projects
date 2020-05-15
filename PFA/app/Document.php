<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Http\UploadedFile;
use App\Movement;
use Illuminate\Support\Facades\Storage;
use Illuminate\Filesystem\Filesystem;

class Document extends Model
{
    protected $fillable = [
        'type',	 
		'original_name', 
		'description',
		'created_at',
    ];

    const CREATED_AT = 'created_at';

    public $timestamps = false;

    protected $table = 'documents';

    public function url()
    {
    	$movement = $this->movement();

    	//return asset('storage/app/documents/'.$movement->account_id.'/'.$movement->id.'.'.$this->type);

        return Storage::url('app/documents/'.$movement->account_id.'/'.$movement->id.'.'.$this->type);
    }

    public function path()
    {
    	$movement = $this->movement();

    	return storage_path('app/documents/'.$movement->account_id.'/'.$movement->id.'.'.$this->type);
    }
    
    public function hasFile()
    {
        $file = new Filesystem();

        return $file->exists($this->path());
    }

    public function getFile()
    {
        $file = new Filesystem();

        return $file->get($this->path());
    }

    public function deleteFile()
    {
         $file = new Filesystem();

         return $file->delete($this->path());
    }

    public function movement()
    {
        return Movement::findOrFail($this->original_name);
    }
}


