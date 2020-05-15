<template>
  <div>
    <div class="topnav">
      <a class="active" href="http://localhost:3000/">Home</a>
      <nuxt-link to="/products" class>Products</nuxt-link>
      <nuxt-link to="/payments" class>Payments</nuxt-link>
      <nuxt-link to="/users" class>Users</nuxt-link>
    </div>
    <b-container>
    <h4>Edit Sport Class</h4>
    <b-form @submit.prevent="save">
      <b-form-group label="Id:">
        <b-input name="id" v-model.trim="sportClass.id" disabled />
      </b-form-group>
      <b-form-group label="Name:">
        <b-input name="name" v-model.trim="sportClass.name" required />
      </b-form-group>
      <b-form-group label="Value:">
        <b-input name="value" v-model.trim="sportClass.value" required />
      </b-form-group>
      <b-form-group label="Stock:">
        <b-input name="stock" v-model.trim="sportClass.stock" required />
      </b-form-group>
      <b-form-group label="Modality:">
        <b-input name="id" v-model.trim="sportClass.modality.name" disabled />
      </b-form-group>
      <b-form-group label="Season:">
        <select v-model="sportClass.season">
          <option v-for="s in seasons" v-bind:value="s">
            {{ season(s.startInMilliseconds, s.endInMilliseconds) }}
          </option>
        </select>
        &nbsp;
        <button class="btn btn-primary" :to="`/seasons/create`">New</button>
      </b-form-group>
      <b-form-group label="Timetable:">
        <select v-model="sportClass.timetable">
          <option v-for="t in timetables" v-bind:value="t">
            {{ timetable(t.dayOfWeek, t.secondOfDayDuration, t.secondOfDayInit) }}
          </option>
        </select>
        &nbsp;
        <button class="btn btn-primary" :to="`/timetables/create`">New</button>
      </b-form-group>
      <b-form-group label="Coach:">
        <select v-model="sportClass.coach">
          <option v-for="c in coaches" v-bind:value="c">
            {{ c.name }}
          </option>
        </select>
        &nbsp;
        <button class="btn btn-primary" :to="`/users/create`">New</button>
      </b-form-group>
      <b-form-group label="Rank:">
        <select v-model="sportClass.rank">
          <option v-for="r in ranks" v-bind:value="r">
            {{ r.name }}
          </option>
        </select>
        &nbsp;
        <button class="btn btn-primary" :to="`/users/create`">New</button>
      </b-form-group>
    </b-form>
    <div>
      <button class="btn btn-sm btn-primary" @click.prevent="save">Save</button>
      <button class="btn btn-sm btn-primary" @click.prevent="cancel">Cancel</button>
    </div>
    </b-container>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
        sportClass: {
            id: null,
            name: null,
            value: null,
            stock: null,
            category: {
                name: 'SportClass',
                products: []
            },
            modality: {
                name: null
            },
            season: {
                id: null,
                startInMilliseconds: null,
                endInMilliseconds: null,
                classes: []
            },
            rank: {
                name: null,
                classes: []
            },
            timetable: {
                id: null,
                dayOfWeek: null,
                secondOfDayDuration: null,
                secondOfDayInit: null,
                classes: []
            },
            coach: {
                username: null,
                password: null,
                name: null,
                email: null,
                classes: []
            },
            attendances: []
        },
        seasons: [],
        timetables: [],
        coaches: [],
        ranks: []
    };
  },
  created() {
    this.$axios.$get(`/api/modalities/${this.$route.params.id}`)
        .then(modality => {
            this.sportClass.modality = modality || {}
        })
    this.$axios.$get(`/api/classes/${this.$route.params.id}`)
          .then(sportClass => {
              this.sportClass = sportClass || {}
          })
    this.$axios.$get("/api/seasons")
        .then(seasons => {
            this.seasons = seasons || []
        })
    this.$axios.$get("/api/timetables")
        .then(timetables => {
            this.timetables = timetables || []
        })
    this.$axios.$get("/api/coaches")
        .then(coaches => {
            this.coaches = coaches || []
        })
    this.$axios.$get("/api/ranks")
        .then(ranks => {
            this.ranks = ranks || []
        })
  },
  methods: {
    season(start, end) {
        return new Date(start).toDateString() + ' -/- ' + new Date(end).toDateString()
    },
    timetable(dayOfWeek, secondOfDayDuration, secondOfDayInit) {
        const weekday = new Array(7);
        weekday[0] = "Sunday";
        weekday[1] = "Monday";
        weekday[2] = "Tuesday";
        weekday[3] = "Wednesday";
        weekday[4] = "Thursday";
        weekday[5] = "Friday";
        weekday[6] = "Saturday";
        return weekday[dayOfWeek] + '// Init:' + secondOfDayInit + '// Duration:' + secondOfDayDuration
    },
    save() {
      this.$axios.$put(`api/classes/${this.$route.params.id}`, this.sportClass)
        .then(response => {
          console.log(response.data);
          this.$router.push(`/modalities/${this.sportClass.modality.id}/edit`);
        })
        .catch(error => {
          console.log(error);
        });
    },
    cancel() {
      this.$router.push(`/modalities/${this.sportClass.modality.id}/edit`);
    }
  }
};
</script>
