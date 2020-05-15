<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class Table extends JsonResource
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
            'table_number' => $this->table_number,
            'meal' => $this->meal(),
            'created_at' => $this->created_at,
            'updated_at' => $this->updated_at,
            'deleted_at' => $this->deleted_at
         ];
    }

     public function meal()
    {
        $meal = $this->meals->where('state', 'active')->first();
        if ($meal == null)
        {
            $meal = $this->meals->where('state', 'terminated')->first();    
        }
        return $meal;
    }
}
