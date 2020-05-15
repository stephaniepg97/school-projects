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
      <h4>Product Details</h4>
      <p>Id: {{ product.id }}</p>
      <p>Name: {{ product.name }}</p>
      <p>Category: {{ product.category.name }}</p>
      <p>Price: {{ product.value }}</p>
    </b-container>
    <nuxt-link to="/products" class="btn btn-danger">Return</nuxt-link>
    <nuxt-link :to="`/products/${this.$route.params.id}/edit`" class="btn btn-warning">Edit</nuxt-link>
  </div>
</template>
<script>
export default {
  data() {
    return {
      product: {
        category: { name: null }
      }
    };
  },
  computed: {},
  created() {
    this.$axios
      .$get(`/api/products/${this.$route.params.id}`)
      .then(product => (this.product = product || {}));
    //console.log(this.product);
  }
};
</script>
