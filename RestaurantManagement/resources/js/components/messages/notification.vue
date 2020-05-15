<template>
	<div class="alert alert-info" v-if="$store.state.user">
		<span type="button" class="close-btn" onclick="this.parentElement.style.display='none';">&times;</span>
		<strong>{{ $root.message }}</strong>
		<router-link to="api/orders" class="ui basic button" v-if="$root.orders">View orders</router-link>
		<router-link to="api/meals" class="ui basic button" v-else="$root.orders">View Meals</router-link>
	</div>
</template>