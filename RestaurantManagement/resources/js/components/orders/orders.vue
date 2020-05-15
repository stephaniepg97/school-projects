<template>
<div>
	<div class="form-group" v-if="id != -1">
		<label>Add new orders:</label>
		<router-link :to="{ path: `${this.$route.path}/add` }" class="ui basic button">New Orders</router-link>
	</div>

	<filter-bar :search="'orders'" @filter-set="filter" @filter-reset="reset"></filter-bar>
    
    <vuetable ref="vuetable"
      :api-url="apiUrl"
      :fields="fields"
      pagination-data="data"
      pagination-path="meta"
      :css="css.table"
      :sort-order="sortOrder"
      @vuetable:pagination-data="onPaginationData"
      @vuetable:load-error="handleLoadError"
    >
    <template slot="details" slot-scope="props">
		<router-link :to="{ path: `/orders/${props.rowData.id}/details` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
	</template>
    <template slot="item" slot-scope="props">
		<div v-if="props.rowData.item">
			<router-link :to="{ path: `/items/${props.rowData.item.id}/details` }" replace class="ui basic button">Item</router-link>
		</div>
	</template>
	<template slot="meal" slot-scope="props">
		<div v-if="props.rowData.meal">
			<router-link :to="{ path: `/meals/${props.rowData.meal.id}/details` }" replace class="ui basic button">Meal</router-link>
		</div>
	</template>
	<template slot="delete" slot-scope="props">
		<button class="ui basic button" @click="delete(props.rowData)"><i class="delete icon"></i></button>
	</template>
    <template slot="actions" slot-scope="props">
		<button class="ui basic button" @click="setInPreparation(props.rowData)" v-if="(props.rowData.state == 'confirmed') && props.rowData.item.type == 'dish'">Set As In Preparation</button>
		<button class="ui basic button" @click="setPrepared(props.rowData)" v-if="(props.rowData.state == 'in preparation') || ((props.rowData.item.type == 'drink') && (props.rowData.state == 'confirmed'))">Set As Prepared</button>
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

	import FilterBar from '../filterBar/FilterBar.vue';
	Vue.component('filter-bar', FilterBar)

	export default {
		data: function(){
			return { 
				apiUrl: `api${this.$route.path}?page=1`,
				order: null,
		        fields: [
					{
						name: '__handle',
						titleClass: 'center aligned',
						dataClass: 'center aligned'
					},
					{
						name: '__sequence',
						title: '#',
						titleClass: 'center aligned',
						dataClass: 'right aligned'
					},
					{
						name: '__checkbox',
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
						name: 'cook.name',
						title: 'Responsible Cook',
						sortField: 'cook.name',
					}, 
					{
						name: 'waiter.name',
						title: 'Responsible Waiter',
						sortField: 'cook.name',
					},
					{
						name: 'seasoning',
						sortField: 'cook.name',
						visible: this.id != -1
					},
					{
						name: '__slot:item',
						title: 'Item',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
					},
					{
						name: '__slot:meal',
						title: 'Meal',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: this.id == -1
					},
					{
						name: '__slot:delete',
						title: 'Delete',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'waiter')
					},
					{
						name: '__slot:details',
						title: 'Details',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'cook')
					},
					{
						name: '__slot:actions',
						title: 'Actions',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
					}
				],
				sortOrder: [
					{
					  field: 'start',
					  sortField: 'start',
					  direction: 'desc'
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

	    		if (filter.cook) {
	    			this.apiUrl += `&cook=${filter.cook}`;
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
			setPrepared: function(order){
				this.order = order;
	            axios.patch(`api/orders/${this.$order.id}/prepared`)
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
	        setInPreparation: function(order){
	        	this.order = order;
	            axios.patch(`api/orders/${this.$order.id}/in-preparation`)
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
	        formatDate (value, fmt = 'D MMM YYYY') {
		      return (value == null)
		        ? ''
		        : moment(value, 'YYYY-MM-DD').format(fmt);
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