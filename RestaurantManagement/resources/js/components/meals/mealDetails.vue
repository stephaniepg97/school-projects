<template>
<div v-if="meal != null">
    <div class="form-group">
    	<label for="inputState">State</label>
    	<input
        	type="text" class="form-control" readonly="readonly"
        	name="state" id="inputState" v-model="meal.state" />
    </div>
    <div class="form-group">
        <label for="inputTable">Table</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="table" id="inputTable" v-model="meal.table.table_number"/>
    </div>
    <div class="form-group">
        <label for="inputStartDate">Start date</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="startDate" id="inputStartDate" v-model="meal.start"/>
    </div>
    <div class="form-group">
        <label for="inputEndDate">End date</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="endDate" id="inputEndDate" v-model="meal.end"/>
    </div>
    <div class="form-group"  v-if="meal.waiter">
        <label for="txtResponsibleWaiter">Responsible Waiter</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="responsibleWaiter" id="txtResponsibleWaiter" v-model="meal.waiter.name"/>

        <router-link :to="{ path: `/users/${meal.waiter.id}/profile` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
    </div>
    <div class="form-group"  v-else="meal.waiter">
        <label for="txtResponsibleWaiterDeleted">Responsible Waiter</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="txtResponsibleWaiterDeleted" id="txtResponsibleWaiterDeleted" value="USER DELETED"/>
    </div>
    <div class="form-group">
        <label for="txtTotalPrice">Total Price Preview</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="totalPrice" id="txtTotalPrice" v-model="meal.total_price_preview"/>
    </div>
    <div class="form-group">
        <router-link :to="{ path: `/meals/${this.id}/edit` }" replace class="ui basic button"><i class="edit icon"></i></router-link>
        <a class="btn btn-light" v-on:click.prevent="cancel">Return</a>
    </div>
</div>
</template>

<script state="text/javascript">

    export default {
        data: function() { 
            return {
                meal: null,
            }
        },
        props: {
            id: {
                required: true
            } 
        },
        methods: {
            cancel: function() {
                this.$router.go(-1);
            }
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