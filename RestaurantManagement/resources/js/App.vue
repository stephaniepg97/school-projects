<template>
  <div class="container" id="app">
    <div class="jumbotron">
      <h1>{{ $route.meta.title }}</h1>
        <div v-if="$store.state.user">
          <div>Currently on shift: {{onShift}}</div>
          <div v-if="$store.state.user.shift_active">
            <button class="ui basic button" @click="end">End Shift</button>
            <div>Started: {{ $store.state.user.last_shift_start }}</div>
            <b>Current time on shift:</b>
          </div>
          <div v-else="$store.state.user.shift_active">
            <button class="ui basic button" @click="start">Start Shift</button>
            <div>Ended: {{ $store.state.user.last_shift_end }}</div>
            <b>Current time since last shift:</b> 
          </div>
          <p id="timer"><span></span></p>
        </div>
        <nav class="navbar">
          <a v-if="$store.state.user">
            <a v-if="$store.state.user.email_verified_at">
              <router-link :to="{ path: `/users/${$store.state.user.id}/profile` }" class="navbar-item ui basic button">Profile</router-link>
              <router-link :to="{ path: `/users/${$store.state.user.id}/password` }" class="navbar-item ui basic button">Change Password</router-link> 
              <router-link to="/logout" class="navbar-item ui basic button" >Logout</router-link>
              <router-link :to="{ path: `/users/${$store.state.user.id}/meals` }" class="navbar-item ui basic button" v-if="$store.state.user.type == 'waiter'" >My Meals</router-link> 
              <router-link :to="{ path: `/users/${$store.state.user.id}/orders` }" class="navbar-item ui basic button" v-if="($store.state.user.type == 'cook') || ($store.state.user.type == 'waiter')" >My Orders</router-link> 
              <router-link to="/register" class="navbar-item ui basic button" v-if="$store.state.user.type == 'manager'">Register New Worker</router-link>
              <hr>
              <router-link to="/dashboard" class="navbar-item ui basic button" v-if="$store.state.user.type == 'manager'">Dashboard</router-link>
              <router-link to="/statistics" class="navbar-item ui basic button" v-if="$store.state.user.type == 'manager'">Statistics</router-link>
              <router-link to="/messages" class="navbar-item ui basic button"  v-if="!($store.state.user.type == 'manager')">Messages</router-link>
              <router-link to="/meals" class="navbar-item ui basic button">Meals</router-link>
              <router-link to="/users" class="navbar-item ui basic button">Users</router-link>  
              <router-link to="/tables" class="navbar-item ui basic button">Tables</router-link>  
              <router-link to="/invoices" class="navbar-item ui basic button">Invoices</router-link>  
              <router-link to="/orders" class="navbar-item ui basic button">Orders</router-link> 
            </a>
            <router-link :to="{ path: `/users/${$store.state.user.id}/verify` }" v-if="!$store.state.user.email_verified_at" class="navbar-item ui basic button">Verify Email</router-link> 
          </a>
          <a v-else="$store.state.user">
            <router-link to="/login" class="navbar-item ui basic button">Login</router-link>
          </a>
          <router-link to="/items" class="navbar-item ui basic button">Items</router-link> 
        </nav>  
    </div>
    <success-message v-if="$root.success"></success-message>
    <error-message v-else="$root.success"></error-message>
    <router-view></router-view>
  </div> 
</template>


<script>
import Vue from 'vue'

import store from './stores/global-store';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import Vuelidate from 'vuelidate';
Vue.use(Vuelidate);

import Toasted from 'vue-toasted';
Vue.use(Toasted, {
  position: 'bottom-center',
  duration: 5000,
  type: 'info',
});

import Moment from 'moment';

const items = Vue.component('items', require('./components/items/items.vue'));
const users = Vue.component('users', require('./components/users/users.vue'));
const tables = Vue.component('tables', require('./components/tables/tables.vue'));
const meals = Vue.component('meals', require('./components/meals/meals.vue'));
const invoices = Vue.component('invoices', require('./components/invoices/invoices.vue'));
const dashboard = Vue.component('dashboard', require('./components/dashboard/dashboard.vue'));
const orders = Vue.component('orders', require('./components/orders/orders.vue'));
const login = Vue.component('login', require('./components/auth/login.vue'));
const logout = Vue.component('logout', require('./components/auth/logout.vue'));
const verify = Vue.component('verify', require('./components/auth/verify.vue'));
const password = Vue.component('password', require('./components/auth/password.vue'));
const messages = Vue.component('messages', require('./components/messages/chat.vue'));

//MESSAGES
import SuccessMessage from './components/messages/success.vue';
import ErrorMessage from './components/messages/error.vue';
import Notification from './components/messages/notification.vue';

//USERS
import UserDetails from './components/users/userDetails.vue';
import UserEdit from './components/users/userEdit.vue';
import UserAdd from './components/users/userAdd.vue';

//TABLES
import TableAdd from './components/tables/tableAdd.vue';

//ORDERS
import OrderDetails from './components/orders/orderDetails.vue';

//MEALS
import MealDetails from './components/meals/mealDetails.vue';
import MealAdd from './components/meals/mealAdd.vue';
import MealEdit from './components/meals/mealEdit.vue';

//INVOICES
import InvoiceDetails from './components/invoices/invoiceDetails.vue';

//ITEMS
import ItemDetails from './components/items/itemDetails.vue';
import ItemAdd from './components/items/itemAdd.vue';
import ItemEdit from './components/items/itemEdit.vue';

//GRAPHIC
import Graphic from './components/graphic/graphic.vue';

//STATISTICS
import Statistics from './components/statistics/statistics.vue';

const routes = [
  { path: '/orders/add', component: items, meta: {title: 'Select Items'} },
  { path: '/meals/add', component: MealAdd, meta: {title: 'New Meal'} },


  { path: '/', redirect: '/items' },
  { path: '/login', component: login, name: 'login', meta: {title: 'Login'} },
  { path: '/logout', component: logout, name: 'logout', meta: {title: 'Logout'} },
  { path: '/register', component: UserAdd, name: 'register', meta: {title: 'Register'} },
  { path: '/items', component: items, name: 'items', meta: {title: 'Items'} },
  { path: '/items/add', component: ItemAdd, meta: {title: 'New Item'} },
  { path: '/tables', component: tables, meta: {title: 'Tables'} },
  { path: '/tables/add', component: TableAdd, meta: {title: 'New Table'} },
  { path: '/meals', component: meals, meta: {title: 'Meals'} },
  { path: '/invoices', component: invoices, meta: {title: 'Invoices'} },
  { path: '/dashboard', component: dashboard, meta: {title: 'Dashboard'} },
  { path: '/orders', component: orders, meta: {title: 'Orders'} },
  { path: '/users', component: users, meta: {title: 'Workers'} },
  { path: '/messages', component: messages, meta: {title: 'Chat Group'} },
  { path: '/statistics', component: Statistics, meta: {title: 'Statistics'} },
  { path: '/users/:id/verify', component: verify, name: 'verify', props:true, meta: {title: 'Email Verification'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Email Verification for Worker no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/users/:id/password', component: password, name: 'password', props:true, meta: {title: 'Update Password'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Update Password for Worker no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/users/:id/orders', component: orders, props:true, meta: {title: 'Orders from Cook/Waiter'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Orders from Cook/Waiter no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/users/:id/tables', component: tables, props:true, meta: {title: 'Tables'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Tables (Waiter no.: '  + to.params.id + ')';        
        next();
      },
  },
  { path: '/users/:id/meals/add', component: MealAdd, props:true, meta: {title: 'New Meal'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'New Meal (Waiter no.: '  + to.params.id + ')';        
        next();
      },
  },
  { path: '/users/:id/meals', component: meals, props:true, meta: {title: 'Meals from Waiter'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Meals from Waiter no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/users/:id/profile', component: UserDetails, name: 'profile', props:true, meta: {title: 'Profile'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Worker no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/users/:id/edit', component: UserEdit, props:true, meta: {title: 'Edit Worker'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Edit Worker no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/meals/:id/details', component: MealDetails, props:true, meta: {title: 'Meal Details'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Meal no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/meals/:id/edit', component: MealEdit, props:true, meta: {title: 'Meal Details'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Edit Meal no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/meals/:id/orders/add', component: items, props:true, meta: {title: 'Select Items'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Select Items => Meal no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/meals/:id/orders', component: orders, props:true, meta: {title: 'Orders from Meal'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Orders => Meal no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/invoices/:id/details', component: InvoiceDetails, props:true, meta: {title: 'Invoice Details'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Invoice no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/:type/orders/averages', component: Graphic , props:true, meta: {title: 'Orders\' Averages'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Orders\' Averages from all of ' + to.params.type;        
        next();
      },
  },
  { path: '/:type/:id/orders/averages', component: Graphic , props:true, meta: {title: 'Orders\' Averages'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Orders\' Averages from one of ' + to.params.type + ' (no.: '  + to.params.id + ')';        
        next();
      },
  },
  { path: '/items/:id/edit', component: ItemEdit, props:true, meta: {title: 'Item Edit'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Item no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/items/:id/details', component: ItemDetails, props:true, meta: {title: 'Item Details'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Item no.: '  + to.params.id;        
        next();
      },
  },
  { path: '/orders/:id/details', component: OrderDetails, props:true, meta: {title: 'Order Details'},
    beforeEnter: (to, from, next) => {
        to.meta.title = 'Order no.: '  + to.params.id;        
        next();
      },
  }
];

const router = new VueRouter({
    routes:routes
});

export default {
  name: 'app',
  store,
  router,
  data: function(){
    return { 
      time: null 
    }
  },
  components: {
    'success-message': SuccessMessage,
    'error-message': ErrorMessage
  },
  methods: {
    timer: function() {
      if (!this.$store.state.user) {
        return;
      }
      return setInterval(() => {  
        var now = Moment(new Date()).format("YYYY/MM/DD HH:mm:ss");
        var then = null;
        if (this.$store.state.user.shift_active) {
          then = this.$store.state.user.last_shift_start;
        } 
        else {
          then = this.$store.state.user.last_shift_end;
        }
        var ms = Moment(now,"YYYY/MM/DD HH:mm:ss").diff(Moment(then,"YYYY/MM/DD HH:mm:ss"));
        var d = Moment.duration(ms);
        var s = Math.floor(d.asHours()) + Moment.utc(ms).format(":mm:ss");
        $("#timer span").text(s);
      }, 1000);
    },
    start: function(){
          axios.patch('api/users/' + this.$store.state.user.id + '/shift-on')
              .then(response => {
                clearInterval(this.time);
                this.time = this.timer();   
                this.$root.success = true;
                this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
              }).catch(error => {
                this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
              });
      },
      end: function(){
          axios.patch('api/users/' + this.$store.state.user.id + '/shift-off')
            .then(response => {
              clearInterval(this.time);
              this.time = this.timer(); 
              this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
              }).catch(error => {
                this.$root.success = false;
                        this.$root.message = error.response.data.status + "," + error.response.data.statusText + ":" + error.response.data.message;
              });
      }
  },
  computed: {
      onShift: function() {
        return (this.$store.state.user.shift_active) ? 'Yes' : 'No';
      }
  },
  created() {
      this.$store.commit('loadTokenAndUserFromSession');
      console.log(this.$store.state.user);
      this.time = this.timer();   
  }
};

/*router.beforeEach((to, from, next) => {
    if (to.name != 'login') {
      if (to.name != 'items') {
        if (!store.state.user) {
            next("/login");
            return;
        }
      }
    }
    next();
});*/
</script>


