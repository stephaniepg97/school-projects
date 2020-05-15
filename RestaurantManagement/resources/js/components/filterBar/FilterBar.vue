<template>
<div>
  <div class="col-md-6" v-if="search == 'meals' || search == 'invoices' || search == 'orders'">
    <div class="form-group">
      <label for="waiter">Responsible Waiter:</label>
      <input class="form-input" type="text" id="waiter" v-model="filter.waiter" />
    </div>
    <div class="form-group">
      <label for="date">Date:</label>
      <input id="date" type="date" v-model="filter.date" />
    </div>
    <div class="form-group">
        <label for="state">State:</label>
        <select id="state" v-model="filter.state">
            <option disabled selected value> -- select an option -- </option>
            <option value="active" v-if="search == 'meals'">Active</option>
            <option value="terminated" v-if="search == 'meals'">Terminated</option>
            <option value="pending" v-if="search == 'invoices' || search == 'orders'">Pending</option>
            <option value="confirmed" v-if="search == 'orders'">Confirmed</option>
            <option value="in preparation" v-if="search == 'orders'">In preparation</option>
            <option value="prepared" v-if="search == 'orders'">Prepared</option>
            <option value="delivered" v-if="search == 'orders'">Delivered</option>
            <option value="not delivered" v-if="search == 'orders'">Not Delivered</option>
            <option value="not paid" v-if="search == 'invoices' || search == 'meals'">Not paid</option>
            <option value="paid" v-if="search == 'invoices' || search == 'meals'">Paid</option>
        </select>
    </div>
  </div>
  <div class="col-md-6" v-if="search == 'orders'">
    <div class="form-group">
        <label for="state">Seasoning:</label>
        <select id="state" v-model="filter.seasoning">
            <option disabled selected value> -- select an option -- </option>
            <option value="true">With special</option>
            <option value="false">With no special</option>
        </select>
    </div>
    <div class="form-group">
        <label for="cook">Responsible Cook:</label>
        <input class="form-input" type="text" id="cook" v-model="filter.cook" />
    </div>
  </div>
  <div class="col-md-6" v-if="search == 'users' || search == 'items'">
    <div class="form-group">
      <label for="name" >Name:</label>
      <input id="name" class="form-input" type="text" v-model="filter.name" />
    </div>
    <div class="form-group">
        <label for="type">Type: </label>
        <select id="type" v-model="filter.type" >
            <option value="manager" v-if="search == 'users'">Manager</option>
            <option value="cook" v-if="search == 'users'">Cook</option>
            <option value="cashier" v-if="search == 'users'">Cashier</option>
            <option value="waiter" v-if="search == 'users'">Waiter</option>
            <option value="dish" v-if="search == 'items'">Dish</option>
            <option value="drink" v-if="search == 'items'">Drink</option>
        </select>
    </div>
  </div>
  <div class="col-md-6" v-if="search == 'users_stats'">
    <div class="form-group">
      <label for="name" >Name:</label>
      <input id="name" class="form-input" type="text" v-model="filter.name" />
    </div>
  </div>
  <div class="form-group">
    <button class="ui basic button" @click.prevent="doFilter">Go</button>
    <button class="ui light button" @click.prevent="resetFilter">Reset</button>
  </div>
</div>
</template>

<script>
  export default {
    data: function() { 
      return {
        filter: {
          waiter: null,
          date: null,
          state: null,
          name: null,
          type: null,
          seasoning: null,
          cook: null 
        }
      }
    },
    props: ['search'],
    methods: {
      doFilter () {
        this.$emit('filter-set', this.filter);
      },
      resetFilter () {
        this.filter.waiter = "";
        this.$emit('filter-reset');
      }
    },
  };
</script>
<style>
.form-group {
    margin-top: 2rem;
    margin-bottom: 2rem;
    padding: 5px;
}
button.ui.button {
    padding: 8px 8px;
    margin-top: 5px;
}
</style>
