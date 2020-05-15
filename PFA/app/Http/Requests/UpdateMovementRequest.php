<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;
use App\MovementCategory;

class UpdateMovementRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        $current_date = date("Y-m-d");
        $size = MovementCategory::size();
        return [
            'movement_category_id' => 'required|integer|between:1,'.$size,
            'date' => 'required|date|before_or_equal:'.$current_date,
            'value' => 'required|regex:/^\d+\.?\d{2}$/',
            'file' => 'nullable|file',
            'description' => 'nullable|string',
            'document_description' => 'nullable|string',
        ];
    }

    public function messages()
    {
        return [
            'value.regex' => 'Value is not decimal(2): 00.00',
        ];
    }
}
