<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
      <h4>Graduation Details</h4>
      <p>Id: {{ graduation.id }}</p>
      <p>Name: {{ graduation.name }}</p>
      <p>Value: {{ graduation.value }}</p>
      <p>Stock/Limit: {{ graduation.stock }}</p>
      <p>Modality: {{ graduation.modality.name }}</p>
      <p>Original Id: {{ graduation.originalId }}</p>
      <nuxt-link class="btn btn-sm btn-primary" :to="`/graduations/${this.$route.params.id}/edit`">Edit</nuxt-link>
    </b-container>
  </div>
</template>
<script>
export default {
  data() {
    return {
      graduation: {
        modality: {
          name: null
        }
      }
    };
  },
  created() {
    this.$axios.$get(`/api/graduations/${this.$route.params.id}`)
    .then(graduation => {
      this.graduation = graduation || {}
    })
  }
};
</script>
