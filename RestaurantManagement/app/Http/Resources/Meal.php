<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;
use App\Http\Resources\User as UserResource;
use DateTime;
use App\Meal as MealModel;

class Meal extends JsonResource
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
            'start'=> $this->start,
            'end'=> $this->end,
            'total_price_preview'=> $this->total_price_preview,
            'created_at' => $this->created_at,
            'updated_at' => $this->updated_at,
            'invoice'=> $this->invoice,
            'table'=> $this->table, 
            'waiter'=> new UserResource($this->waiter),  
            'orders' => $this->whenLoaded('orders'),
        ];
    }
}
