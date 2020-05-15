<template>
<div>
	<add-edit :user="user" @user-validated="edit"></add-edit>
</div>
</template>

<script type="text/javascript">

	import AddEdit from './add-edit.vue';

	export default {
        data: function() { 
            return {
                user: null,
            }
        },
        props: {
            id: {
                required: true,
                default: "-1"
            } 
        },
        components: {
            'add-edit': AddEdit
        },
        methods: {
	    	edit: function() {
                if (!this.user.url) {
                   this.user.image = this.$store.state.image;
                }
	        	axios.put('api/users/'+this.id, this.user)
                    .then(response=>{
                        console.log(response);
                        if (!this.user.url) {
                            this.$store.commit('clearImage');
                        }
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": User no.: " + response.data.id + " updated successfully.";
                        this.$router.replace({ path: `/users/${response.data.id}/profile` });
                    })
                    .catch(error => {
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
            }
		},
		mounted () {
            this.$root.message = null;
            axios.get('api/users/'+this.id)
            .then(response=>{
                this.user = response.data;
            })
            .catch(error=>{
                this.$root.success = false;
                this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
            });
        },
	};
</script>
