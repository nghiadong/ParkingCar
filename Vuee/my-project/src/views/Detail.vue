<template>
    <v-container class="mt-8">
      <v-row>
        <v-col md="6">
          <v-carousel 
            v-model="activeSlide" 
            hide-delimiters
            show-arrows="hover"
            style="height:300px"
          >
            <v-carousel-item 
              v-for="(image, index) in productImages" 
              :key="index"
              
            >
              <v-img
                @mousemove="zoom"
                :src="image.src"
                :alt="image.alt"
                
              />
            </v-carousel-item>
          </v-carousel>
          <v-row class="thumbnails" v-if="productImages.length > 1">
            <v-col
              v-for="(image, index) in productImages"
              :key="index"
              @click="activeSlide = index"
            >
              <v-img :src="image.src" :alt="image.alt" />
            </v-col>
          </v-row>
        </v-col>
  
        <v-col md="6">
          <v-container ps-lg-10 mt-6 mt-md-0> 
            <!-- Heading -->
            <h1 class="mb-1">{{ detailInfo.TenXe }}</h1>
            <div class="fs-4" v-for="detail in detailInfo" :key="detail">
              <span class="fw-bold text-dark" style="font-size: 25px; font-family: ;">{{ detail.MoTa }}</span>
            </div>
  
            <!-- HR -->
            <v-divider class="my-6"></v-divider>
  
            <!-- Buttons for different quantities -->
              <v-row class="mb-5">
                <v-btn-toggle
                  color="green"
                  dark
                >
                  <h3 style="margin-left: 20px;">Số lượng</h3>
        
              </v-btn-toggle>
              </v-row>
  
            <!-- Quantity Input -->
            <form action="">
              <v-text-field v-model="quantity" min="1" max="10" type="number" style="max-width:100px"></v-text-field>
            </form>
  
            <!-- Add to Cart Button -->
            <v-btn @click="showForm = !showForm" class="mt-3" color="primary">
              <v-icon>
                <svg xmlns="https://autopro8.mediacdn.vn/134505113543774208/2023/5/4/lamborghini-day-12-16831115061231715119432-1683168790025-16831687901942078871154.jpg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-plus">
                      <line x1="12" y1="5" x2="12" y2="19"></line>
                      <line x1="5" y1="12" x2="19" y2="12"></line>
                </svg>
              </v-icon>
              Đặt Xe  
            </v-btn>

            <v-dialog v-model="showForm" max-width="600px">
              <v-card style="padding: 15px;">
                <h3>Đặt Xe</h3>
                <v-text-field label="ID Bãi Xe" v-model="BaiXeID"></v-text-field>
                <v-text-field label="Ngày đặt" v-model="NgayDatISO" type="text"></v-text-field>
                <v-text-field label="Biển số" v-model="BienSo"></v-text-field>
                <v-text-field label="Danh Mục" v-model="DanhMucXeID"></v-text-field>
                <v-select
                  v-model="TrangThai"
                  :items="['Đã vào', 'Chưa vào']"
                  label="Trạng thái"
                ></v-select>
                <v-btn @click="doPost" style="background-color: rgb(15, 238, 164);">Thêm mới</v-btn>
                <span>{{ message }}</span>
              </v-card>
            </v-dialog>

                    <!-- HR -->
            <v-divider class="my-6"></v-divider>
  
  
          </v-container>
        </v-col>
      </v-row>
    </v-container>
  </template>
  
  <script>
  import axios from 'axios';
  export default {
    data() {
      return {
        activeSlide: 0,
        quantity: 1,
        showForm: false,
        BaiXeID: this.$route.params.BaiXeID,
        detailInfo: '',
        NgayDat: "",
        BienSo: "",
        TrangThai: "",
        DanhMucXeID: "",
        ID: "",
        message: "",
        NgayDat: new Date(),
        productImages: [
          { src: 'https://chungcuorientalwestlake.com/wp-content/uploads/2021/06/thiet-ke-bai-do-xe-may-5.jpg', alt: '' },
          { src: 'https://luhanhvietnam.com.vn/du-lich/vnt_upload/news/03_2023/bai-do-xe-san-bay-tan-son-nhat-TCP-Park-_.jpg', alt: '' },
          { src: 'https://luhanhvietnam.com.vn/du-lich/vnt_upload/news/03_2023/bai-do-xe-san-bay-tan-son-nhat-TCPPark_.jpg', alt: '' },
          { src: 'https://baoxaydung.com.vn/stores/news_dataimages/2023/042023/25/14/420230425141415.jpg?rt=20230425141429 ', alt: '' },
        ],
      };
    },
    created() {
    this.fetchDetailInfo();
    },
    computed: {
    NgayDatISO: {
      get() {
        // Chuyển đổi ngày thành chuỗi ISO 8601
        return this.NgayDat.toISOString().slice(0, 19); // Cắt bớt phần thừa để loại bỏ giờ và phút
      },
      set(value) {
        // Chuyển đổi chuỗi ISO 8601 thành ngày
        this.NgayDat = new Date(value);
      },
    },
  },
    methods: {
  
      increment() {
        this.quantity += 1;
      },
      decrement() {
        if (this.quantity > 1) {
          this.quantity -= 1;
        }
      },
      addToCart() {
        // Add your logic for adding to cart here
      },
      fetchDetailInfo() {
      // Gọi API hoặc sử dụng dữ liệu có sẵn để lấy thông tin chi tiết
      axios.get(`https://localhost:7117/api/ChiTietBaiXe?id=` + this.$route.params.BaiXeID)
        .then(response => {
          console.log(this.$route.params.BaiXeID)
          console.log(response.data);
          this.detailInfo = response.data;
          console.log(this.detailInfo[0].MoTa);
        })
        .catch(error => {
          console.error('Error fetching detail info:', error);
        });
    },
    doPost() {
          axios.post('https://localhost:7117/api/DatXe/', {
            BaiXeID: this.BaiXeID,
            NgayDat: this.NgayDat,
            BienSo: this.BienSo,
            DanhMucXeID: this.DanhMucXeID,
            TrangThai: this.TrangThai
          })
          .then(response => {
            this.message = "Thêm mới thành công!";
            // Cập nhật danh sách sinh viên sau khi thêm mới
          })
          .catch(e => {
            this.message = "Thêm mới thất bại!";
            console.error(e);
          });
        },
    },
  };
  </script>
  
  <style>
  
  </style>
  