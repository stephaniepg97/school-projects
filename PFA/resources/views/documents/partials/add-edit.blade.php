    <div class="form-group">
        <label for="inputDocument_file">{{ __('Document') }}</label>
        <input
            type="file" class="form-control"
            name="file" id="inputDocument_file"  
            value="{{ old('file') }}" 
            />
    </div>
    <div class="form-group">
        <label for="inputDocument_description">{{ __('Document Description') }}</label>
        <input
            type="text" class="form-control"
            name="document_description" id="inputDocument_description"
            value="{{ old('document_description') }}" 
            />
    </div>