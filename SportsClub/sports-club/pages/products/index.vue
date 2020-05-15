<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://127.0.0.1:3000/">Home</a>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/modalities" class>Modalities</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
      <div class="search-container">
        <form action="/action_page.php">
          <input type="text" placeholder="Search.." name="search" />
          <button type="submit">
            <i class="fa fa-search"></i>
          </button>
        </form>
      </div>
    </div>
    <b-container>
      <b-table striped over :items="products" :fields="fields">
        <template v-slot:cell(actions)="row">
          <nuxt-link class="btn btn-sm btn-primary" :to="`/products/${row.item.id}`">Details</nuxt-link>
          <button class="btn btn-sm btn-danger" v-on:click.prevent="deleteProduct(row.item.id)">Delete</button>
          <button class="btn btn-sm btn-primary"  v-show="buy" v-on:click.prevent="selected(row.item)" >Add To Cart</button>
        </template>
      </b-table>
      <div>
        <nuxt-link to="/products/create" class="btn btn-primary">Create a New Product</nuxt-link>
        <button class="btn btn-primary" v-show="!buy" v-on:click.prevent="start">Buy</button>
        <button class="btn btn-primary" v-show="buy" v-on:click.prevent="end">End Shopping</button>
      </div>
      &nbsp;
      <div v-if="this.payment.product.name">
        <h4>Create New Payment</h4>
        <b-form>
          <b-form-group label="Associate:">
            <b-input name="name" v-model.trim="payment.associate.name" disabled />
          </b-form-group>
          <b-form-group label="Product:">
            <b-input name="product" v-model.trim="payment.product.name" disabled />
          </b-form-group>
          <b-form-group label="Value:">
            <b-input name="value" v-model.trim="payment.product.value*payment.quantity" disabled />
          </b-form-group>
          <b-form-group label="Total:">
            <b-input name="purchase" v-model.trim="purchase.total + payment.product.value*payment.quantity" disabled />
          </b-form-group>
          <b-form-group label="Quantity:">
            <b-input name="quantity" v-model.trim="payment.quantity" type="number" step="1" required />
          </b-form-group>
          <b-form-group label="State:">
            <input type="radio" id="paid" value="0" v-model="payment.state">
            <label for="paid">Pay</label>
            <br>
            <input type="radio" id="not-paid" value="1" v-model="payment.state">
            <label for="not-paid">Don't pay</label>
            <br>
            <input type="radio" id="partial" value="2" v-model="payment.state">
            <label for="partial">Pay partially</label>
          </b-form-group>
        </b-form>
        <div>
          <button class="btn btn-sm btn-primary" @click.prevent="addToCart">Add To Cart</button>
          <button class="btn btn-sm btn-primary" @click.prevent="clear">Cancel</button>
        </div>
      </div>
    </b-container>
  </div>
</template>

<script>
export default {
  data() {
    return {
      buy: false,
      products: [],
      purchase: {
          associate: {
              name: null
          },
          date: null,
          total: 0,
          payments: []
      },
      payment: {
          id: null,
          associate: {
              name: null
          },
          paid: 0,
          quantity: 0,
          launchInMilliseconds: null,
          state: 0,
          product: {
              name: null,
              value: 0
          }
      },
      fields: [
        {
          key: "id",
          sortable: true
        },
        {
          key: "name",
          sortable: true
        },
        {
          key: "category.name",
          sortable: true,
          label: "Category"
        },
        {
          key: "value",
          sortable: true
        },
        {
          key: "actions",
          sortable: true
        }
      ],
      buyFields: [
        {
          key: "name",
          sortable: true
        }
      ]
    };
  },
  created() {
      this.$axios.$get("/api/products").then(products => {
          this.products = products;
      });
  },
  methods: {
    selected(product) {
      this.payment.associate = this.purchase.associate;
      this.payment.product = product;
    },
    clear() {
        this.buy = false;
        this.purchase = {
            associate: {
                name: null
            },
            date: null,
            total: 0,
            payments: []
        };
        this.payment = {
            id: null,
            associate: {
                name: null
            },
            quantity: 0,
            state: null,
            product: {
                name: null,
                value: 0
            }
        };
    },
    addToCart() {
        this.payment.launchInMilliseconds = new Date().getTime();
        if (this.payment.state == 0)
            this.payment.paid = this.payment.product.value*this.payment.quantity;
        this.purchase.payments.push(JSON.parse(JSON.stringify(this.payment)));
        this.purchase.total += this.payment.product.value*this.payment.quantity;
        this.payment = {
            id: null,
            associate: {
                name: null
            },
            quantity: 0,
            state: null,
            product: {
                name: null,
                value: 0
            }
        };
    },
    deleteProduct: function(id) {
      this.$axios.$delete("/api/products/" + id)
      .then(() => {
        this.showSuccess = true;
        this.successMessage = "Product Deleted";
        this.$router.go();
      });
    },
    start: function() {
      this.buy = true;
      this.$axios.$get(`/api/users/${this.$store.state.auth.user.sub}`)
      .then(response => {
          this.purchase.associate = response;
      })
    },
    end: function() {
      this.purchase.date = new Date().getTime();
      this.$axios.$post(`/api/users/${this.$store.state.auth.user.sub}/purchase`,
          this.purchase
        )
        .then(response => {
          this.$toast.success("Purchase Complete with Success!");
          console.log(response);
          this.clear()
        })
        .catch(error => {
          this.$toast.error("Error on create Purchase");
        });
    }
  },
};
</script>
