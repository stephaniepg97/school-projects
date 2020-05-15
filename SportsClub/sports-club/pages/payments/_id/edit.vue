<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
    </div>
    <h4>Edit Payment</h4>
    <b-form @submit.prevent="save">
      <b-form-group label="Id:">
        <b-input name="id" v-model.trim="payment.id" required />
      </b-form-group>
      <b-form-group label="Associate:">
      <b-select
        v-model="payment.associate.username"
        :options="this.users"
        required
        value-field="username"
        text-field="username"
      >
        <template v-slot:first>
          <option :value="null" disabled>-- Please select the Associate --</option>
        </template>
      </b-select>
      </b-form-group>
       <b-form-group label="Product:">
      <b-select
        v-model="payment.product.id"
        :options="this.products"
        required
        value-field="id"
        text-field="id"
      >
        <template v-slot:first>
          <option :value="null" disabled>-- Please select the Product --</option>
        </template>
      </b-select>
      </b-form-group>
      <b-form-group label="Quantity:">
              <b-input name="quantity" v-model.trim="payment.quantity" required />
            </b-form-group>
      <b-form-group label="State:">
              <b-input name="state" v-model.trim="payment.state" required />
            </b-form-group>
    </b-form>
    <button @click.prevent="save">Save</button>
    <button @click.prevent="cancel">Cancel</button>
  </div>
</template>

<script>
    export default {
        data() {
            return {
                payment: {
                    id: null,
                    associate: {username : null},
                    quantity: null,
                    state: null,
                    product: {id : null}
                },
                users: [],
                products: [],
                fields: ['id', 'category', 'modality', 'name', 'value', 'stock', 'originalId']
            };
        },
        methods: {
            addProduct() {

            },

            created(){
              this.$axios.$get("/api/users/associates").then(users => {
              
              this.users = users;
            });
            },
            genericProduct(){
                 this.$axios.$get("/api/products").then(products => {
          
              this.products = products;
            });
            },

            remove(id) {

            },
            save() {
              this.$axios.put('api/payments', this.payment)
              .then(response => {
                  console.log(response.data);
              })
              .catch(error => {
                  console.log(error);
              });
            },
            cancel() {
                this.$router.push("/payments");
            }
        }
    };
</script>
