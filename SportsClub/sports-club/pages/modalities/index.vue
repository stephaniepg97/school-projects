<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class="">Products</nuxt-link>
      <nuxt-link to="/payments" class="">Payments</nuxt-link>
      <nuxt-link to="/users" class="">Users</nuxt-link>
      <div class="search-container">
        <form action="/action_page.php">
          <input type="text" placeholder="Search.." name="search" />
          <button @click.prevent>Go</button>
          <i class="fa fa-search"></i>
        </form>
      </div>
    </div>
    <b-container>
      <b-table striped over :items="modalities" :fields="fields">
        <template v-slot:cell(actions)="row">
          <div class="custom-actions">
            <nuxt-link class="btn btn-sm btn-primary" :to="`/modalities/${row.item.id}/`">Details</nuxt-link>
            <nuxt-link class="btn btn-sm btn-primary" :to="`/modalities/${row.item.id}/edit`">Edit</nuxt-link>
            <button class="btn btn-sm btn-danger" @click.prevent="deleteItem(row.item.id)">Delete</button>
          </div>
        </template>
      </b-table>
      <nuxt-link to="/modalities/create" class="btn btn-primary">Create a New Modality</nuxt-link>
      <div class="alert alert-success" v-if="showSuccess">
        <button type="button" class="close-btn" v-on:click="showSuccess=false">&times;</button>
        <strong>{{ successMessage }}</strong>
      </div>
      <div class="alert alert-danger" v-if="showFailure">
        <button type="button" class="close-btn" v-on:click="showFailure=false">&times;</button>
        <strong>{{ failMessage }}</strong>
      </div>
    </b-container>
  </div>
</template>

<script>
export default {
  data() {
    return {
      showSuccess: false,
      successMessage: "",
      showFailure: false,
      failMessage: "",
      modalities: [],
      fields: ["id", "name", "actions"]
    };
  },
  created() {
    this.$axios.$get("/api/modalities").then(modalities => {
      console.log(modalities);
      this.modalities = modalities;
    });
  },
  methods: {
    deleteItem(id) {
      this.$axios.$delete(`/api/modalities/${id}`)
        .then(response => {
          console.log(response.data);
          this.showSuccess = true;
          this.successMessage = "Modality Deleted";
          this.$router.go();
        })
        .catch(error => {
          console.log(error.response);
          this.showFailure = true;
          this.failMessage = error.response;
        });
    }
  }
};
</script>
