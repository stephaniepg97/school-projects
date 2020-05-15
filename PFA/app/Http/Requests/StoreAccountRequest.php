<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;
use App\AccountType;

class StoreAccountRequest extends FormRequest
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
        $size = AccountType::size();
        return [
            'start_balance' => 'required|regex:/^\d+\.?\d{2}$/',
            'account_type_id' => 'required|integer|between:1,'.$size,
            'code' => 'required|string|unique:accounts,code|size:10',
            'description' => 'nullable|string',
            'date' => 'required|date|before_or_equal:'.$current_date,
        ];
    }

    public function messages()
    {
        return [
            'start_balance.regex' => 'Value is not decimal(2): 00.00',
        ];
    }
}
