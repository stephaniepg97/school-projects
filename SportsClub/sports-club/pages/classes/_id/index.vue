<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
      <h4>Sport Class Details</h4>
      <p>Id: {{ sportClass.id }}</p>
      <p>Name: {{ sportClass.name }}</p>
      <p>Value: {{ sportClass.value }}</p>
      <p>Stock/Limit: {{ sportClass.stock }}</p>
      <p>Modality: {{ sportClass.modality.name }}</p>
      <p>Original Id: {{ sportClass.originalId }}</p>
      <p>Season: {{ season }}</p>
      <p>Timetable: {{ timetable }}</p>
      <p>Coach: {{ sportClass.coach.name }}</p>
      <p>Rank: {{ sportClass.rank.name }}</p>
      <nuxt-link class="btn btn-sm btn-primary" :to="`/classes/${this.$route.params.id}/edit`">Edit</nuxt-link>
    </b-container>
  </div>
</template>
<script>
export default {
  data() {
    return {
      sportClass: {
        modality: {
            name: null,
        },
        season: {
            startInMilliseconds: null,
            endInMilliseconds: null
        },
        timetable: {
            dayOfWeek: null,
            secondOfDayDuration: null,
            secondOfDayInit: null
        },
        coach: {
          name: null,
        },
        rank: {
          name: null
        }
      }
    };
  },
  created() {
    this.$axios.$get(`/api/classes/${this.$route.params.id}`)
    .then(sportClass => {
      this.sportClass = sportClass || {}
    })
  },
  computed: {
      season() {
          return new Date(this.sportClass.season.startInMilliseconds).toDateString() + ' -/- ' + new Date(this.sportClass.season.endInMilliseconds).toDateString()
      },
      timetable() {
          const weekday = new Array(7);
          weekday[0] = "Sunday";
          weekday[1] = "Monday";
          weekday[2] = "Tuesday";
          weekday[3] = "Wednesday";
          weekday[4] = "Thursday";
          weekday[5] = "Friday";
          weekday[6] = "Saturday";
          return weekday[this.sportClass.timetable.dayOfWeek] + '// Init:' + this.sportClass.timetable.secondOfDayInit + '// Duration:' + this.sportClass.timetable.secondOfDayDuration
      },
  }
};
</script>
