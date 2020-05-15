<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://127.0.0.1:3000/">Home</a>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/modalities" class>Modalities</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
      <div class="search-container"></div>
    </div>
    <b-container>
      <h4>User Details</h4>
      <p>Name: {{ user.name }}</p>
      <p>Username: {{ user.username }}</p>
      <p>Email: {{ user.email }}</p>
    </b-container>
    <nuxt-link to="/users" class="btn btn-danger">Return</nuxt-link>
    <nuxt-link :to="`/users/${this.$route.params.username}/edit`" class="btn btn-warning">Edit</nuxt-link>
  </div>
</template>
<script>
export default {
  data() {
    return {
      user: {},
      fields: ["name", "username", "email"]
    };
  },
  computed: {},
  created() {
    this.$axios.$get(`/api/users/${this.$route.params.username}`).then(user => {
      this.user = user || {};
    });
  }
};
</script>
