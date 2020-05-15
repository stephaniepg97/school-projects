<template>
	<div class="alert alert-success" v-if="$root.message">
		<span type="button" class="close-btn" onclick="this.parentElement.style.display='none';">&times;</span>
		<strong>Success!</strong>
		<p>{{ $root.message }}</p>
	</div>
</template>