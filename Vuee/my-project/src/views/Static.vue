<template>
    <h1 style="text-align: center; color: #1ce960;">Thống kê số lượng xe vào bãi:</h1>
    <v-main style="background-color: #EEEEEE;">
      <side-bar/>
      <v-container>
        <v-row>
          <v-col v-for="(stat, index) in statistics" :key="index" cols="3" >
            <v-card>
              <v-card-title class="p-3 pt-2" >
                <v-row align="center" class="mt-1 ml-1 row" style="background-color: #ff6600;">
                  <v-col class="icon-lg mt-n4">
                    <v-avatar class="bg-gradient-dark shadow-dark border-radius-xl">
                      <v-icon color="white">mdi-car</v-icon>
                    </v-avatar>
                  </v-col>
                </v-row>
                <v-row justify="flex-end" class="pt-1">
                  <v-col class="text-right">
                    <p class="mb-1 font-weight-regular" style="font-size: 25px; opacity: 0.8"> Các xe {{ stat.TrangThai }}</p>
                    <h4 class="mb-0">Số lượng: {{ stat.SoLuong }}</h4>
                  </v-col>
                </v-row>
              </v-card-title>
              <v-card-actions class="p-3">
                <p class="mb-0">
                  <span class="text-success text-sm font-weight-bolder"><v-icon>mdi-account</v-icon></span>
                </p>
              </v-card-actions>
            </v-card>
            <br><br>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </template>
  
  <script>
  import axios from 'axios';
  export default {
    data() {
      return {
        statistics: [
          {
          },
        ],
      };
    },
    mounted() {
      this.fetchData();
    },
    methods: {
      fetchData() {
        axios.get("https://localhost:7117/api/DatXe/trangthai")
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
  
  <style scoped>
    .row{
      width: 64px; height: 64px; box-shadow: 0px 0px 8px;border-radius: 12px;
    }
  </style>