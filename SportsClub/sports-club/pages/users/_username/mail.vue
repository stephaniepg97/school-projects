<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://127.0.0.1:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/modalities" class>Modalities</nuxt-link>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <div class="search-container"></div>
    </div>

    <b-form @send="onSend" @reset="onReset">
      <b-form-group label="To:">
        <b-input name="to" v-model.trim="email.user.email" disabled />
      </b-form-group>
      <b-form-group label="Subject:" description="Enter a subject">
        <b-input name="to" v-model.trim="email.subject" required />
      </b-form-group>
      <b-form-group label="Message" description="Enter your message">
        <b-textarea
          rows="5"
          cols="50"
          name="text"
          type="text"
          placeholder="Your message"
          v-model="email.message"
          required
        />
      </b-form-group>
      <b-button @click.prevent="onSend" class="btn-success">Send</b-button>
      <b-button @click.prevent="onReset" class="btn-warning">Reset</b-button>
    </b-form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      email: {
          user: {
              email: null
          },
          subject: null,
          message: null
      }
    };
  },
  created() {
      this.$axios.$get(`/api/users/${this.$route.params.username}`)
          .then((user) => {
              console.log(user)
              this.email.user = user
          })
  },
  methods: {
    onSend() {
        this.$axios.$post(`/api/users/${this.$route.params.username}/email/send`, this.email)
            .then(msg => {
                this.$toast.success(msg)
            })
            .catch(error => {
                this.$toast.error('error sending the e-mail')
            })
    },
    onReset() {
      this.email.subject = null;
      this.email.message = null;
    }
  }
};
</script>
