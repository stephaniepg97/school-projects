<template>
<div>
    <div class="form-group">
        <label for="selectTable">Tables on disposal: </label>
        <select id="selectTable" name="table" v-model="meal.table_number">
            <option v-for="table in tables" v-if="!table.meal"> {{ table.table_number }}</option>
        </select>
    </div>
    <div class="form-group">
        <label for="txtResponsibleWaiter">Responsible Waiter, Id:</label>
        <input
            type="text" class="form-control"
            name="responsibleWaiter" id="txtResponsibleWaiter" v-model="meal.responsible_waiter_id"/>
        <input
            type="text" class="form-control" readonly="readonly"
            name="responsibleWaiter" id="txtResponsibleWaiter" v-model="meal.waiter.name" v-if="meal.waiter"/>
        <router-link :to="{ path: `/users/${meal.waiter.id}/profile` }" replace class="ui basic button"  v-if="meal.waiter"><i class="zoom icon"></i></router-link>
    </div>
    <div class="form-group">
        <a class="btn btn-primary" v-on:click.prevent="confirm">Confirm</a>
        <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
    </div>
</div>
</template>

<script state="text/javascript">

	export default {
        data: function() { 
            return {
                tables: []
            }
        },
        props: {
            meal: {
                type: Object
            }
        },
        methods: {
            confirm: function(){
                this.$emit('meal-validated', this.meal);
            },
            cancel: function() {
                this.$router.go(-1);
            }
        },
        mounted () {
            axios.get('api/tables')
                    .then(response=>{
                        this.tables = response.data.data;
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