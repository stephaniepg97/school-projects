<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
      <h4>Create Graduation</h4>
      <b-form @submit.prevent="save">
        <b-form-group label="Id:">
          <b-input name="id" v-model.trim="graduation.id" disabled />
        </b-form-group>
        <b-form-group label="Name:">
          <b-input name="name" v-model.trim="graduation.name" required />
        </b-form-group>
        <b-form-group label="Value:">
          <b-input name="value" v-model.trim="graduation.value" required />
        </b-form-group>
        <b-form-group label="Stock:">
          <b-input name="stock" v-model.trim="graduation.stock" required />
        </b-form-group>
        <b-form-group label="Modality:">
          <b-input name="id" v-model.trim="graduation.modality.name" disabled />
        </b-form-group>
      </b-form>
    <div>
      <button class="btn btn-sm btn-primary" @click.prevent="save">Save</button>
      <button class="btn btn-sm btn-primary" @click.prevent="cancel">Cancel</button>
    </div>
    </b-container>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
      graduation: {
        id: null,
        name: null,
        value: null,
        stock: null,
        category: {
          name: 'Graduation',
          products: []
        },
        modality: {
          name: null
        }
      },
    };
  },
  created() {
    this.$axios.$get(`/api/modalities/${this.$route.params.id}`)
      .then(modality => {
          this.graduation.modality = modality || {}
      })
  },
  methods: {
    save() {
      this.$axios.$post(`/api/graduations`, this.graduation)
        .then(response => {
            this.graduation = response;
            this.$router.push(`/modalities/${this.$route.params.id}/edit`);
        })
        .catch(error => {
            console.log(error);
        });
    },
    cancel() {
      this.$router.push(`/modalities/${this.$route.params.id}/edit`);
    }
  }
};
</script>
