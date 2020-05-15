<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/modalities" class>Modalities</nuxt-link>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
    </div>
    <b-container>
      <div>
        <h1>Create a New User</h1>

        <form @submit.prevent="create" :disabled="!isFormValid">
          <b-form-group
            id="username"
            description="The username is required"
            label="Enter your username"
            label-for="username"
            :invalid-feedback="invalidUsernameFeedback"
            :state="isUsernameValid"
          >
            <b-input id="username" v-model.trim="username" :state="isUsernameValid" trim></b-input>
          </b-form-group>

          <b-input
            v-model="password"
            type="password"
            :state="isPasswordValid"
            required
            placeholder="Enter your password"
          />

          <b-input v-model.trim="name" required :state="isNameValid" placeholder="Enter your name" />

          <b-input
            ref="email"
            v-model.trim="email"
            type="email"
            :state="isEmailValid"
            required
            placeholder="Enter your e-mail"
          />

          <b-select v-model="type" :options="this.types" required>
            <template v-slot:first>
              <option :value="null" disabled>-- Please select the Type --</option>
            </template>
          </b-select>

          <p class="text-danger" v-show="errorMsg">{{ errorMsg }}</p>

          <nuxt-link class="btn btn-dark" to="/users">Return</nuxt-link>

          <button class="btn btn-danger" type="reset" @click="reset">Reset</button>
          <button class="btn btn-success" @click.prevent="create" :disabled="!isFormValid">Create</button>
        </form>
      </div>
    </b-container>
  </div>
</template>

<script>
export default {
  data() {
    return {
      username: null,
      password: null,
      name: null,
      email: null,
      type: null,
      types: ["Administrator", "Associate", "Coach"],
      errorMsg: false
    };
  },

  computed: {
    invalidUsernameFeedback() {
      if (!this.username) {
        return null;
      }

      let usernameLen = this.username.length;

      if (usernameLen < 3 || usernameLen > 15) {
        return "The username must be between [3, 15] characters.";
      }

      return "";
    },

    isUsernameValid() {
      if (this.invalidUsernameFeedback === null) {
        return null;
      }

      return this.invalidUsernameFeedback === "";
    },

    isPasswordValid() {
      if (!this.password) {
        return null;
      }

      let passwordLen = this.password.length;

      if (passwordLen < 3 || passwordLen > 255) {
        return false;
      }

      return true;
    },

    isNameValid() {
      if (!this.name) {
        return null;
      }

      let nameLen = this.name.length;

      if (nameLen < 3 || nameLen > 25) {
        return false;
      }

      return true;
    },

    isEmailValid() {
      if (!this.email) {
        return null;
      }

      return this.$refs.email.checkValidity();
    },

    isFormValid() {
      if (!this.isUsernameValid) {
        return false;
      }

      if (!this.isPasswordValid) {
        return false;
      }

      if (!this.isNameValid) {
        return false;
      }

      if (!this.isEmailValid) {
        return false;
      }

      return true;
    }
  },

  methods: {
    reset() {
      this.errorMsg = false;
    },

    create() {
      console.log(this.type);
      this.$axios
        .$post("/api/users/create/" + this.type, {
          username: this.username,
          password: this.password,
          name: this.name,
          email: this.email,
        })
        .then(() => {
          this.$router.push("/users");
        })
        .catch(error => {
          this.errorMsg = error.response.data;
        });
    }
  }
};
</script>
