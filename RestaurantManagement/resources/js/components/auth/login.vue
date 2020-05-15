<template>
	<div>
	    <div class="form-group">
	        <label for="inputEmail">Email</label>
	        <input
	            type="email" class="form-control" v-model.trim="user.email"
	            name="email" id="inputEmail" placeholder="Email address"/>
	    </div>
	    <div class="form-group">
	        <label for="inputPassword">Password</label>
	        <input
	            type="password" class="form-control" v-model="user.password"
	            name="password" id="inputPassword"/>
	    </div>
	    <div class="form-group">
	    	<a class="btn btn-primary" v-on:click.prevent="login">Login</a>
	    	<a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
	    </div>
	</div>
</template>

<script type="text/javascript">
	export default {
		data: function(){
			return { 
				user: {
		        	email: "",
		        	password: "",
                }
			}
		},
	   	methods: {
            login() {
                axios.post('api/login', this.user)
                    .then(response => {
                        this.$store.commit('setToken',response.data.access_token);
                        return axios.get('api/users/me');
                    })
                    .then(response => {
                    	console.log(response.data.data);
                        this.$store.commit('setUser',response.data.data);
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + 'User authenticated correctly';
                        if (!this.$store.state.user.email_verified_at) {
				            this.$router.replace({ path: `/users/${this.$store.state.user.id}/verify`});
				        } else {
                        	this.$router.replace({ path: "/dashboard" });
                    	}
                    })
                    .catch(error => {
                        this.$store.commit('clearUserAndToken');
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    })
            },
            cancel() {
            	this.$router.go(-1);
            }
		},
		mounted() {
			this.$root.message = null;
		}
	};
</script>

<style scoped>	

</style>