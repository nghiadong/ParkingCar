<template>
    <div class="centered-content">
      <!-- Hiển thị thành phần Them -->
      <Them :themData="themData" @updateThemData="updateThemData" />
      <!-- Hiển thị thành phần Trangchinh -->
      <Trangchinh :trangChinhData="trangChinhData" @updateTrangChinhData="updateTrangChinhData" />
    </div>
  </template>
  
  <script>
 // import Them from './them.vue';
  import Trangchinh from './trangchinh.vue';
  
  export default {
    components: {
      //Them,
      Trangchinh,
    },
    data() {
      return {
        themData: null,
        trangChinhData: null,
      };
    },
    methods: {
      updateThemData(data) {
        // Cập nhật dữ liệu từ Them
        this.themData = data;
      },
      updateTrangchinhData(data) {
        // Cập nhật dữ liệu từ Trangchinh
        this.trangChinhData = data;
      },
    },
  };
  </script>
  
  <style>

  </style>