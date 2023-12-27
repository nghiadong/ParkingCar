<template>
    <v-container>
        <v-row>
            <v-col>
                <v-subheader class="display-1 font-weight-bold"></v-subheader>
            </v-col>
        </v-row>

        <div style="margin-left: 1080px; width: 500px; margin-top: 20px; margin-bottom: 30px;">
      <v-text-field
          v-model="searchTrangThai"
          append-icon="mdi-magnify"
          label="Tìm kiếm"
          single-line
          hide-details
          @click:append="searchByTrangThai"
    ></v-text-field>
   </div>
        <v-row>
            <v-col  v-for="bx in listBaiXe" :key="bx.ID">
                <v-card>
                    <v-card-title class="text-h5">
                        <v-icon>mdi-car</v-icon>
                        <span class="ml-2">Thông Tin Gửi Xe</span>
                    </v-card-title>
                    <v-card-text>
                        <v-row>
                            <v-col cols="12" md="6">
                                <v-subheader>Bãi Xe ID:</v-subheader>
                                <div>{{ bx.BaiXeID }}</div>
                            </v-col>
                            <v-col cols="12" md="6">
                                <v-subheader>Ngày đặt:</v-subheader>
                                <div>{{ bx.NgayDat }}</div>
                            </v-col>
                            <v-col cols="12" md="6">
                                <v-subheader>Biển số:</v-subheader>
                                <div>{{ bx.BienSo }}</div>
                            </v-col>
                            <v-col cols="12" md="6">
                                <v-subheader>Danh mục:</v-subheader>
                                <div>{{ bx.DanhMucXeID }}</div>
                            </v-col>
                            <v-col cols="12" md="6">
                                <v-subheader>Trạng thái:</v-subheader>
                                <div>{{ bx.TrangThai }}</div>
                            </v-col>
                        </v-row>
                        <v-row class="d-flex justify-center align-center">
                            <v-btn value="Update"  @click="editBaiXe(bx)">
                                <v-icon icon>mdi-pencil</v-icon>
                                <span>Update</span>
                            </v-btn>
                            <v-btn value="Delete" @click="confirmDelete(bx.ID)">
                                <v-icon>mdi-delete</v-icon>

                                <span>Delete</span>
                            </v-btn>
                        </v-row>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
        <v-dialog v-model="editDialog" max-width="600px">
      <v-card>
        <v-card-title>
          Sửa Thông Tin Đặt xe
        </v-card-title>
        <v-card-text>
          <v-text-field v-model="editedBaiXe.BaiXeID" label="Bãi xe ID"></v-text-field>
          <v-text-field v-model="editedBaiXe.NgayDat" label="Ngày đặt"></v-text-field>
          <v-text-field v-model="editedBaiXe.BienSo" label="Biển số"></v-text-field>
          <v-text-field v-model="editedBaiXe.DanhMucXeID" label="Danh Mục"></v-text-field>
          <v-select
            v-model="editedBaiXe.TrangThai"
            :items="['Đã vào', 'Chưa vào']"
            label="Trạng thái"
           ></v-select>
          <!-- Thêm các trường sửa thông tin tại đây -->
        </v-card-text>
        <v-card-actions>
          <v-btn @click="saveEdit" color="primary">Lưu</v-btn>
          <v-btn @click="cancelEdit">Hủy</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    </v-container>
</template>
  
<script>
import axios from 'axios';
export default {
    name: 'StudentPage',
    data() {
        return {
            listBaiXe: [],
            errors: [],
            editDialog: false,
            editedBaiXe: {},
            showForm: false,
            BaiXeID: "",
            NgayDat: "",
            BienSo: "",
            TrangThai: "",
            DanhMucXeID: "",
            ID: "",
            message: "",
            NgayDat: new Date(),
            searchTrangThai: ''
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
        axios.get('https://localhost:7117/api/DatXe/')
        .then(response => {
          this.listBaiXe = response.data;
        })
        .catch(e => {
          this.errors.push(e);
        });
    },
    confirmDelete(ID) {
      if (confirm("Bạn có muốn xóa Bãi Xe có id: " + ID + " không?")) {
        this.deleteBaiXe(ID);
      }
    },
    deleteBaiXe(ID) {
      axios.delete('https://localhost:7117/api/DatXe/' + ID)
        .then(response => {
          // Xóa thành công, cập nhật lại danh sách
          this.fetchBaiXes();
        })
        .catch(e => {
          this.errors.push(e);
        });
    },
    editBaiXe(bx) {
      this.editedBaiXe = { ...bx };
      this.editDialog = true;
    },
    saveEdit() {
      this.updateBaiXe(this.editedBaiXe);
      this.editDialog = false;
    },
    cancelEdit() {
      this.editDialog = false;
    },
    updateBaiXe(baiXe) {
      axios.put('https://localhost:7117/api/DatXe/' + baiXe.ID, baiXe)
        .then(response => {
          // Cập nhật thành công, cập nhật lại danh sách
          this.fetchBaiXes();
        })
        .catch(e => {
          this.errors.push(e);
        });
    },
    searchByTrangThai() {
    if (this.searchTrangThai.trim() === '') {
      // Nếu giá trị vị trí trống, hiển thị tất cả danh sách
      this.fetchBaiXes();
    } else {
      // Nếu có giá trị vị trí, thực hiện tìm kiếm theo vị trí
      const TrangThai = this.searchTrangThai;
      axios.get('https://localhost:7117/api/DatXe', { params: { TrangThai } })
        .then(response => {
          // Kiểm tra xem response.data có phải là một mảng không
          if (Array.isArray(response.data)) {
            this.listBaiXe = response.data;
          } else {
            // Nếu không phải mảng, gán response.data vào một mảng để hiển thị
            this.listBaiXe = [response.data];
          }
        })
        .catch(e => {
          this.errors.push(e);
        });
  }
}
    },
};
</script>
  
<style scoped>
/* CSS tùy chỉnh nếu cần */
</style>