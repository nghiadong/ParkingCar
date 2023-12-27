<template>
    <v-carousel>
  <v-carousel-item
    src="https://thienthanhlimo.com/wp-content/uploads/2022/05/101-anh-sieu-xe-4k-tai-free-lam-hinh-nen-dt-may-tinh-1.jpg"
    cover
  ></v-carousel-item>

  <v-carousel-item
    src="https://aozoom.com.vn/uploads/news/27-07-2023/12088cb28c3db934de9b013689c5eb2233ffdc76.jpg"
    cover
  ></v-carousel-item>

  <v-carousel-item
    src="https://haycafe.vn/wp-content/uploads/2021/12/Hinh-nen-xe-moto-sieu-dep-1-800x600.jpg"
    cover
  ></v-carousel-item>
</v-carousel>
<div class="space"></div>

    <v-row>
        <v-col md="9" >
            <v-row>
                <v-col md="3" v-for="bx in listBaiXe" :key="bx.ID">
                    <v-card
                        class="mx-auto"
                        max-width="400"
                    >
                        <v-img
                        class="align-end text-white"
                        height="200"
                        :src="'https://localhost:7117/Photos/' + bx.HinhAnh"
                        cover
                        >   
                        </v-img>

                        <v-card-subtitle class="pt-4">
                        {{ bx.ViTri }}
                        
                        </v-card-subtitle>

                        <v-card-subtitle class="pt-4">
                        <p>Chỗ trống: {{ bx.SoLuong }}</p>                        
                        </v-card-subtitle>

                        <v-card-text>
                        
                        <div>{{ bx.TenXe }}</div>
                        </v-card-text>

                        <v-card-actions>
                        <v-btn color="orange" @click="openFormDialog(bx.ID)">
                            Đặt chỗ
                        </v-btn>

                        <v-btn :to="{ name: 'detail', params: { BaiXeID: bx.ID } }" color="orange">
                        Xem chi tiết
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
                        </v-card-actions>
                    </v-card>
                </v-col>
            </v-row>
        </v-col>

        <v-col>
            <v-card>
                <v-img
                        class="align-end text-white"
                        height="800"
                        src="https://hoanghamobile.com/tin-tuc/wp-content/webp-express/webp-images/uploads/2023/08/hinh-nen-xe-do-2.jpg.webp"
                        cover
                        >                       
                        </v-img>
            </v-card>
            <div style="margin-top: 20px;"></div>
            <v-card style="margin-bottom: 10px;">
                <v-img
                        class="align-end text-white"
                        height="800"
                        src="https://didongviet.vn/dchannel/wp-content/uploads/2022/12/9hinh-nen-moto-hinh-nen-4k-cho-dien-thoai-didongviet@2x-576x1024.jpg"
                        cover
                        >                       
                        </v-img>
            </v-card>
        </v-col>
        
    </v-row>
</template>
<style>
  .space {
  margin-bottom: 20px; /* Điều này tạo khoảng cách 20px giữa "Thêm" và "Trangchinh" */
}
  </style>
<script>
import axios from 'axios';
export default {
  data() {
    return {  
      listBaiXe: [],
      errors: [],
      showForm: false,
      TenXe: "",
      ViTri: "",
      SoLuong: "",
      HinhAnh: "",
      DanhMucID: "",
      ID: "",
      BaiXeID: this.$route.params.BaiXeID,
      NgayDat: "",
      BienSo: "",
      TrangThai: "",
      DanhMucXeID: "",
      NgayDat: new Date(),
      message: "",
    };
  },
  created() {
    this.fetchBaiXes();
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
    fetchBaiXes() {
      axios.get('https://localhost:7117/api/BaiXe/')
        .then(response => {
          this.listBaiXe = response.data;
        })
        .catch(e => {
          this.errors.push(e);
        });
    },
    openFormDialog(BaiXeID) {
      // Mở form dialog và truyền BaiXeID
      this.showForm = true;
      this.BaiXeID = BaiXeID;
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
}
}
</script>