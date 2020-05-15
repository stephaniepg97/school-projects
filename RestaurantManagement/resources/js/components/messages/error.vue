<template>
	<div class="alert alert-danger" v-if="$root.message">
		<span type="button" class="close-btn" onclick="this.parentElement.style.display='none';">&times;</span>
		<strong>Error!</strong>
		<p>{{ $root.message }}</p>
	</div>
</template>