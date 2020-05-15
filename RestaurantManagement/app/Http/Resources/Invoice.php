<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;
use Illuminate\Filesystem\Filesystem;
use App\Http\Resources\Meal as MealResource;

class Invoice extends JsonResource
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
            'state'=> $this->state,
            'nif'=> $this->nif,
            'total_price'=> $this->total_price,
            'date'=> $this->date,
            'customer_name'=> $this->name,
            'created_at' => $this->created_at,
            'updated_at' => $this->updated_at,
            'meal' => new MealResource($this->meal),
            'items' =>$this->whenLoaded('items'),
            'filename' => $this->filename(),
            $this->mergeWhen($this->hasFile(), [
                'url'=> $this->url(),
            ]),
        ];
    }

    public function filename()
    {
        return $this->nif.'_'.$this->date.'.pdf';
    }

    public function path()
    {
        return storage_path('app/public/invoices/'.$this->nif.'_'.$this->date.'.pdf');
    }

    public function url() {
        return asset('storage/invoices/'.$this->nif.'_'.$this->date.'.pdf');
    }

    public function hasFile()
    {
        $file = new Filesystem();

        return $file->exists($this->path());
    }
    
}
