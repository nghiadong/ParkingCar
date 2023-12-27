<template>
    <div class="Form1" style="padding: 25px;">
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Danh Mục</th>
            <th>Tên Xe</th>
            <th>Vị trí</th>
            <th>Số lượng</th>
            <th>Hình Ảnh</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="bx in listBaiXe" :key="bx.id">
            <td>{{ bx.ID }}</td>
            <td>{{ bx.DanhMucID }}</td>
            <td>{{ bx.TenXe }}</td>
            <td>{{ bx.ViTri }}</td>
            <td>{{ bx.SoLuong }}</td>
            <td>{{ bx.HinhAnh }}</td>
            <td style="text-align: center;">
              <v-btn icon @click="confirmDelete(bx.ID)" class="btn btn-outline-danger btn-sm">
                <v-icon>mdi-delete</v-icon>
              </v-btn>
              <v-btn icon @click="showEditDialog(bx)" class="btn btn-outline-primary btn-sm">
                <v-icon>mdi-pencil</v-icon>
              </v-btn>
            </td>
          </tr>
        </tbody>
      </table>
  
      <!-- Dialog Sửa -->
      <v-dialog v-model="editDialog" max-width="600px">
        <template v-slot:activator="{ on }"></template>
        <v-card>
          <v-card-title>Sửa thông tin Bãi Xe</v-card-title>
          <v-card-text>
            <v-text-field v-model="editedBaiXe.TenXe" label="Tên Xe"></v-text-field>
            <v-text-field v-model="editedBaiXe.ViTri" label="Vị trí"></v-text-field>
            <v-text-field v-model="editedBaiXe.SoLuong" label="Số lượng"></v-text-field>
            <!-- Thêm các trường khác tương tự -->
  
            <!-- Hiển thị lỗi sửa -->
            <ul v-if="editErrors.length">
              <li v-for="error in editErrors" :key="error.message">{{ error.message }}</li>
            </ul>
          </v-card-text>
          <v-card-actions>
            <v-btn @click="saveEdit">Lưu</v-btn>
            <v-btn @click="cancelEdit">Hủy</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
  
      <!-- Hiển thị lỗi -->
      <ul v-if="errors.length">
        <li v-for="error in errors" :key="error.message">{{ error.message }}</li>
      </ul>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        listBaiXe: [],
        errors: [],
        // Thêm biến để quản lý trạng thái dialog sửa
        editDialog: false,
        // Dữ liệu của bảng đang được sửa
        editedBaiXe: {},
        // Danh sách lỗi khi sửa
        editErrors: [],
      };
    },
    created() {
      this.fetchBaiXes();
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
      confirmDelete(ID) {
        if (confirm("Bạn có muốn xóa Bãi Xe có id: " + ID + " không?")) {
          this.deleteBaiXe(ID);
        }
      },
      deleteBaiXe(ID) {
        axios.delete('https://localhost:7117/api/BaiXe/' + ID)
          .then(response => {
            // Xóa thành công, cập nhật lại danh sách
            this.fetchBaiXes();
          })
          .catch(e => {
            this.errors.push(e);
          });
      },
      showEditDialog(baiXe) {
        // Hiển thị dialog sửa và gán giá trị của bảng vào biến editedBaiXe
        this.editedBaiXe = Object.assign({}, baiXe);
        this.editDialog = true;
      },
      saveEdit() {
        // Gửi request lưu thông tin sửa đổi
        axios.post('https://localhost:7117/api/BaiXe/' + this.editedBaiXe.ID, this.editedBaiXe)
  
          .then(response => {
            // Lưu thành công, cập nhật lại danh sách và đóng dialog
            this.fetchBaiXes();
            this.editDialog = false;
          })
          .catch(e => {
            // Xử lý lỗi khi sửa
            this.editErrors.push(e);
          });
      },
      cancelEdit() {
        // Hủy bỏ sửa và đóng dialog
        this.editDialog = false;
        // Đặt lại danh sách lỗi
        this.editErrors = [];
      },
    },
  };
  </script>
  
  <style>
  table {
    border-collapse: collapse;
    width: 100%;
  }
  
  th, td {
    border: 1px solid #dddddd;
    text-align: left;
    padding: 8px;
  }
  
  th {
    background-color: #f2f2f2;
  }
  
  tr:nth-child(even) {
    background-color: #f2f2f2;
  }
  
  tr:hover {
    background-color: #d9d9d9;
  }
  
  ul {
    list-style: none;
    padding: 0;
  }
  
  li {
    margin-bottom: 10px;
  }
  .modal {
  display: none;
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgba(0, 0, 0, 0.4);
  }
  
  .modal-content {
  background-color: #fff;
  margin: 15% auto;
  padding: 20px;
  width: 70%;
  }
  
  .modal-content h3 {
  text-align: center;
  }
  
  .modal-content v-text-field {
  width: 100%;
  }
  
  .modal-content button {
  display: block;
  margin: 10px auto;
  }
  
  /* Hiển thị modal dialog khi biến showEditDialog là true */
  .modal.show {
  display: block;
  }
  </style>