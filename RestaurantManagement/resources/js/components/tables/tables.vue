<template>
<div>
	<router-link to="tables/add" class="ui basic button">New Table</router-link> 
    <filter-bar></filter-bar>
    <vuetable ref="vuetable"
      api-url="api/tables?page=1"
      :fields="fields"
      pagination-path="meta"
      :css="css.table"
      :sort-order="sortOrder"
      @vuetable:pagination-data="onPaginationData"
      @vuetable:load-error="handleLoadError"
    >
    <template slot="meal" slot-scope="props">
		<div class="custom-actions">
    		<router-link :to="{ path: `/meals/${props.rowData.meal.id}/details` }" replace class="ui basic button" v-if="props.rowData.meal">Meal</router-link>
  		</div>
	</template>
	<template slot="actions" slot-scope="props">
		<div class="custom-actions">
    		<button class="ui basic button" @click="delete(props.rowData)"><i class="delete icon"></i></button>
  		</div>
	</template>
    </vuetable>	
    <div class="vuetable-pagination">
      <vuetable-pagination-info ref="paginationInfo"
        info-class="pagination-info"
      ></vuetable-pagination-info>
      <vuetable-pagination ref="pagination"
        :css="css.pagination"
        @vuetable-pagination:change-page="onChangePage"
      ></vuetable-pagination>				
	</div>	
</div>	
</template>

<script type="text/javascript">

	import Vuetable from 'vuetable-2/src/components/Vuetable'
	import VuetablePagination from 'vuetable-2/src/components/VuetablePagination'
	import VuetablePaginationInfo from 'vuetable-2/src/components/VuetablePaginationInfo'
	import Vue from 'vue'
	
	export default {
		data: function(){
			return {
				fields: [
			        {
						name: '__handle',
						titleClass: 'center aligned',
						dataClass: 'center aligned'
					},
					{
						name: 'table_number',
						sortField: 'table_number',
						title: 'Number',
					},
					{
						name: '__slot:meal',
						title: 'Meal',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
					},
					{
						name: '__slot:actions',
						title: 'Actions',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
						visible: (this.$store.state.user != null) && (this.$store.state.user.type == 'manager') 
					}
				],
				sortOrder: [
					{
					  field: 'table_number',
					  sortField: 'table_number',
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
	    	delete(table) {
				axios.delete('api/tables/'+table.table_number)
					.then(response => {
						console.log(response);
					  	this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": Table no.: " + table.table_number + " deleted successfully.";
			        	Vue.nextTick( () => this.$refs.vuetable.refresh() );
					})
					.catch(error => {
						console.log(error);
						axios.patch('api/tables/'+table.table_number+'/trash')
						.then(response => {
							console.log(response);
						    this.$root.success = true;
	                        this.$root.message = response.status + ", " + response.statusText + ": " + response.data.message;
				        	Vue.nextTick( () => this.$refs.vuetable.refresh() );
						})
						.catch(error => {
							this.$root.success = false;
						    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
						});
					});
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