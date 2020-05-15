<template>
	<div v-if="($store.state.user != null) && ($store.state.user.id == id)">
		<div class="form-group" :class="{ 'form-group--error': $v.request.password.$error }">
	        <label class="form__label" >Password</label>
	        <input class="form__input" type="password" v-model.trim="$v.request.password.$model"/>
	    </div>
	    <div class="error" v-if="!$v.request.password.required">Field is required.</div>
	    <div class="error" v-if="!$v.request.password.minLength">Password too short. Must have at least {{$v.request.password.$params.minLength.min}} characters.</div>

	    <div class="form-group" :class="{ 'form-group--error': $v.request.confirm.$error }">
	        <label class="form__label" >Confirm password</label>
	        <input class="form__input" type="password" v-model.trim="$v.request.confirm.$model"/>
	    </div>
	    <div class="error" v-if="!$v.request.confirm.required">Field is required.</div> 
	    <div class="error" v-if="!$v.request.confirm.sameAsPassword">Passwords must be identical.</div>
	    
	    <div class="form-group">
	    	<a class="btn btn-primary" v-on:click.prevent="save">Save</a>
	        <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
	    </div>
	</div>
</template>

<script type="text/javascript">

	import { required, minLength, sameAs } from 'vuelidate/lib/validators';

	export default {
		data: function(){
            return { 
                request: {
                	password: null,
		        	confirm: null
		        }
            }
        },
        props: {
            id: {
                required: true
            } 
        },
	    methods: {
	        save: function(){
	        	axios.patch('api/users/'+this.id+'/password', this.request)
                .then(response => {
		        	this.$root.success = true;
                    this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
		        })
				.catch(error => {
					this.$root.success = false;
				    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
				});
	        },
	        cancel: function(){
	        	this.$router.go(-1);
	        },
		},
		mounted() {
			this.$root.message = null;
		},
		validations: {
			request: {
            	password: {
            		minLength: minLength(3),
            		required
            	},
	        	confirm: {
	        		sameAsPassword: sameAs('password'),
	        		required
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
