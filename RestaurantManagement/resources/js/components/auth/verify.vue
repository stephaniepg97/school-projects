<template>
	<div v-if="($store.state.user != null) && ($store.state.user.id == id) && !($store.state.user.email_verified_at)">
        <div class="form-group">
        	A fresh verification link has been sent to your email address. Before proceeding, please check your email for a verification link. If you did not receive the email, <a @click="resend">click here to request another</a>.
		</div>
        <div class="form-group">
	    	<a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
	    </div>
	</div>
</template>

<script type="text/javascript">
	export default {
		data: function(){
			return { 
				user: null,
			}
		},
		props: {
            id: {
                required: true,
                default: "-1"
            } 
        },
	   	methods: {
	   		resend: function() {
	   			axios.get('api/resend', this.$store.state.user)
                    .then(response=>{
                        this.$root.success = true;
                        this.$root.message = 'Email resent';
                    })
                    .catch(error=>{
                        console.log(error);
                        this.$root.success = false;
					    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
            },
	   		cancel: function() {
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