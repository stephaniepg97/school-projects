<template>
<div v-if="order != null">
    <div class="form-group">
        <label for="txtState">State</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="state" id="txtState" v-model="order.state"/>
    </div>
    <div class="form-group" v-if="order.item">
        <label for="txtItem">Item</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="item" id="txtItem" v-model="order.item.name"/>
    </div>
    <div class="form-group" v-if="order.item">
        <router-link :to="{ path: `/items/${order.item.id}/details` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
    </div>
    <div class="form-group"  v-else="order.item">
        <label>Item: ITEM DELETED</label>
    </div>
    <div class="form-group">
        <label>Meal</label>
        <router-link :to="{ path: `/meals/${order.meal.id}/details` }" replace class="ui basic button">Meal</router-link>
    </div>
    <div class="form-group"  v-if="order.waiter">
        <label for="txtResponsibleWaiter">Responsible Waiter</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="responsibleWaiter" id="txtResponsibleWaiter" v-model="order.waiter.name"/>
    </div>
    <div class="form-group"  v-if="order.waiter">
        <router-link :to="{ path: `/users/${order.waiter.id}/profile` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
    </div>
    <div class="form-group"  v-else="order.waiter">
        <label>Responsible Waiter: USER DELETED</label>
    </div>
    <div class="form-group"  v-if="order.cook">
        <label for="txtResponsibleCook">Responsible Cook</label>
        <input
            type="text" class="form-control" readonly="readonly"
            name="responsibleCook" id="txtResponsibleCook" v-model="order.cook.name"/>

        <router-link :to="{ path: `/users/${order.cook.id}/profile` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
    </div>
    <div class="form-group"  v-else="order.cook">
        <label>Responsible Cook: NOT DEFINED YET</label>
    </div>
    <div class="form-group">
        <a class="btn btn-light" v-on:click.prevent="cancel">Return</a>
    </div>
</div>
</template>

<script state="text/javascript">

    export default {
        data: function() { 
            return {
                order: null
            }
        },
        props: {
            id: {
                required: true,
                default: "-1"
            } 
        },
        methods: {
            cancel: function() {
                this.$router.go(-1);
            }
        },
        mounted () {
            this.$root.message = null;
            axios.get('api/orders/'+this.id)
                .then(response=>{
                    this.order = response.data;
                })
                .catch(error=>{
                    this.$root.success = false;
                    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                });
        }
	};
</script>

<style scoped>	

</style>