<template>
<div>
    <div class="form-group">
        <label for="number">Number</label>
        <input
            type="number" class="form-control" v-model="table.table_number"
            name="number" id="number"/>
    </div>
    <div class="form-group">
        <a class="btn btn-primary" v-on:click.prevent="add">Confirm</a>
        <a class="btn btn-light" v-on:click.prevent="cancel">Cancel</a>
    </div>
</div>
</template>

<script type="text/javascript">
	export default {
		data: function(){
            return { 
                table: {
                    table_number: -1
                }
            }
        }, 
	    methods: {
	        add: function(){
	            axios.post('api/tables', this.table)
	                .then(response=>{
	                	this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": New table created with no.: " + response.data.table_number;
                        this.$router.replace({ path: "/tables" });
	                })
                    .catch(error=>{
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
	        },
            cancel: function() {
                this.$router.go(-1);
            }
		},
        mounted () {
            this.$root.message = null;
        }
	};
</script>

<style scoped>	

</style>