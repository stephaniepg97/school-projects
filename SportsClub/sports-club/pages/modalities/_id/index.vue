<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
      <h4>Modality Details</h4>
      <p>Id: {{ modality.id }}</p>
      <p>Name: {{ modality.name }}</p>
      <div>
        <p>Graduations:</p>
        <b-table striped over :items="graduations" :fields="fields">
          <template v-slot:cell(actions)="row">
            <div class="custom-actions">
              <nuxt-link class="btn btn-sm btn-primary" :to="`/graduations/${row.item.id}`">Details</nuxt-link>
            </div>
          </template>
        </b-table>
      </div>
      &nbsp;
      <div>
        <p>Sport Classes:</p>
        <b-table striped over :items="classes" :fields="fields">
          <template v-slot:cell(actions)="row">
            <div class="custom-actions">
              <nuxt-link class="btn btn-sm btn-primary" :to="`/classes/${row.item.id}`">Details</nuxt-link>
            </div>
          </template>
        </b-table>
      </div>
      &nbsp;
      <div>
        <nuxt-link class="btn btn-sm btn-primary" :to="`/modalities/${this.$route.params.id}/edit`">Edit</nuxt-link>
        <nuxt-link class="btn btn-sm btn-primary" to="/modalities">Return</nuxt-link>
      </div>
    </b-container>
  </div>
</template>
<script>
export default {
  data() {
    return {
        modality: {},
        graduations: [],
        classes: [],
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
};
</script>
