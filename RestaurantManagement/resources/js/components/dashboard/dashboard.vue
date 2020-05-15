<template>
<div>
	<h3>Invoices</h3> 
	<div class="form-group">
		<p><em>Observe at real time a summary of the information about “pending” invoices: </em></p>
		<router-link :to="{ path: `/invoices?state=pending` }" replace class="ui basic button">Pending Invoices</router-link>
	</div>

	<h3>Meals</h3> 
	<div class="form-group">
		<p><em> Observe at real time a summary of the information about “active” and “terminated” meals and all orders associated with these meals: </em></p>
		<router-link :to="{ path: `/meals?state=active` }" replace class="ui basic button">Active Meals</router-link>
		<router-link :to="{ path: `/meals?state=terminated` }" replace class="ui basic button">Terminated Meals</router-link>
	</div>
</div>
</template>

<script type="text/javascript">

//<div class="form-group" v-if="$store.state.user">

//	    <div v-if="$store.state.user.type == 'manager'">

	
	export default {
		data: function() { 
            return {
                values: {}
            }
        },
		methods: {
			
		}
	};
</script>

<style>
.form-group {
    margin-top: 2rem;
    margin-bottom: 2rem;
    padding: 5px;
}
button.ui.button {
    padding: 8px 8px;
    margin-top: 3px;
    margin-bottom: 3px;
}
</style>
