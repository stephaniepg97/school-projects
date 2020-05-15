<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://127.0.0.1:3000/">Home</a>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/modalities" class>Modalities</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
      <div class="search-container"></div>
    </div>
    <b-form @submit.prevent="onSubmit">
      <h4>Product Edit</h4>
      <b-form-group label="Id:">
        <b-input name="id" v-model.trim="product.id" required />
      </b-form-group>
      <b-form-group label="Name:">
        <b-input name="name" v-model.trim="product.name" required />
      </b-form-group>
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

      <b-form-group label="Stock:">
        <b-input name="stock" v-model.trim="product.stock" required />
      </b-form-group>

      </b-form-group>
      <b-form-group label="Value:">
        <b-input name="value" v-model.trim="product.value" required />
      </b-form-group>
    </b-form>
    <b-button type="submit" @click.prevent="onSubmit" class="btn-warning">Save</b-button>
    <nuxt-link to="/products" class="btn">Return</nuxt-link>
  </div>
</template>

<script>
export default {
  data() {
    return {
      user: {},
      product: {
        category: {
          name: null,
          products: [],
          version: null
        },
        id: null,
        name: null,
        stock: null,
        value: null,
        version: null,
        modality: {
          id: null,
          name: null,
          products: [],
          version: null
        },
        registrations: []
      },
      categories: []
    };
  },
  computed: {},
  methods: {
    onSubmit() {
      //console.log(this.product);
      this.$axios.put("/api/products/" + this.product.id, this.product)
        .then(response => {
          this.showFailure = false;
        })
        .then(response => {
          this.showFailure = false;

          this.$emit("product-save");
        })
        .catch(error => {
          console.log(error);
        });
    }
  },
  created() {
    this.$axios
      .$get(`/api/products/${this.$route.params.id}`)
      .then(product => (this.product = product || {}));
    this.$axios.$get("/api/categories").then(categories => {
      this.categories = categories;
      //console.log(this.product);
    });
  }
};
</script>

<style>
* {
  box-sizing: border-box;
}

body {
  margin: 0;
  font-family: Arial, Helvetica, sans-serif;
}

.topnav {
  overflow: hidden;
  background-color: #e9e9e9;
}

.topnav a {
  float: left;
  display: block;
  color: black;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
  font-size: 17px;
}

.topnav a:hover {
  background-color: #ddd;
  color: black;
}

.topnav a.active {
  background-color: #2196f3;
  color: white;
}

.topnav .search-container {
  float: right;
}

.topnav input[type="text"] {
  padding: 6px;
  margin-top: 8px;
  font-size: 17px;
  border: none;
}

.topnav .search-container button {
  float: right;
  padding: 6px 10px;
  margin-top: 8px;
  margin-right: 16px;
  background: #ddd;
  font-size: 17px;
  border: none;
  cursor: pointer;
}

.topnav .search-container button:hover {
  background: #ccc;
}

@media screen and (max-width: 600px) {
  .topnav .search-container {
    float: none;
  }
  .topnav a,
  .topnav input[type="text"],
  .topnav .search-container button {
    float: none;
    display: block;
    text-align: left;
    width: 100%;
    margin: 0;
    padding: 14px;
  }
  .topnav input[type="text"] {
    border: 1px solid #ccc;
  }
}
</style>
