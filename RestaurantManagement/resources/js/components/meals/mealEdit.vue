<template>
<div v-if="meal != null">
    <div class="form-group">
        <label>See orders from this meal:</label>
        <router-link :to="{ path: `/meals/${this.id}/orders` }" class="ui basic button">Orders</router-link>
    </div>
    <add-edit :meal="meal" @meal-validated="edit"></add-edit>
</div>
</template>

<script state="text/javascript">

    import AddEdit from './add-edit.vue';

	export default {
        data: function() { 
            return {
                meal: null
            }
        },
        props: {
            id: {
                required: true
            } 
        },
        components: {
            'add-edit': AddEdit
        },
        methods: {
            edit: function(){
                axios.patch('api/meals/'+this.id, this.meal)
                    .then(response=>{
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": Meal updated successfully.";
                        this.$router.replace({ path: `/meals/${this.id}/details` });
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
            axios.get('api/meals/'+this.id)
                    .then(response=>{
                        console.log(response);
                        this.meal = response.data;
                    })
                    .catch(error=>{
                        console.log(error);
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
        }
	};
</script>

<style scoped>	

</style>