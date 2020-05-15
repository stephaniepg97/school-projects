<template>
	<div class="alert alert-info" v-if="$store.state.user">
		<span type="button" class="close-btn" onclick="this.parentElement.style.display='none';">&times;</span>
		<strong>Meal terminated!</strong>
		<router-link :to="{ path: `/meals/${$root.newMeal.id}/details` }" class="ui basic button">View Meal</router-link>
	</div>
</template>