<template>
	<div>
	   <add-edit :user="user" @user-validated="add"></add-edit>
	</div>
</template>

<script type="text/javascript">

import AddEdit from './add-edit.vue';
	
	export default {
        data: function() { 
            return {
                user: {
        			name: "", 
        			username: "",
        			email: "", 
        			type: "",
        			photo_url: "",
        			url: null
                },
            }
        },
        components: {
            'add-edit': AddEdit
        },
        methods: {
	        add: function() {
                if (this.$store.state.image) {
                    this.user.image = this.$store.state.image;
                }
                axios.post('api/register', this.user)
                .then(response=>{
                    console.log(response);
                    if (this.$store.state.image) {
                        this.$store.commit('clearImage');
                    }
                    this.$root.success = true;
                    this.$root.message = response.status + ", " + response.statusText + ": New User created with no.: " + response.data.id;
                    this.$router.replace({ path: `/users/${response.data.id}/profile` });
                })
                .catch(error => {
                    console.log(error);
                    this.$root.success = false;
                    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                });
	        }
		},
        mounted () {
            this.$root.message = null;
        }
	};
</script>
