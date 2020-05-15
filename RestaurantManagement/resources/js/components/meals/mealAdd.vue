<template>
    <div v-if="($store.state.user != null) && ($store.state.user.id == id)">
        <add-edit :meal="meal" @meal-validated="add"></add-edit>
    </div>
</template>

<script state="text/javascript">

    import AddEdit from './add-edit.vue';

	export default {
        data: function() { 
            return {
                meal: {
                    table_number: "-1", 
                    responsible_waiter_id: this.id
                }
            }
        },
        props: {
            id: {
                default: "-1"
            } 
        },
        components: {
            'add-edit': AddEdit
        },
        methods: {
            add: function() {
                axios.post('api/meals', this.meal)
                    .then(response=>{
                        console.log(response);
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + 'New meal created with no.: ' + response.data.id;
                        this.$router.replace({ path: `/meals/${response.data.id}/orders/add` });
                    })
                    .catch(error=>{
                        console.log(error);
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
            },
        },
        mounted () {
            this.$root.message = null;
        }
	};
</script>

<style scoped>	

</style>