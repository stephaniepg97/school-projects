<template>
<div v-if="item != null">
    <div v-if="item.url" class="form-group">
        <img v-bind:src="item.url" width="300px">
    </div>
    <div v-else="item.url" class="form-group">
        <img id="image" width="300px">
    </div>
    <div class="form-group" :class="{ 'form-group--error': $v.item.photo_url.$error }">
        <input id="photo" type="file" name="photo_url" accept="image/*" @change="getPhoto"/>
    </div>
    <div class="error" v-if="!$v.item.photo_url.image_pattern">Image extension not valid! Accept only: jpeg, jpg, png, bmp, svg, gif.</div>

    <div class="form-group" :class="{ 'form-group--error': $v.item.type.$error }">
        <label class="form__label" for="type">Type</label>
        <select name="type" id="type" v-model="$v.item.type.$model" >
            <option value="dish">Dish</option>
            <option value="drink">Drink</option>
        </select>
    </div>
    <div class="error" v-if="!$v.item.type.required">Field is required</div>

    <div class="form-group" :class="{ 'form-group--error': $v.item.name.$error }">
        <label class="form__label">Name</label>
        <input class="form__input" v-model.trim="$v.item.name.$model"/>
    </div>
    <div class="error" v-if="!$v.item.name.required">Field is required</div>
    
    <div class="form-group" :class="{ 'form-group--error': $v.item.description.$error }">
        <label class="form__label">Description</label>
        <input class="form__textarea" v-model.trim="$v.item.description.$model"/>
    </div>
    <div class="error" v-if="!$v.item.description.maxLength">Description too long. Must have at maximum {{$v.item.description.$params.maxLength.max}} characters.</div>

    <div class="form-group" :class="{ 'form-group--error': $v.item.price.$error }">
        <label class="form__label">Price</label>
        <input
            type="number" step="0.01" class="form__input" v-model.trim="$v.item.price.$model"/>
    </div>
    <div class="error" v-if="!$v.item.price.price_pattern">Price need to be .00 pattern</div>
    <div class="error" v-if="!$v.item.price.required">Field is required</div>

    <div class="form-group">
        <a class="btn btn-primary" v-on:click.prevent="confirm">Confirm</a>
        <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
    </div>
</div>
</template>

<script type="text/javascript">

    import { required, helpers, maxLength } from 'vuelidate/lib/validators';

    const price_pattern = helpers.regex('price_pattern', /^\d+\.?\d{2}$/);
    const image_pattern = helpers.regex('image_pattern', /^.+\.(jpe?g)?(png)?(bmp)?(svg)?(gif)?$/);

	export default {
        props: {
            item: {
                type: Object
            } 
        },
        methods: {
            getPhoto: function() {
                this.item.photo_url = $("#photo")[0].files[0].name;
                this.item.url = null;
                if (window.File && window.FileReader && window.FileList && window.Blob) {
                    // Read files
                    var file = $("#photo")[0].files[0];
                    if (file) {
                        var reader = new FileReader();
                        reader.readAsDataURL(file);
                        reader.onload = (evt) => {
                            $("#image").attr('src', evt.target.result);
                            this.$store.commit('loadImage', evt.target.result);
                        };
                        reader.onerror = (evt) => {
                            console.error("An error ocurred reading the file.",evt);
                            this.$root.success = false;
                            this.$root.message = "An error ocurred reading the file."+evt;
                        };
                    } else {
                        this.$root.success = false;
                        this.$root.message = "Image invalid to read!";
                    }
                } else {
                    this.$root.success = false;
                    this.$root.message = "The File APIs are not fully supported by your browser.";
                }
            },
            confirm: function() {
                this.$v.$touch();
                if (this.$v.$invalid) {
                    this.$root.success = false;
                    this.$root.message = 'Invalid form.';
                } else {
                    this.$emit('item-validated');
                }
            },
            cancel: function() {
                this.$router.go(-1);
            }
        },
        validations: {
            item: {
                name: {
                    required
                },
                description: {
                    maxLength: maxLength(255)
                },
                price: {
                    required,
                    price_pattern
                },
                photo_url: {
                    image_pattern
                },
                type: {
                    required
                }
            }
        },
	};
</script>

<style scoped>  
.form-group {
    margin-bottom: 2rem;
}

.form__input, .form__textarea {
    position: relative;
    margin-bottom: 2rem;
}

.form__input, .form__textarea {
    font-family: "Lato", sans-serif;
    font-size: 0.875rem;
    font-weight: 300;
    color: #374853;
    line-height: 2.375rem;
    min-height: 2.375rem;
    position: relative;
    border: 1px solid #E8E8E8;
    border-radius: 5px;
    background: #fff;
    padding: 0 0.8125rem;
    width: 100%;
    transition: border .1s ease;
    box-sizing: border-box;
}

.form-group--error input, .form-group--error textarea, .form-group--error input:focus, .form-group--error input:hover {
    border-color: #f79483;
}

.form-group .form__input, .form-group .form__textarea {
    margin-bottom: 0;
}

.form-group--error + .error {
    display: block;
    color: #f57f6c;
}

.form-group__message, .error {
    font-size: 0.75rem;
    line-height: 1;
    display: none;
    margin-left: 14px;
    margin-top: -1.6875rem;
    margin-bottom: 0.9375rem;
}

</style>