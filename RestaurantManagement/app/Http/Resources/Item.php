<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;
use Illuminate\Filesystem\Filesystem;
use App\Order;
use DateTime;

class Item extends JsonResource
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
            'type' => $this->type,      
            'description' => $this->description,   
            'photo_url' => $this->photo_url,     
            'price' => $this->price,
            'created_at' => $this->created_at,
            'updated_at' => $this->updated_at,
            'deleted_at' => $this->deleted_at,
            'invoices' => $this->whenLoaded('invoices'),
            'orders' => $this->whenLoaded('orders'),
            $this->mergeWhen($this->hasPhoto(), [
                'url'=> $this->url(),
            ])
         ];
    }

    public function url() {
        return asset('storage/items/'.$this->photo_url);
    }

    public function path()
    {
        return storage_path('app/public/items/'.$this->photo_url);
    }

    public function hasPhoto()
    {
        $file = new Filesystem();

        return $file->exists($this->path());
    }
}
