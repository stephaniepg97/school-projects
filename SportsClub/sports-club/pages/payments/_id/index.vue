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
      <h4>Payment Details</h4>
      <p>Id: {{ payment.id }}</p>
      <p>Quantity: {{ payment.quantity }}</p>
      <p> Payed: {{ payment.payed }}</p>
      <p> State: {{ payment.state }}</p>
    </b-container>
    <nuxt-link to="/payments" class="btn btn-danger">Return</nuxt-link>
    <nuxt-link :to="`/payments/${this.$route.params.id}/edit`" class="btn btn-warning">Edit</nuxt-link>
  </div>
</template>
<script>
export default {
  data() {
    return {
      payment: { },
      fields: ['id', 'quantity', 'payed', 'state']
    };
  },
  created() {
      this.$axios.$get(`/api/payments/${this.$route.params.id}`)
          .then(payment => {
              this.payment = payment || {}
          })
  },
};

</script>
