<template>
<div>

	<table v-show="days" class="table" v-for="(months, year) in values">
	    <thead>
	    	<th>Year</th>
	    	<th>{{year}}</th>
	    </thead>
	    <tbody>
	    	<td v-for="(days, month) in months">
	    		<table>
			    	<thead>
			    		<th>Day/Month</th>
			    		<td>{{month}}</td> 
			    	</thead>
			  		<tbody>
				    	<tr v-for="(value, day) in days" :key="day">
				    		<th>{{day}}</th>
				    		<td>{{value}}</td> 
				    	</tr>
				  	</tbody>
		  		</table>
		  	</td>
	  	</tbody>
	</table>

	<table v-show="!days" class="table" v-for="(months, year) in values">
	    <thead>
	    	<th>Month/Year</th>
	    	<th>{{year}}</th>
	    </thead>
	    <tbody>
	    	<tr v-for="(value, month) in months">
	    		<th>{{month}}</th>
	    		<td>{{value}}</td> 
	  		</tr>
	  	</tbody>
	</table>

	<h3>Performance</h3> 
	
	<b>Cooks and Waiters, by day: </b>
	<div class="form-group">
		<p><em>Show the average number of orders handled by day for each <b>cook</b>: </em></p>
		<button class="ui basic button" @click="users = 'cook'">Cooks</button>
	</div>
	<div class="form-group">
		<p><em>Show the average number of orders handled by day for each <b>waiter</b>: </em></p>
		<button class="ui basic button" @click="users = 'waiter'">Waiters</button>
	</div>

	<users v-if="users" @show-user-averages="averagesUsers" @hide-table="users = null" :type="users"></users>

	<b>Meals, per month: </b>
	<div class="form-group">
		<p><em>Show the average time it takes to handle each meal (time between the moment a meal iscreated until the meal is closed – “paid” or “not paid”) per month </em></p>
		<button class="ui basic button" @click="averagesMeals">Averages</button>
		<span>See results on table above.</span>
	</div>

	<div class="form-group">
		<p><em>Show the average time it takes to handle each meal (time between the moment a meal is created until the meal is closed – “paid” or “not paid”) per month: <b>ON GRAPHIC</b> </em></p>
		<router-link to="/meals/orders/averages" replace class="ui basic button">Graphic</router-link>
	</div>

	<b>Orders, per month: </b>
	<div class="form-group">	
		<p><em>Show  the average time it takes to handle each order (time between the moment an order is created until the order is closed – “delivered” or “not delivered”) organized by item of the restaurant’s menu</em></p>
		<button class="ui basic button" @click="items = true">Items</button>
	</div>

	<items v-if="items" @show-item-averages="averagesItems" @hide-table="items = false"></items>

	<h3>Totals</h3> 

	<b>Total number of Meals, per month: </b>
	<div class="form-group">
		<p><em>Show total number of meals handled per month </em></p>
		<button class="ui basic button" @click="totalsMeals">Totals</button>
		<span>See results on table above.</span>
	</div>
	
	<b>Total number of Orders, per month: </b>
	<div class="form-group">
		<p><em>Show total number of orders handled per month </em></p>
		<button class="ui basic button" @click="totalsOrders">Totals</button>
		<span>See results on table above.</span>
	</div>
</div>
</template>

<script type="text/javascript">

//<div class="form-group" v-if="$store.state.user">

//	    <div v-if="$store.state.user.type == 'manager'">
import Users from './users.vue';
import Items from './items.vue';
	
	export default {
		data: function() { 
            return {
                values: {},
                users: null,
                items: false,
                days: false
            }
        },
		methods: {
			totalsOrders: function() {
				this.days = false;
			    axios.get('api/orders/totals')
			        .then(response => {
			        	console.log(response.data);
			        	this.values = response.data;
			        }).catch(error => {
			        	console.log(error);
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
			        });
			},
			totalsMeals: function(){
				this.days = false;
			    axios.get('api/meals/totals')
			        .then(response => {
			        	console.log(response.data);
			        	this.values = response.data;
			        }).catch(error => {
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
			        });
			},
			averagesMeals: function(){
				this.days = false;
			    axios.get('api/meals/orders/averages')
			        .then(response => {
			        	console.log(response.data);
			        	this.values= response.data;
			        }).catch(error => {
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
			        });
			},
			averagesItems: function(itemId){
				this.days = false;
				axios.get(`api/items/${itemId}/orders/averages`)
			        .then(response => {
			        	console.log(response.data);
			        	this.values = response.data;
			        }).catch(error => {
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
			        });
			},
			averagesUsers: function(userId){
				this.days = true;
				axios.get(`api/users/${userId}/orders/averages`)
			        .then(response => {
			        	console.log(response.data);
			        	this.values = response.data;
			        	var values = Object.values(this.values);
			        	console.log(values);
			        	values = Object.values(values);
			        	console.log(Object.values(values));
			        }).catch(error => {
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
			        });
			    
			},
		},
		components: {
		    'users': Users,
		    'items': Items
		}
	};
</script>

<style>
.form-group {
    margin-top: 2rem;
    margin-bottom: 2rem;
    padding: 5px;
}
button.ui.button {
    padding: 8px 8px;
    margin-top: 3px;
    margin-bottom: 3px;
}
</style>
