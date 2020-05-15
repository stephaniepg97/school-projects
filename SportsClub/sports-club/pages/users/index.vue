<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
      <div class="search-container">
        <form action="/action_page.php">
          <input type="text" placeholder="Search.." name="search" />
          <button @click.prevent>Go</button>
          <i class="fa fa-search"></i>
        </form>
      </div>
    </div>
    <b-container>
      <b-table striped over :items="users" :fields="fields">
        <template v-slot:cell(actions)="row">
          <div class="custom-actions">
            <nuxt-link class="btn btn-sm btn-primary" :to="`/users/${row.item.username}/`">Details</nuxt-link>
            <nuxt-link class="btn btn-sm btn-primary" :to="`/users/${row.item.username}/edit`">Edit</nuxt-link>
            <nuxt-link class="btn btn-sm btn-primary" :to="`/users/${row.item.username}/mail`">Send e-mail</nuxt-link>
            <button class="btn btn-sm btn-danger" @click.prevent="deleteItem(row.item.username)">Delete</button>
          </div>
        </template>
      </b-table>
      <div class="alert alert-success" v-if="showSuccess">
        <button type="button" class="close-btn" v-on:click="showSuccess=false">&times;</button>
        <strong>{{ successMessage }}</strong>
      </div>
      <nuxt-link to="/users/create" class="btn btn-primary">Create a New User</nuxt-link>
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
      users: [],
      showSuccess: false,
      successMessage: "",
      showFailure: false,
      failMessage: "",
      fields: ["username", "name", "email",  "actions"]
    };
  },
  methods: {
    deleteItem(username) {
      this.$axios.$delete(`/api/users/${username}`)
        .then(response => {
          console.log(response.data);
          this.showSuccess = true;
          this.successMessage = "User Deleted";
          this.$router.go();
        })
        .catch(error => {
          console.log(error.response);
          this.showFailure = true;
          this.failMessage = error.response;
        });
    }
  },
  created() {
    this.$axios.$get("/api/users").then(users => {
      this.users = users;
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
