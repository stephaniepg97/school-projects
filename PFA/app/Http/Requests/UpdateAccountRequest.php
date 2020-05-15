<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;
use App\AccountType;
use Route;

class UpdateAccountRequest extends FormRequest
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
        $size = AccountType::size();
        $account = Route::current()->parameter('account');
        return [
            'start_balance' => 'required|regex:/^\d+\.?\d{2}$/',
            'account_type_id' => 'required|integer|between:1,'.$size,
            'code' => 'required|string|unique:accounts,code,'.$account->id.'|size:10',
            'description' => 'nullable|string',
        ];
    }

    public function messages()
    {
        return [
            'start_balance.regex' => 'Value is not decimal(2): 00.00',
        ];
    }
}
