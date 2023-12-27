<template>
  <div class="Form1" style="padding: 25px;">
    <div style="margin-bottom: 30px; display: flex;">
      <div style="margin-top: 30px;">
  <!-- Button để hiển thị form thêm mới -->
  <v-btn @click="showForm = !showForm" max-width="600px">Thêm thông tin</v-btn>
  
  <!-- Form thêm mới sinh viên -->
  <v-dialog v-model="showForm" max-width="600px">
    <v-card style="padding: 15px;">
      <h3>Thêm thông tin</h3>
      <v-text-field label="Danh mục" v-model="DanhMucID"></v-text-field>
      <v-text-field label="Tên xe" v-model="TenXe"></v-text-field>
      <v-text-field label="Vị trí" v-model="ViTri"></v-text-field>
      <v-text-field label=" Số lượng" v-model="SoLuong"></v-text-field>
      <v-file-input
        v-model="HinhAnh"
        label="Hình ảnh" 
      ></v-file-input>
      <v-btn @click="doPost" style="background-color: rgb(15, 238, 164);">Thêm mới</v-btn>
      <span>{{ message }}</span>
    </v-card>
  </v-dialog>
    </div>
    <div style="margin-left: 1000px; width: 700px; margin-top: 20px;">
      <v-text-field
      v-model="searchViTri"
      append-icon="mdi-magnify"
      label="Tìm kiếm"
      single-line
      hide-details
      @click:append="searchByViTri"
    ></v-text-field>
   </div>
    </div>
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
      <tr v-for="bx in listBaiXe" :key="bx.ID">
        <td>{{ bx.ID }}</td>
        <td>{{ bx.DanhMucID }}</td>
        <td>{{ bx.TenXe }}</td>
        <td>{{ bx.ViTri }}</td>
        <td>{{ bx.SoLuong }}</td>
        <!-- Display image using full URL -->
        <td class="d-flex justify-center align-center">
          <v-img :src="'https://localhost:7117/Photos/' + bx.HinhAnh" alt="" width="100px" height="100px"></v-img>
        </td>
        <!-- Action buttons -->
        <td style="text-align: center;">
          <v-btn icon @click="editBaiXe(bx)" class="btn btn-outline-primary btn-sm">
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
          <v-btn icon @click="confirmDelete(bx.ID)" class="btn btn-outline-danger btn-sm">
            <v-icon>mdi-delete</v-icon>
          </v-btn>
    </td>
  </tr>
</tbody>
    </table>

    <!-- Hiển thị lỗi -->
    <ul v-if="errors.length">
      <li v-for="error in errors" :key="error.message">{{ error.message }}</li>
    </ul>

    <!-- Dialog sửa thông tin -->
    <v-dialog v-model="editDialog" max-width="600px">
      <v-card>
        <v-card-title>
          Sửa Thông Tin Bãi Xe
        </v-card-title>
        <v-card-text>
          <v-text-field v-model="editedBaiXe.DanhMucID" label="Danh Mục"></v-text-field>
          <v-text-field v-model="editedBaiXe.TenXe" label="Tên Xe"></v-text-field>
          <v-text-field v-model="editedBaiXe.ViTri" label="Vị Trí"></v-text-field>
          <v-text-field v-model="editedBaiXe.SoLuong" label="Số Lượng"></v-text-field>
          <v-text-field v-model="editedBaiXe.HinhAnh" label="Hình Ảnh"></v-text-field>
          <!-- Thêm các trường sửa thông tin tại đây -->
        </v-card-text>
        <v-card-actions>
          <v-btn @click="saveEdit" color="primary">Lưu</v-btn>
          <v-btn @click="cancelEdit">Hủy</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {  
      listBaiXe: [],
      errors: [],
      editDialog: false,
      editedBaiXe: {},
      showForm: false,
      TenXe: "",
      ViTri: "",
      SoLuong: "",
      HinhAnh: "",
      DanhMucID: "",
      ID: "",
      message: "",
      searchViTri : ''
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
      axios.put('https://localhost:7117/api/BaiXe/' + baiXe.ID, baiXe)
        .then(response => {
          // Cập nhật thành công, cập nhật lại danh sách
          this.fetchBaiXes();
        })
        .catch(e => {
          this.errors.push(e);
        });
    },
    handleFileChange(event) {
    this.selectedFile = event.target.files[0];
    console.log('Selected File:', this.selectedFile);
    },
    doPost() {
          // let formData = new FormData();
          // let imageUrl;
          // formData.append("file",this.HinhAnh);
          // console.log(this.HinhAnh);
          // axios.post('https://localhost:7117/api/UploadImage/SaveFile?id=001', formData, {headers: {'Content-Type': 'multipart/form-data'}})
          // .then(response =>{
          //   imageUrl = response.data;
          // }).catch(e =>{
          //   console.log(e);
          // })
          axios.post('https://localhost:7117/api/BaiXe/', {
            DanhMucID: this.DanhMucID,
            TenXe: this.TenXe,
            ViTri: this.ViTri,
            SoLuong: this.SoLuong,
            HinhAnh: 'baixe1.jpg'
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
        searchByViTri() {
  if (this.searchViTri.trim() === '') {
    // Nếu giá trị vị trí trống, hiển thị tất cả danh sách
    this.fetchBaiXes();
  } else {
    // Nếu có giá trị vị trí, thực hiện tìm kiếm theo vị trí
    const viTri = this.searchViTri;
    axios.get('https://localhost:7117/api/BaiXe', { params: { viTri } })
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

  }
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