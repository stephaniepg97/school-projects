<template>
<div>
	<div class="form-group">
		<button class="ui basic button" @click="hide">Hide</button>
	</div>
	<filter-bar :search="'users_stats'" @filter-set="filter" @filter-reset="reset"></filter-bar>
    <vuetable ref="vuetable"
      :api-url="apiUrl"
      :fields="fields"
      pagination-path="meta"
      :css="css.table"
      track-by="id"
      @vuetable:pagination-data="onPaginationData"
      @vuetable:load-error="handleLoadError"
    >
    <template slot="actions" slot-scope="props">
		<div class="custom-actions">
			<button class="ui basic button" @click="averages(props.rowData)">Averages</button>
			<button class="ui basic button" @click="graph(props.rowData)">Graph</button>
  		</div>
	</template>
	<template slot="photo" slot-scope="props">
		<div v-if="props.rowData.url">
			<a class="ui basic button" :href="props.rowData.url">
				<img :src="props.rowData.url" width="130px">
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

	import Vuetable from 'vuetable-2/src/components/Vuetable'
	import VuetablePagination from 'vuetable-2/src/components/VuetablePagination'
	import VuetablePaginationInfo from 'vuetable-2/src/components/VuetablePaginationInfo'
	import Vue from 'vue'

	export default {
        data: function () {
			return {
				apiUrl: `api/users?page=1&type=${this.type}`,
				user: null,
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
						name: 'id',
						sortField: 'id',
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
						name: '__slot:actions',
						title: 'Actions',
						titleClass: 'center aligned',
						dataClass: 'center aligned',
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
            type: {
                type: String,
                required: true
            } 
        },
		components: {
	    	Vuetable,
    		VuetablePagination,
    		VuetablePaginationInfo
	    },
	    methods: {
	    	filter: function(filter) {
				this.apiUrl = `api/users?page=1&type=${this.type}`;
	    		if (filter.name) {
	    			this.apiUrl += `&name=${filter.name}`;
	    		}
				console.log(this.apiUrl);
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
			reset: function() {
				this.apiUrl = `api/users?page=1&type=${this.type}`;
				Vue.nextTick( () => this.$refs.vuetable.refresh() );
			},
	    	averages: function(user){
			    this.$emit('show-user-averages', user.id);
			},
			graph: function(user) {
				this.user = user;
	    		this.$router.replace({ path: `/users/${this.user.id}/orders/averages` });
	    	},
	    	hide: function() {
	    		this.$emit('hide-table');
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
	    },
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