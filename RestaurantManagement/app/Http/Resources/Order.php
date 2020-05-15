<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;
use App\Order as OrderModel;
use DateTime;

class Order extends JsonResource
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
            'created_at' => $this->created_at,
            'updated_at' => $this->updated_at,
            'meal'=> $this->meal,
            'waiter'=> $this->meal->waiter,
            'cook'=> $this->cook,
            'item'=> $this->item
        ];
    }
}
