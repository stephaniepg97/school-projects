<template>
  <div class="card mt-3">
      <div class="card-body">
          <div class="card-title">
              <h3>Send message to manager: </h3>
              <hr>
          </div>
          <div class="card-body">
              <div class="messages" v-for="(msg, index) in messages" :key="index">
                  <p><span class="font-weight-bold">{{ msg.user }}: </span>{{ msg.message }}</p>
              </div>
          </div>
      </div>
      <div class="card-footer">
          <form @submit.prevent="sendMessage">
              <div class="gorm-group">
                  <label for="user">User:</label>
                  <input type="text" v-model="user" class="form-control">
              </div>
              <div class="gorm-group pb-3">
                  <label for="message">Message:</label>
                  <input type="text" v-model="message" class="form-control">
              </div>
              <button type="submit" class="btn btn-success">Send</button>
          </form>
      </div>
  </div>
</template>

<script>
export default {
    data() {
        return {
            user: "",
            message: "",
            messages: []
        }
    },
    methods: {
        sendMessage(e) {
            e.preventDefault();
            if (this.user.type != 'manager') {
              this.$root.success = false;
              this.$root.message = "Error: Receiver is not a manager. Impossible to send message."
            }
            this.message = "";
            this.user = "";
        }
    }
};
</script>

<style>
</style>