<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;
use Illuminate\Http\UploadedFile;
use Illuminate\Support\Facades\Storage;
use Illuminate\Filesystem\Filesystem;
use App\Order;
use DateTime;

class User extends JsonResource
{
    /**
     * Transform the resource into an array.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return array
     */
    public function toArray($request)
    {
        return [
            'id' => $this->id,
            'name' => $this->name,
            'username' => $this->username,
            'email' => $this->email,
            'email_verified_at' => $this->email_verified_at,
            'type'=> $this->type,
            'blocked'=> $this->blocked,
            'photo_url'=> $this->photo_url,
            $this->mergeWhen($this->hasPhoto(), [
                'url'=> $this->url(),
            ]),
            'last_shift_start'=> $this->last_shift_start,
            'last_shift_end'=> $this->last_shift_end,
            'shift_active' => $this->shift_active,
            'created_at' => $this->created_at,
            'updated_at' => $this->updated_at,
            'deleted_at' => $this->deleted_at
        ];
    }

    public function url() {
        return asset('storage/profiles/'.$this->photo_url);
    }

    public function path()
    {
        return storage_path('app/public/profiles/'.$this->photo_url);
    }
    
    public function hasPhoto()
    {
        $file = new Filesystem();

        return $file->exists($this->path());
    }
}
