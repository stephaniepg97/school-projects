<template>
    <div class="form-group">
        <a class="btn btn-primary" v-on:click.prevent="logout">Logout</a>
        <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
    </div>
</template>

<script type="text/javascript">    
    export default {
        methods: {
            logout() {
                axios.post('api/logout')
                    .then(response => {
                        this.$store.commit('clearUserAndToken');
                        this.$router.replace({ path: "/items" });
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + "User has logged out correctly";
                    })
                    .catch(error => {
                        this.$store.commit('clearUserAndToken');
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + "Logout incorrect. But local credentials were discarded. " + error.response.data.message;
                    });        
            },
            cancel: function(){
                this.$router.go(-1);
            },
        },
        mounted() {
            this.$root.message = null;
        }
    };
</script>