<template>
    <add-edit :item="item" @item-validated="add"></add-edit>
</template>

<script type="text/javascript">

    import AddEdit from './add-edit.vue';

	export default {
        data: function() { 
            return {
                item: {
                    name: "", 
                    description: "",
                    price: "",
                    type: "",
                    photo_url: "",
                    url: null
                }
            }
        },
        components: {
            'add-edit': AddEdit
        },
        methods: {
            add: function() {
                if (this.$store.state.image) {
                    this.item.image = this.$store.state.image;
                }
                axios.post('api/items', this.item)
                    .then(response=>{
                        if (this.$store.state.image) {
                            this.$store.commit('clearImage');
                        }
                        this.$root.success = true;
                        this.$root.message = response.status + ", " + response.statusText + ": New Item created with no.: " + response.data.id;
                        this.$router.replace({ path: `/items/${response.data.id}/details` });
                    })
                    .catch(error => {
                        this.$root.success = false;
                        this.$root.message = error.response.status + ", " + error.response.statusText + ": " + error.response.data.message;
                    });
            },
        },
        mounted () {
            this.$root.message = null;
        }
	};
</script>

<style scoped>  
.form-group {
    margin-bottom: 2rem;
}
</style>