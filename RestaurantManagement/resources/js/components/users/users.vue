<template>
<div>
	<router-link to="/register" class="ui basic button">New Worker</router-link> 
    <filter-bar :search="'users'" @filter-set="filter" @filter-reset="reset"></filter-bar>
    <vuetable ref="vuetable"
      :api-url="apiUrl"
      :fields="fields"
      track-by="id"
      pagination-path="meta"
      :css="css.table"
      @vuetable:pagination-data="onPaginationData"
      @vuetable:load-error="handleLoadError"
    >
    <template slot="details" slot-scope="props">
		<div class="custom-actions">
    		<router-link :to="{ path: `/users/${props.rowData.id}/profile` }" replace class="ui basic button"><i class="zoom icon"></i></router-link>
    		<router-link :to="{ path: `/users/${props.rowData.id}/meals` }" replace class="ui basic button" v-if="props.rowData.type == 'waiter'">Meals</router-link>
    		<router-link :to="{ path: `/users/${props.rowData.id}/orders` }" replace class="ui basic button" v-if="(props.rowData.type == 'cook') || (props.rowData.type == 'waiter')">Orders</router-link>
  		</div>
	</template>
    <template slot="actions" slot-scope="props">
		<div class="custom-actions">
			<router-link :to="{ path: `/users/${props.rowData.id}/edit` }" replace class="ui basic button"><i class="edit icon"></i></router-link>
			<button class="ui basic button" @click="delete(props.rowData)">Delete</button>
    		<button class="ui basic button" @click="unblock(props.rowData)" v-if="props.rowData.blocked">Unblock</button>
    		<button class="ui basic button" @click="block(props.rowData)" v-else="props.rowData.blocked">Block</button>
  		</div>
	</template>
	<template slot="averages" slot-scope="props">
		<div class="custom-actions">
    		<router-link :to="{ path: `/users/${props.rowData.id}/orders/averages` }" replace class="ui basic button" v-if="(props.rowData.type == 'cook') || (props.rowData.type == 'waiter')">Graphic</router-link>
  		</div>
	</template>
	<template slot="photo" slot-scope="props">
		<div v-if="props.rowData.url">
			<a class="ui basic button" :href="props.rowData.url">
				<img v-bind:src="props.rowData.url" width="130px">
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

	import VueEvents from 'vue-events'
	Vue.use(VueEvents)

	import FilterBar from '../filterBar/FilterBar.vue';
	Vue.component('filter-bar', FilterBar)

	export default {
		data: function(){
			return {
				apiUrl: "api/users?page=1",
		        fields: [
					{
						name: '__handle',
						titleClass: 'center aligned',
						dataClass: 'center aligned'
					},
					{
						name: '__slot:photo',
						title: 'Photo',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						width: '150px'
					},
					{
						name: 'username',
						sortField: 'username',
					}, 
					{
						name: 'name',
						sortField: 'name',
					}, 
					{
						name: 'email',
						sortField: 'email'
					},
					{
						name: 'type',
						sortField: 'type'
					},
					{
						name: 'last_shift_start',
						sortField: 'last_shift_start',
						title: 'Last Shift Started At',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						callback: 'formatDate|DD-MM-YYYY'
					},
					{
						name: 'last_shift_end',
						sortField: 'last_shift_end',
						title: 'Last Shift Ended At',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						callback: 'formatDate|DD-MM-YYYY'
					},
					{
						name: 'shift_active',
						sortField: 'shift_active',
						title: 'On shift',
						callback: 'formatBoolean'
					},
					{
						name: 'blocked',
						sortField: 'shift_active',
						title: 'Is blocked',
						callback: 'formatBoolean'
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
						name: '__slot:averages',
						title: 'Averages ',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'manager') 
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
		components: {
	    	Vuetable,
    		VuetablePagination,
    		VuetablePaginationInfo
	    },
	    methods: {
	    	filter: function(filter) {
				this.apiUrl = "api/users?page=1";
	    		if (filter.type) {
	    			this.apiUrl += `&type=${filter.type}`;
	    		}

	    		if (filter.name) {
	    			this.apiUrl += `&name=${filter.name}`;
	    		}
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
			reset: function() {
				this.apiUrl = "api/users?page=1";
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
	    	delete: function(user) {
	    		this.user = user;
	        	axios.delete('api/users/'+this.user.id)
	              .then(response => {
	              		console.log(response);
			        	this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
						Vue.nextTick( () => this.$refs.vuetable.refresh() );
			        })
	              .catch(error => {
	              		console.log(error);
			        	this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                       // return axios.patch('api/users/'+data.id+'/trash');
			       });
	        },
			block: function(user) {
				this.user = user;
				axios.patch('api/users/'+this.user.id+'/block')
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
			unblock: function(user) {
				this.user = user;
				axios.patch('api/users/'+this.user.id+'/unblock')
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
			allcap (value) {
		      return value.toUpperCase();
		    },
		    formatDate (value, fmt = 'D MMM YYYY') {
		      return (value == null)
		        ? ''
		        : moment(value, 'YYYY-MM-DD').format(fmt);
		    },
		    formatBoolean (value) {
		    	return value ? 'Yes' : 'No';
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