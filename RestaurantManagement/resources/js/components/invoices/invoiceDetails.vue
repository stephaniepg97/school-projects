<template>
<div v-if="invoice != null">
	<div class="form-group">
    	<label for="txtState">State</label>
    	<input
        	type="text" class="form-control" readonly="readonly"
        	name="state" id="txtState" v-model="invoice.state" />
    </div>
    <div class="form-group">
        <label for="inputNIF">NIF</label>
        <input
            type="text" class="form-control"
            name="nif" id="inputNIF" v-model="invoice.nif"/>
    </div>
    <div class="form-group">
        <label for="inputCustomerName">Customer's Name</label>
        <input
            type="text" class="form-control"
            name="invoice" id="inputCustomerName" v-model="invoice.customer_name"/>
    </div>
    <div class="form-group">
        <label for="txtTable">Table</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="table" id="txtTable" v-model="invoice.meal.table.table_number"/>
    </div>
    <div class="form-group">
        <label for="txtResponsibleWaiter">Responsible Waiter</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="responsibleWaiter" id="txtResponsibleWaiter" v-model="invoice.meal.waiter.name"/>
        <router-link :to="{ path: `/profile/${meal.waiter.id}` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
    </div>
    <div class="form-group">
        <label for="txtDate">Date</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="date" id="txtDate" v-bind:value="invoice.date"/>
    </div>
    <div class="form-group">
        <label for="txtTotalPrice">Total Price</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="totalPrice" id="txtTotalPrice" v-bind:value="invoice.total_price"/>
    </div>
    <div class="form-group">
        <router-link :to="{ path: `/meals/${invoice.meal.id}/details` }" replace class="ui basic button">See Meal</router-link>
    </div>
    <div class="form-group">
        <a class="btn btn-primary" v-on:click.prevent="pay">Do Payment</a>
        <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
    </div>
</div>
</template>

<script state="text/javascript">

    import Items from '../items/items.vue';

    export default {
        data: function() { 
            return {
                invoice: null,
            }
        },
        props: {
            id: {
                required: true,
                default: "-1"
            } 
        },
        components: {
            'items': Items
        },
        methods: {
            pay: function() {
                axios.patch('api/users/'+this.id+'/do-payment')
                  .then(response => {
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
                    })
                  .catch(error => {
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                   });
            },
            cancel: function() {
                this.$router.go(-1);
            }
        },
        mounted () {
            this.$root.message = null;
            axios.get('api/invoices/'+this.id)
                .then(response=>{
                    this.invoice = response.data.data;
                })
                .catch(error => {
                    this.$root.success = false;
                    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                });
        }
    };
</script>

<style scoped>  

</style>