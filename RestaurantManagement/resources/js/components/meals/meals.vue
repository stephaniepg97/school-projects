<template>
<div>
	<div v-if="this.$store.state.user">
		<div class="form-group" v-if="(this.$store.state.user.type == 'waiter')">
			<router-link :to="{ path: `${this.$route.path}/add` }" class="ui basic button" v-if="id != -1">New Meal</router-link>
		</div>
		<div class="form-group" v-if="(this.$store.state.user.type == 'manager')">
			<p><em>Show the average time it takes to handle each meal, per month: </em></p>
			<router-link to="/meals/orders/averages" replace class="ui basic button">Graphic</router-link>
		</div>
	</div>
	<filter-bar :search="'meals'" @filter-set="filter" @filter-reset="reset"></filter-bar>
    <vuetable ref="vuetable"
      :api-url="apiUrl"
      :fields="fields"
      pagination-path="meta"
      :css="css.table"
      :sort-order="sortOrder"
      @vuetable:pagination-data="onPaginationData"
      @vuetable:load-error="handleLoadError"
    >
    <template slot="terminate" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="setTerminated(props.rowData)" v-if="!(props.rowData.state == 'terminated') && !(props.rowData.state == 'paid') && !(props.rowData.state == 'not paid')">Set To Terminated</button>
  		</div>
	</template>
	<template slot="details" slot-scope="props">
		<div class="custom-actions">
			<router-link :to="{ path: `/meals/${props.rowData.id}/details` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
			<router-link :to="{ path: `/meals/${props.rowData.id}/orders` }" class="ui basic button">Orders</router-link>
  		</div>
	</template>
	<template slot="actions" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="setNotPaid(props.rowData)" v-if="props.rowData.state == 'terminated'">Set To Not Paid</button>
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

	import FilterBar from '../filterBar/FilterBar.vue';
	Vue.component('filter-bar', FilterBar)
	
	export default {
		data: function(){
			return {
				apiUrl: `api${this.$route.path}?page=1`,
				meal: null,
		        fields: [
					{
						name: '__handle',
						titleClass: 'center aligned',
						dataClass: 'center aligned'
					},
					{
						name: 'id',
						sortField: 'id',
					},
					{
						name: 'state',
						sortField: 'state',
					}, 
					{
						name: 'start',
						sortField: 'start',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						callback: 'formatDate|DD-MM-YYYY'
					},
					{
						name: 'end',
						sortField: 'end',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						callback: 'formatDate|DD-MM-YYYY'
					},
					{
			          name: 'total_price_preview',
			          title: 'Total Price Preview',
			          titleClass: 'text-center',
			          dataClass: 'text-right',
			          callback: 'formatNumber'
			        },
					{
						name: 'invoice.name',
						title: 'Customer',
						sortField: 'invoice.name',
					}, 
					{
						name: 'waiter.name',
						title: 'Responsible Waiter',
						sortField: 'cook.name',
						visible: this.id == -1
					}, 
					{
						name: 'table.table_number',
						title: 'Table',
						sortField: 'table.table_number',
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
						name: '__slot:details',
						title: 'Details',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
					},
					{
						name: '__slot:actions',
						title: 'Actions',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'manager')
					},
					{
						name: '__slot:terminate',
						title: 'Terminate',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'waiter')
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
				this.apiUrl = `api${this.$route.path}?page=1`;

	    		if (filter.date) {
	    			this.apiUrl += `&date=${filter.date}`;
	    		}

	    		if (filter.waiter) {
	    			this.apiUrl += `&waiter=${filter.waiter}`;
	    		}

	    		if (filter.state) {
	    			this.apiUrl += `&state=${filter.state}`;
	    		}

				console.log(this.apiUrl);
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
			reset: function() {
				this.apiUrl = `api${this.$route.path}?page=1`;
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
	        setNotPaid: function(meal){
	        	this.meal = meal;
	            axios.patch(`api/meals/${this.meal.id}/not-paid`)
	                .then(response => {
			        	this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
			        	Vue.nextTick( () => this.$refs.vuetable.refresh() );
			        })
	              .catch(error => {
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
			       });
	        },
	        setTerminated: function(meal){
	        	this.meal = meal;
	            axios.patch(`api/meals/${this.meal.id}/terminated`)
	                .then(response => {
			        	this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + response.data.response.message;
			        	Vue.nextTick( () => this.$refs.vuetable.refresh() );
			        })
	              .catch(error => {
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
			       });
	        },
	        formatDate (value, fmt = 'D MMM YYYY') {
		      	return (value == null) ? '' : moment(value, 'YYYY-MM-DD').format(fmt);
		    },
		    formatNumber (value) {
		      	return accounting.formatNumber(value, 2);
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