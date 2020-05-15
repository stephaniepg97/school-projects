<div class="form-group row">
    <label for="input_account_type_id" class="col-md-4 col-form-label text-md-right">{{ __('Account Type') }}</label>

    <div class="col-md-6">
    <select name="account_type_id" id="input_account_type_id">
        <option disabled selected> -- select an option -- </option>
            @foreach($types as $type)
        <option value="{{ $type->id }}">{{ $type->name }}</option>
            @endforeach
    </select>
    </div>
</div>

<div class="form-group row">
    <label for="start_balance" class="col-md-4 col-form-label text-md-right">{{ __('Start Balance') }}</label>
    <div class="col-md-6">
        <input id="start_balance" type="text" class="form-control{{ $errors->has('start_balance') ? ' is-invalid' : '' }}" name="start_balance" value="{{ old('start_balance', $account->start_balance) }}" required>
    </div>
</div>

<div class="form-group row">
    <label for="input_code" class="col-md-4 col-form-label text-md-right">{{ __('Code') }}</label>
    <div class="col-md-6">
    	<input
            id="input_code" type="text" class="form-control{{ $errors->has('code') ? ' is-invalid' : '' }}" 
            name="code"  value="{{ old('code', $account->code) }}" required>
    </div>
</div>
<div class="form-group row">
    <label for="input_description" class="col-md-4 col-form-label text-md-right">{{ __('Description') }}</label>
    <div class="col-md-6">    
        <input
            id="input_description"  type="text" class="form-control{{ $errors->has('description') ? ' is-invalid' : '' }}" 
            name="description" value="{{ old('description', $account->description) }}" >
    </div>
</div>