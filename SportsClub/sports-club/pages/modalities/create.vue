<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
      <h4>Create New Modality</h4>
      <b-form @submit.prevent="create">
        <b-form-group label="Name:">
          <b-input name="name" v-model.trim="modality.name" required />
        </b-form-group>
      </b-form>
      <div>
        <button class="btn btn-sm btn-primary" @click.prevent="create">Create</button>
        <button class="btn btn-sm btn-primary" @click.prevent="cancel">Cancel</button>
      </div>
    </b-container>
  </div>
</template>

<script>
    export default {
        data() {
            return {
                modality: {
                    id: null,
                    name: null,
                    graduations: [],
                    classes: []
                },
                fields: ['id', 'name', 'value', 'stock', 'originalId']
            };
        },
        methods: {
            create() {
              this.$axios.$post('api/modalities', this.modality)
              .then(response => {
                  this.modality = response;
                  this.$router.push(`/modalities/${this.modality.id}`);
              })
              .catch(error => {
                  console.log(error);
              });
            },
            cancel() {
                this.$router.push("/modalities");
            }
        }
    };
</script>
