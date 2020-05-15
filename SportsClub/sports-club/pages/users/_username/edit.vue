<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <h4>Edit Modality</h4>
    <b-form @submit.prevent="save">
      <b-form-group label="Name:">
        <b-input name="name" v-model.trim="user.name" required />
      </b-form-group>
      <b-form-group label="Email:">
        <b-input name="email" v-model.trim="user.email" required />
      </b-form-group>
    </b-form>
    <button @click.prevent="save">Save</button>
    <button @click.prevent="cancel">Cancel</button>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
        user: {},
        fields: ['name', 'email']
    };
  },
  created() {
    this.$axios.$get(`/api/users/${this.$route.params.username}`)
    .then(user => {
        this.user = user || {}
    })
  },
  methods: {
    save() {
      this.$axios.$put(`api/users/${this.$route.params.username}`, this.user)
        .then(response => {
          console.log(response.data);
           this.$router.push("/users");
        })
        .catch(error => {
          console.log(error);
        });
    },
    cancel() {
      this.$router.push("/users");
    }
  }
};
</script>
