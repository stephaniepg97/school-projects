<template>
<div>
	<div class="form-group" v-if="$store.state.user != null && $store.state.user.type == 'manager'">
		<router-link to="items/add" class="ui basic button">New Item</router-link>
	</div>
	<div class="form-group" v-if="id != -1">
		<div class="form-group">
	        <label>Seasoning</label>
	        <select name="type" v-model="order.seasoning" >
	            <option value="unsalty">Unsalty</option>
	            <option value="salty">Salty</option>
	            <option value="spicy">Spicy</option>
	            <option value="sweet">Sweet</option>
	        </select>
	    </div>

	    <div class="form-group">
			<p>Please check the following items to include on the current meal: </p>
			<button class="ui basic button" @click="createOrders">Confirm selected items</button>
		</div>
	</div>	
	<filter-bar :search="'items'" @filter-set="filter" @filter-reset="reset"></filter-bar>
    <vuetable ref="vuetable"
      :api-url="apiUrl"
      track-by="id"
      :fields="fields"
      pagination-path="meta"
      :css="css.table"
      :sort-order="sortOrder"
      @vuetable:pagination-data="onPaginationData"
      @vuetable:load-error="handleLoadError"
    >
    <template slot="actions" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="delete(props.rowData)"><i class="delete icon"></i></button>
    		<router-link :to="{ path: `/items/${props.rowData.id}/edit` }" replace class="ui basic button"><i class="edit icon"></i></router-link>
    		<router-link :to="{ path: `/items/${props.rowData.id}/details` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
  		</div>
	</template>
	<template slot="averages" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="graph(props.rowData)">Graph</button>
  		</div>
	</template>
	<template slot="details" slot-scope="props">
		<div class="custom-actions">
			<router-link :to="{ path: `/items/${props.rowData.id}/details` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
  		</div>
	</template>
	<template slot="photo" slot-scope="props">
		<div v-if="props.rowData.url">
			<a class="ui basic button" :href="props.rowData.url">
				<img :src="props.rowData.url" width="230px">
			</a>
		</div>
	</template>
    </vuetable>
    <div class="vuetable-pagination">
      <vuetable-pagination-info ref="paginationInfo" info-class="pagination-info"></vuetable-pagination-info>
      <vuetable-pagination ref="pagination" :css="css.pagination" @vuetable-pagination:change-page="onChangePage"></vuetable-pagination>				
	</div>					
</div>				
</template>

<script type="text/javascript">

	import moment from 'moment'
	import Vuetable from 'vuetable-2/src/components/Vuetable'
	import VuetablePagination from 'vuetable-2/src/components/VuetablePagination'
	import VuetablePaginationInfo from 'vuetable-2/src/components/VuetablePaginationInfo'
	import Vue from 'vue'
	import accounting from 'accounting'

	import VueEvents from 'vue-events'
	Vue.use(VueEvents)

	import FilterBar from '../filterBar/FilterBar.vue';
	Vue.component('filter-bar', FilterBar)

	export default {
		data: function(){
			return {
				apiUrl: "api/items?page=1",
				indexes: [],
				orders: [],
				item: null,
				order: {
                    item_id: -1,
                    meal_id: this.id,
                    seasoning: null
                },
		        fields: [
					{
						name: '__handle',
						titleClass: 'center aligned',
						dataClass: 'center aligned'
					},
					{
						name: '__checkbox',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: this.id != -1
					},
					{
						name: '__slot:photo',
						title: 'Photo',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						width: '250px'
					},
					{
						name: 'id',
						sortField: 'id',
					},
					{
						name: 'name',
						sortField: 'name',
					}, 
					{
						name: 'type',
						sortField: 'type',
					}, 
					{
						name: 'description',
						sortField: 'description',
					},
					{
						name: 'price',
						titleClass: 'text-center',
						dataClass: 'text-right',
						callback: 'formatNumber'
			        },
					{
						name: 'created_at.date',
						sortField: 'created_at',
						title: 'Created At',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						callback: 'formatDate|DD-MM-YYYY'
					},
					{
						name: 'updated_at.date',
						sortField: 'updated_at',
						title: 'Updated At',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						callback: 'formatDate|DD-MM-YYYY'
					},
					{
						name: '__slot:actions',
						title: 'Actions',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'manager') 
					},
					{
						name: '__slot:details',
						title: 'Details',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: this.id != -1
					},
					{
						name: '__slot:averages',
						title: 'Averages ',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'manager') 
					}
				],
				sortOrder: [
					{
					  field: 'id',
					  sortField: 'id',
					  direction: 'asc'
					}
				],
				css: {
					table: {
						tableClass: 'table table-bordered table-striped table-hover',
						ascendingIcon: 'glyphicon glyphicon-chevron-up',
						descendingIcon: 'glyphicon glyphicon-chevron-down'
					},
					pagination: {
						wrapperClass: 'pagination',
						activeClass: 'active',
						disabledClass: 'disabled',
						pageClass: 'page',
						linkClass: 'link',
						icons: {
							first: '',
							prev: '',
							next: '',
							last: '',
						},
					},
					icons: {
						first: 'glyphicon glyphicon-step-backward',
						prev: 'glyphicon glyphicon-chevron-left',
						next: 'glyphicon glyphicon-chevron-right',
						last: 'glyphicon glyphicon-step-forward',
					}
				}
			}
		},
		props: {
            id: {
            	default: "-1"
            } 
        },
		components: {
	    	Vuetable,
    		VuetablePagination,
    		VuetablePaginationInfo
	    },
	    methods: {
	    	filter: function(filter) {
				this.apiUrl = 'api/items?page=1';
	    		if (filter.type) {
	    			this.apiUrl += `&type=${filter.type}`;
	    		}

	    		if (filter.name) {
	    			this.apiUrl += `&name=${filter.name}`;
	    		}
				Vue.nextTick( () => this.$refs.vuetable.refresh());
			},
			reset: function() {
				this.apiUrl = "api/items?page=1";
				Vue.nextTick( () => this.$refs.vuetable.refresh());
			},
			graph: function(item) {
				this.item = item;
	    		this.$router.replace({ path: `/items/${this.item.id}/orders/averages?graph=true` });
	    	},
	        delete: function(item){
	        	this.item = item;
	            axios.delete(`api/items/${this.item.id}`)
	                .then(response => {
	                    
	                })
	                .catch(error => {
	                	axios.patch(`api/items/${this.item.id}/trash`)
	                	.then(response => {
	                    	this.$root.success = true;
	                        this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
							Vue.nextTick( () => this.$refs.vuetable.refresh() );
	                	})
	                	.catch(error => {
						  	console.log(error);
							this.$root.success = false;
							this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
						});
	                });
	        },
	        createOrders: function(){
	        	this.indexes = this.$refs.vuetable.selectedTo;
	        	this.$root.message = "Orders with selected items created: ";
	        	for (var i = 0; i < this.indexes.length; i++) {
	        		this.addOrder(this.indexes[i]);
	        	}
                this.$root.success = true;
                this.$router.replace({ path: `/meals/${this.order.meal_id}/orders` });
            },
	        addOrder: function(itemId){
	        	this.order.item_id = itemId;
                axios.post('api/orders', this.order)
                    .then(response=>{
                    	this.orders.push(response.data.id);
					    this.$root.message += response.status + ", " + response.statusText + ": Order no.: " + response.data.id + ", with Item no.: " + itemId;
                    })
                    .catch(error=>{
                        console.log(error);
                        this.$root.success = false;
					    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
            },
	        formatDate (value, fmt = 'D MMM YYYY') {
		      	return (value == null) ? '' : moment(value, 'YYYY-MM-DD').format(fmt);
		    },
		    onPaginationData (paginationData) {
		      this.$refs.pagination.setPaginationData(paginationData);
		      this.$refs.paginationInfo.setPaginationData(paginationData);
		    },
		    onChangePage (page) {
		      this.$refs.vuetable.changePage(page);
		    },
		    handleLoadError(error) {
		    	this.$root.success = false;
		 		this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
		    }
	    }
	};
</script>

<style>
.pagination {
  margin: 0;
  float: right;
}
.pagination a.page {
  border: 1px solid lightgray;
  border-radius: 3px;
  padding: 5px 10px;
  margin-right: 2px;
}
.pagination a.page.active {
  color: white;
  background-color: #337ab7;
  border: 1px solid lightgray;
  border-radius: 3px;
  padding: 5px 10px;
  margin-right: 2px;
}
.pagination a.btn-nav {
  border: 1px solid lightgray;
  border-radius: 3px;
  padding: 5px 7px;
  margin-right: 2px;
}
.pagination a.btn-nav.disabled {
  color: lightgray;
  border: 1px solid lightgray;
  border-radius: 3px;
  padding: 5px 7px;
  margin-right: 2px;
  cursor: not-allowed;
}
.pagination-info {
  float: left;
}
.custom-actions button.ui.button {
padding: 8px 8px;
}
.custom-actions button.ui.button > i.icon {
margin: auto !important;
}
</style>