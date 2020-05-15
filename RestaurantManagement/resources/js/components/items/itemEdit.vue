<template>
<div>
    <add-edit :item="item" @item-validated="edit"></add-edit>
</div>
</template>

<script type="text/javascript">

    import AddEdit from './add-edit.vue';

	export default {
        data: function() { 
            return {
                item: null
            }
        },
        props: {
            id: {
                required: true,
                default: "-1"
            } 
        },
        components: {
            'add-edit': AddEdit
        },
	    methods: {
	        edit: function() {
                if (!this.item.url) {
                   this.item.image = this.$store.state.image;
                }
                axios.put('api/items/'+this.item.id, this.item)
                    .then(response=>{
                        if (!this.item.url) {
                            this.$store.commit('clearImage');
                        }
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": Item no.: " + response.data.id + " updated successfully.";
                        this.$router.replace({ path: `/items/${response.data.id}/details` });
                    })
                    .catch(error => {
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
            }
	    },
		mounted () {
            this.$root.message = null;
            axios.get('api/items/'+this.id)
                    .then(response=>{
                        this.item = response.data;
                        console.log(this.item);
                    })
                    .catch(error => {
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
        }
	};
</script>

<style>  
.form-group {
    margin-bottom: 2rem;
}
</style>