<template>
<div>
	<div class="form-group">
		<button class="ui basic button" @click="hide">Hide</button>
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
    <template slot="averages" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="averages(props.rowData)">Averages</button>
			<button class="ui basic button" @click="graph(props.rowData)">Graph</button>
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

	<div class="form-group">
		<button class="ui basic button" @click="hide">Hide</button>
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
				item: null,
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
						name: 'price',
						titleClass: 'text-center',
						dataClass: 'text-right',
						callback: 'formatNumber'
			        },
					{
						name: '__slot:averages',
						title: 'Averages '
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
	    		this.params = filter;
				this.apiUrl = 'api/items?page=1';
	    		if (filter.type) {
	    			this.apiUrl += `&type=${filter.type}`;
	    		}

	    		if (filter.name) {
	    			this.apiUrl += `&name=${filter.name}`;
	    		}
				console.log(this.apiUrl);
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
			averages: function(item){
				this.item = item;
			    this.$emit('show-item-averages', this.item.id);
			},
			graph: function(item) {
	    		this.$router.replace({ path: `/items/${item.id}/orders/averages` });
	    	},
			hide: function() {
	    		this.$emit('hide-table');
	    	},
			reset: function() {
				this.apiUrl = "api/items?page=1";
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
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