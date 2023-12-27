<!-- App.vue -->
<template>
  <div id="app">
    <h1>Monthly DatXe Statistics</h1>
    <div v-if="loading">Loading...</div>
    <div v-else>
      <table>
        <thead>
          <tr>
            <th>Month</th>
            <th>Number of DatXe</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(stat, index) in statistics" :key="index">
            <td>{{ stat.Thang }}</td>
            <td>{{ stat.SoLuongDatXe }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      loading: true,
      statistics: [],
    };
  },
  mounted() {
    this.fetchData();
  },
  methods: {
    fetchData() {
      axios.get("https://localhost:7117/api/DatXe/statistics")
        .then((response) => {
          this.statistics = response.data;
          this.loading = false;
        })
        .catch((error) => {
          console.error("Error fetching data:", error);
          this.loading = false;
        });
    },
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

table {
  border-collapse: collapse;
  width: 100%;
}

th, td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

th {
  background-color: #f2f2f2;
}
</style>