<template>
	<div v-if="user != null">
		<div class="form-group">
			<img v-bind:src="user.url">
		</div>
		<div class="form-group">
        	<label for="txtName">Name</label>
        	<input
            	type="text" class="form-control" readonly="readonly"
            	name="name" id="txtName" v-model="user.name" />
	    </div>
	    <div class="form-group">
        	<label for="txtUsername">Username</label>
        	<input
            	type="text" class="form-control" readonly="readonly"
            	name="username" id="txtUsername" v-model="user.username" />
	    </div>
	    <div class="form-group">
	        <label for="txtEmail">Email</label>
	        <input
	            type="email" class="form-control" readonly="readonly"
	            name="email" id="txtEmail" v-model="user.email"/>
	    </div>
	    <div class="form-group">
	        <label for="txtType">Type</label>
	        <input
	        	type="text" class="form-control" readonly="readonly"
	        	id="txtType" name="type" v-model="user.type"/>
	    </div>
	    <div class="form-group">
	        <label for="txtLastShiftStarted">Last Shift Started</label>
	        <input
	        	type="text" class="form-control" readonly="readonly"
	        	id="txtLastShiftStarted" name="lastShiftStarted" v-model="user.last_shift_start"/>
	    </div>
	    <div class="form-group">
	        <label for="txtLastShiftEnded">Last Shift Ended</label>
	        <input
	        	type="text" class="form-control" readonly="readonly"
	        	id="txtLastShiftEnded" name="lastShiftEnded" v-model="user.last_shift_end"/>
	    </div>
	    <div class="form-group">
	        <label for="txtOnShift">On Shift</label>
	        <input
	        	type="text" class="form-control" readonly="readonly"
	        	id="txtOnShift" name="onShift" v-bind:value="onShift"/>
	    </div>
		<div class="form-group">
	        <a class="btn btn-light" v-on:click.prevent="cancel">Return</a>
	        <router-link :to="{ path: `/users/${id}/edit` }" replace class="ui basic button"><i class="edit icon"></i></router-link>
	    </div>
	</div>
</template>

<script type="text/javascript">
	export default {
        data: function() { 
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
            cancel: function() {
                this.$router.go(-1);
            }
        },
		mounted () {
            this.$root.message = null;
            axios.get('api/users/'+this.id)
            .then(response=>{
                this.user = response.data;
            })
            .catch(error=>{
                this.$root.success = false;
                this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
            });
        },
		computed: {
			onShift: function() {
				return (this.user.shift_active) ? 'Yes' : 'No';
			}
		}
	};
</script>

<style scoped>	

</style>