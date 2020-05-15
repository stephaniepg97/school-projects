<template>
	<div v-if="user != null">
		<div v-if="user.url" class="form-group">
	    	<img v-bind:src="user.url">
	    </div>
	    <div v-else="user.url" class="form-group">
	    	<img id="image" width="300px">
	    </div>
	    <div class="form-group" :class="{ 'form-group--error': $v.user.photo_url.$error }">
	    	<input type="file" name="photo_url" accept="image/*" id="photo" @change="getPhoto"/>
	    </div>
	    <div class="error" v-if="!$v.user.photo_url.image_pattern">Image format invalid. Accepts only: .jpeg, .jpg, png, .bmp, .svg, .gif</div>

		<div class="form-group" :class="{ 'form-group--error': $v.user.name.$error }">
	        <label class="form__label">Name</label>
	        <input class="form__input" v-model.trim="$v.user.name.$model"/>
	    </div>
	    <div class="error" v-if="!$v.user.name.required">Field is required</div>
	    <div class="error" v-if="!$v.user.name.name_pattern">Only letters and blank spaces.</div>

	    <div class="form-group" :class="{ 'form-group--error': $v.user.username.$error }">
	        <label class="form__label">Username</label>
	        <input class="form__input" v-model.trim="$v.user.username.$model"/>
	    </div>
	    <div class="error" v-if="!$v.user.username.required">Field is required</div>

	    <div class="form-group" :class="{ 'form-group--error': $v.user.email.$error }" v-if="($store.state.user != null) && ($store.state.user.type == 'manager')">
	        <label class="form__label">Email</label>
	        <input class="form__input" v-model.trim="$v.user.email.$model"/>
	    </div>
	    <div class="error" v-if="!$v.user.email.email">Invalid email</div>
	    
	    <div class="form-group" :class="{ 'form-group--error': $v.user.type.$error }">
	        <label class="form__label">Type</label>
	        <select name="type" v-model="$v.user.type.$model" >
	            <option value="manager">Manager</option>
	            <option value="cook">Cook</option>
	            <option value="cashier">Cashier</option>
	            <option value="waiter">Waiter</option>
	        </select>
	    </div>
	    <div class="error" v-if="!$v.user.type.required">Field is required</div>

	    <div class="form-group">
	        <a class="btn btn-primary" v-on:click.prevent="confirm">Confirm</a>
	        <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
	    </div>
	</div>
</template>

<script type="text/javascript">

	import { required, email, helpers } from 'vuelidate/lib/validators';

	const name_pattern = helpers.regex('name_pattern', /^[A-Za-záàâãéèêíóôõúçÁÀÂÃÉÈÍÓÔÕÚÇ\s]+$/);
	const image_pattern = helpers.regex('image_pattern', /^.+\.(jpe?g)?(png)?(bmp)?(svg)?(gif)?$/);

	export default {
        props: {
            user: {
            	type: Object
            } 
        },
        methods: {
	    	getPhoto: function() {
	    		this.user.photo_url = $("#photo")[0].files[0].name;
	    		this.user.url = null;
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
	        confirm: function(){
	        	this.$v.$touch();
			    if (this.$v.$invalid) {
			    	this.$root.success = false;
                    this.$root.message = 'Invalid form.';
			    } else {
	            	this.$emit('user-validated');
                }
	        },
	        cancel: function() {
                this.$router.go(-1);
            }
		},
		validations: {
			user: {
			    name: {
					required,
					name_pattern
			    },
			    username: {
			    	required
			    },
			    type: {
			    	required
			    },
			    email: {
					email
			    },
			    photo_url: {
			    	image_pattern
			    }
			}
	    }
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
