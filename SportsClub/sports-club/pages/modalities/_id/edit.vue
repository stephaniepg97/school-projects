<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
      <h4>Edit Modality</h4>
      <b-form @submit.prevent="save">
        <b-form-group label="Id:">
          <b-input name="id" v-model.trim="modality.id" disabled />
        </b-form-group>
        <b-form-group label="Name:">
          <b-input name="name" v-model.trim="modality.name" required />
        </b-form-group>
      </b-form>
      <div>
        <p>Graduations:</p>
        <b-table striped over :items="graduations" :fields="fields">
          <template v-slot:cell(actions)="row">
            <div class="custom-actions">
              <nuxt-link class="btn btn-sm btn-primary" :to="`/graduations/${row.item.id}/edit`">Edit</nuxt-link>
              <button class="btn btn-sm btn-danger" @click.prevent="remove(row.item.id, 'graduations')">Remove</button>
            </div>
          </template>
        </b-table>
        <nuxt-link class="btn btn-sm btn-primary" :to="`/modalities/${this.$route.params.id}/graduations/create`">Add New Graduation</nuxt-link>
      </div>
      &nbsp;
      <div>
        <p>Sport Classes:</p>
        <b-table striped over :items="classes" :fields="fields">
          <template v-slot:cell(actions)="row">
            <div class="custom-actions">
              <nuxt-link class="btn btn-sm btn-primary" :to="`/classes/${row.item.id}/edit`">Edit</nuxt-link>
              <button class="btn btn-sm btn-danger" @click.prevent="remove(row.item.id, 'classes')">Remove</button>
            </div>
          </template>
        </b-table>
        <nuxt-link class="btn btn-sm btn-primary" :to="`/modalities/${this.$route.params.id}/classes/create`">Add New Sport Class</nuxt-link>
      </div>
      &nbsp
      <div>
        <nuxt-link class="btn btn-primary" to="/modalities/create">Create a New Modality</nuxt-link>
        <button class="btn btn-sm btn-primary" @click.prevent="cancel">Cancel</button>
      </div>
    </b-container>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
        modality: {},
        classes: [],
        graduations: [],
        fields: ['id', 'name', 'value', 'stock', 'originalId', 'actions']
    };
  },
  created() {
    this.$axios.$get(`/api/modalities/${this.$route.params.id}`)
    .then(modality => {
        this.modality = modality || {}
    })
    this.$axios.$get(`/api/modalities/${this.$route.params.id}/graduations`)
    .then(graduations => {
        this.graduations = graduations || []
    })
    this.$axios.$get(`/api/modalities/${this.$route.params.id}/classes`)
    .then(classes => {
        this.classes = classes || []
    })
  },
  methods: {
    remove(id, type) {
      this.$axios.$delete(`/api/${type}/${id}`)
        .then(() => {
            this.$router.reload()
        })
        .catch(error => {
            console.log(error);
        });
    },
    save() {
      this.$axios.$put(`/api/modalities/${this.$route.params.id}`, this.modality)
        .then((response) => {
          this.modality = response;
          this.$router.push(`/modalities/${this.$route.params.id}`);
        })
        .catch(error => {
          console.log(error);
        });
    },
    cancel() {
      this.$router.push("/modalities");
    }
  }
};
</script>
