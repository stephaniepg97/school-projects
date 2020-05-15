<template>
<div v-if="item != null">
    <div class="form-group">
            <img v-bind:src="item.url">
    </div>
   <div class="form-group">
        <label for="txtType">Type</label>
        <input
            type="text" class="form-control" readonly="readonly" id="txtType" name="type" v-model="item.type"/>
    </div>
    <div class="form-group">
    <label for="inputName">Name</label>
    <input
        type="text" class="form-control" v-model="item.name" readonly="readonly" name="name" id="inputName"/>
    </div>
    <div class="form-group">
        <label for="inputDescription">Description</label>
        <input
            type="text" class="form-control" v-model="item.description" readonly="readonly" name="description" id="inputDescription"/>
    </div>
    <div class="form-group">
        <label for="inputPrice">Price</label>
        <input
            type="number" step="0.01" class="form-control" v-model="item.price" name="price" id="inputPrice" readonly="readonly"/>
    </div>
    <div class="form-group">
        <router-link :to="{ path: `/items/${this.id}/edit` }" replace class="ui basic button"><i class="edit icon"></i></router-link>
        <a class="btn btn-light" v-on:click.prevent="cancel">Return</a>
    </div>
</div>
</template>

<script type="text/javascript">
	export default {
        data: function() { 
            return {
                item: null,
            }
        },
        props: {
            id: {
                required: true,
                default: "-1"
            } 
        },
        methods: {
            cancel: function() {
                this.$router.go(-1);
            }
        },
        mounted () {
            this.$root.message = null;
            axios.get('api/items/'+this.id)
                .then(response=>{
                    this.item = response.data;
                })
                .catch(error => {
                    this.$root.success = false;
                    this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                });
        }
	};
</script>

<style scoped>	

</style>