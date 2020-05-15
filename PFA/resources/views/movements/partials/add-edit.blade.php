    <div class="form-group">
        <label for="input_movement_category_id">{{ __('Category') }}</label>
        <select name="movement_category_id" id="input_movement_category_id">
                <option disabled selected> -- select an option -- </option>
                @foreach($categories as $category)
                <option value="{{ $category->id }}">{{ $category->name }}, {{ $category->type }}</option>
                @endforeach
        </select>

    </div>
    <div class="form-group">
        <label for="inputDate">{{ __('Date') }}</label>
        <input
            type="date" class="form-control"
            name="date"  id="inputDate" 
            value="{{ old('date', $movement->date) }}" 
            required/>
    </div>
    <div class="form-group">
        <label for="inputAmount">{{ __('Value') }}</label>
        <input
            type="text" class="form-control"
            name="value" id="inputAmount" 
            value="{{ old('value', $movement->value) }}" 
            required/>
    </div>
    <div class="form-group">
        <label for="inputdescription">{{ __('Description') }}</label>
        <input
            type="text" class="form-control"
            name="description" id="inputdescription"  
            value="{{ old('description', $movement->description) }}" 
            />
    </div>