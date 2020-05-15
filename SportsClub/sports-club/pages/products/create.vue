<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/modalities" class>Modalities</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
      <div>
        <h1>Create a new Product</h1>

        <form @submit.prevent="create">
          <b-form-group
            id="name"
            description="The name is required"
            label="Enter product name"
            label-for="name"
          >
            <b-input id="name" v-model.trim="product.name" trim></b-input>
          </b-form-group>

          <b-input v-model.trim="product.value" required placeholder="Enter product value" />

          <b-form-group label="Category:">
            <!-- <b-input name="category" v-model.trim="product.category.name" required /> -->
            <b-select
              v-model="product.category.name"
              :options="categories"
              required
              value-field="name"
              text-field="name"
            >
              <template v-slot:first>
                <option :value="null" disabled>-- Please select the Category --</option>
              </template>
            </b-select>
          </b-form-group>

          <b-form-group label="Modality:">
            <!-- <b-input name="category" v-model.trim="product.category.name" required /> -->
            <b-select
              v-model="product.modality.name"
              :options="modalities"
              required
              value-field="id"
              text-field="name"
            >
              <template v-slot:first>
                <option :value="null">-- Select the Modality --</option>
              </template>
            </b-select>
          </b-form-group>

          <b-form-group label="Stock:">
            <b-input name="stock" v-model.trim="product.stock" required />
          </b-form-group>
          <br />
          <p class="text-danger" v-show="errorMsg">{{ errorMsg }}</p>

          <nuxt-link to="/products" class="btn btn-primary">Return</nuxt-link>

          <button type="reset" class="btn btn-warning" @click="reset">Reset</button>
          <button class="btn btn-success" @click.prevent="create">Create</button>
        </form>
      </div>
    </b-container>
  </div>
</template>

<script>
export default {
  data() {
    return {
      product: {
        category: {
          name: null
        },
        name: null,
        stock: null,
        value: null,
        modality: {
          id: null,
          name: null,
          products: [],
          version: null
        },
        registrations: []
      },
      categories: [],
      modalities: [],
      errorMsg: false
    };
  },

  created() {
    this.$axios.$get("/api/categories").then(categories => {
      this.categories = categories;
      //console.log(this.product);
    });
    this.$axios.$get("/api/modalities").then(modalities => {
      this.modalities = modalities;
      //console.log(this.product);
    });
  },

  computed: {},

  methods: {
    reset() {
      this.errorMsg = false;
    },

    create() {
      this.$axios
        .$post("/api/products", this.product)
        .then(() => {
          this.$router.push("/products");
        })
        .catch(error => {
          this.errorMsg = error.response.data;
        });
    }
  }
};
</script>
