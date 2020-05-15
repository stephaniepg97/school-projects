<template>
<div>
	<filter-bar :search="'invoices'" @filter-set="filter" @filter-reset="reset"></filter-bar>
    <vuetable ref="vuetable"
      :api-url="apiUrl"
      :fields="fields"
      pagination-path="meta"
      :css="css.table"
      :sort-order="sortOrder"
      @vuetable:pagination-data="onPaginationData"
      @vuetable:load-error="handleLoadError"
    >
    <template slot="meal" slot-scope="props">
		<div class="custom-actions">
			<router-link :to="{ path: `/meals/${props.rowData.meal.id}/details` }" replace class="ui basic button">Meal</router-link>
  		</div>
	</template>
	<template slot="actions" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="notPaid(props.rowData)" v-if="props.rowData.state == 'pending'">Set To Not Paid</button>
  		</div>
	</template>
	<template slot="pay" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="pay(props.rowData)" v-if="props.rowData.state == 'pending'">Do Payment</button>
  		</div>
	</template>
	<template slot="pdf" slot-scope="props">
		<div class="custom-actions">
			<a class="ui basic button" :href="props.rowData.url" :download="props.rowData.filename" v-if="props.rowData.url">Download from Storage</a>
			<button class="ui basic button" @click="pdf(props.rowData)">Generate PDF & Download</button>
			<a class="ui basic button" :href="props.rowData.url" v-if="props.rowData.url">File</a>
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
	Vue.component('filter-bar', FilterBar);
	
	export default {
		data: function(){
			return { 
				apiUrl: 'api/invoices?page=1',
				invoice: null,
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
						name: 'customer_name',
						title: 'Customer',
						sortField: 'customer_name',
					}, 
					{
						name: 'state',
						sortField: 'state',
					}, 
					{
						name: 'date',
						sortField: 'date',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						callback: 'formatDate|DD-MM-YYYY'
					},
					{
						name: 'meal.waiter.name',
						title: 'Responsible Waiter'
					},
					{
			          name: 'total_price',
			          title: 'Total Price',
			          titleClass: 'text-center',
			          dataClass: 'text-right',
			          callback: 'formatNumber'
			        },
			        {
						name: 'nif',
						title: 'NIF',
						titleClass: 'text-center',
						dataClass: 'text-right',
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
						name: '__slot:meal',
						title: 'Meal',
						titleClass: 'center aligned',
						dataClass: 'center aligned'
					},
					{
						name: '__slot:actions',
						title: 'Actions',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'manager')
					},
					{
						name: '__slot:pay',
						title: 'Pay',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'cashier')
					},
					{
						name: '__slot:pdf',
						title: 'PDF',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && ((this.$store.state.user.type == 'manager') || (this.$store.state.user.type == 'cashier'))
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
		components: {
	    	Vuetable,
    		VuetablePagination,
    		VuetablePaginationInfo
	    },
	    methods: {
	    	filter: function(filter) {
	    		this.apiUrl = 'api/invoices?page=1';
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
				this.apiUrl = 'api/invoices?page=1';
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
	        notPaid: function(invoice){
	        	this.invoice = invoice;
	            axios.patch('api/invoices/'+this.invoice.id+'/not-paid')
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
	        pay: function(invoice){
	        	this.invoice = invoice;
                axios.patch('api/users/'+this.invoice.id+'/do-payment')
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
	        pdf: function(invoice){
	        	this.invoice = invoice;
                axios({
				  url: `api/invoices/${this.invoice.id}/pdf`,
				  method: 'GET',
				  responseType: 'blob'
				}).then((response) => {
					const url = window.URL.createObjectURL(new Blob([response.data]));
					const link = document.createElement('a');
					link.href = url;
					link.setAttribute('download', this.invoice.filename);
					document.body.appendChild(link);
					link.click();
				}).catch(error => {
                    this.$root.success = false;
                    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.message;
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