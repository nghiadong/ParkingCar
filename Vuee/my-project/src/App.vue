<template>
  <v-app id="inspire" style="padding: 15px;">
    <v-navigation-drawer v-model="drawer" color="rgb(9, 166, 187)">
      <template v-slot:prepend>
        <v-list-item
          lines="two"
          prepend-avatar="https://recmiennam.com/wp-content/uploads/2018/01/tai-hinh-nen-lionel-messi-dep-58.jpg"
          title="Nghĩa Đồng"
          subtitle=""
        ></v-list-item>
      </template>

      <v-divider></v-divider>

      <v-list density="compact" nav>
        <v-list-item @click="navigateTo('home')" prepend-icon="mdi-home-city" title="Home"></v-list-item>
        <v-list-item @click="navigateTo('dashboard')" prepend-icon="mdi-view-dashboard" title="Dashboard"></v-list-item>
        <v-list-item @click="navigateTo('car')" prepend-icon="mdi-car" title="Cars"></v-list-item>
        <v-list-item @click="navigateTo('information')" prepend-icon="mdi-account-group-outline" title="Information"></v-list-item>
        <v-list-item @click="navigateTo('users')" prepend-icon="mdi-information" title="Statistical"></v-list-item>
        
      </v-list>
    </v-navigation-drawer>

    <v-app-bar class="custom-app-bar" color="rgb(12, 141, 158)">
    <v-app-bar-nav-icon @click="checkLogin()"></v-app-bar-nav-icon>
    <v-app-bar-title>Quản lý bãi gửi xe</v-app-bar-title>

    <!-- Biểu tượng tìm kiếm -->
    <v-text-field
      v-model="search"
      append-icon="mdi-magnify"
      label="Tìm kiếm"
      single-line
      hide-details
      @click:append="performSearch"
    ></v-text-field>

    <!-- Thanh tùy chọn -->
    <v-spacer></v-spacer>
    <!-- <v-btn icon @click="showLoginFormDialog = true" v-if="isLogin=false">
  <v-icon>mdi-account</v-icon>
</v-btn> -->

<v-dialog v-model="showLoginFormDialog" max-width="400">
  <!-- Dialog đăng nhập -->
      <v-card>
        <v-card-title style="text-align: center;">Đăng nhập</v-card-title>
        <v-card-text>
          <v-form>
            <v-text-field label="Tên đăng nhập" v-model="username"></v-text-field>
            <v-text-field label="Mật khẩu" v-model="password" type="password"></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="handleLogin()">Đăng nhập</v-btn>
          <v-btn color="secondary" @click="showLoginFormDialog = false">Đóng</v-btn>
          <v-btn color="primary" @click="showRegisterFormDialog = true">Đăng ký</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Dialog đăng ký -->
    <v-dialog v-model="showRegisterFormDialog" max-width="400">
      <v-card>
        <v-card-title style="text-align: center;">Đăng ký</v-card-title>
        <v-card-text>
          <v-form>
            <v-text-field label="Tên đăng nhập" v-model="registrationInfo.username"></v-text-field>
            <v-text-field label="Mật khẩu" v-model="registrationInfo.password" type="password"></v-text-field>
            <v-text-field label="Email" v-model="registrationInfo.email"></v-text-field>
            <!-- Thêm các trường khác của form đăng ký ở đây -->
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="register()">Đăng ký</v-btn>
          <v-btn color="secondary" @click="showRegisterFormDialog = false">Đóng</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-btn icon @click="isLogin = false, drawer=!drawer" v-if="isLogin==true">
      
      <v-icon>mdi-logout</v-icon>
    </v-btn>
  </v-app-bar>


    <v-main class="custom-main">
      <router-view></router-view>
    </v-main>
    <v-footer class="" style="background-color: rgb(12, 141, 158);">
    <v-row justify="center" no-gutters >
      <v-btn 
        
       
        color="white"
        variant="text"
        class="mx-2"
        rounded="xl"
      >
        {{ link }}
      </v-btn>
      <v-col class="text-center mt-4" cols="12" style="color: white;">
        {{ new Date().getFullYear() }} — <strong>Vuetify</strong>
      </v-col>
    </v-row>
  </v-footer>

  </v-app>

  
  
</template>

<script>
import axios from 'axios';
import { ref, onBeforeMount } from 'vue';
import { useRouter } from 'vue-router';

export default {
  data() {
    return {
      isLogin: false,
      showLoginFormDialog: false,
      drawer: ref(false),
      router: useRouter(),
      search: ref(''),
      showRegisterFormDialog: false,
      showLoginForm: ref(true),
      username: ref(''),
      password: ref(''),
      registrationInfo: {
        id: 0,
        username: '',
        password: '',
        email: '',
        role: '1'
      },
    };
  },
  methods: {
    checkLogin() {
      this.drawer = !this.drawer
      if (this.isLogin == false) {
        this.showLoginFormDialog = true;
        this.drawer = !this.drawer;
      }
    },
    navigateTo(routeName) {
      this.router.push({ name: routeName });
    },
    performSearch() {
      // Thực hiện tìm kiếm dựa trên giá trị trong biến `search`
      // Tùy thuộc vào yêu cầu cụ thể của bạn
    },
    information() {
      // Hiển thị hộp thoại form đăng nhập/đăng ký khi nhấp vào nút "Account"
      this.showLoginFormDialog = true;
    },
    register() {
      axios.post('https://localhost:7117/api/Registration/registration', this.registrationInfo)
      .then(response =>{
        if(response.data == 1){
          this.showRegisterFormDialog = false;
        }
      }).catch(e =>{
        console.log(e);
      })
    },
    handleLogin(){
      axios.post('https://localhost:7117/api/Registration/login?acc='+ this.username +'&pass=' + this.password)
      .then(response =>{
        if(response.data == 1){
          this.drawer = !this.drawer;
          this.isLogin = true;
          this.showLoginFormDialog = false;
        }
        else {
        // Hiển thị thông báo khi tên đăng nhập hoặc mật khẩu không đúng
        alert('Sai tên đăng nhập hoặc mật khẩu');
      }
      }).catch(e =>{
        console.log(e);
      })
    }
  },
  onBeforeMount() {
    // Đặt các thứ khác trước khi component được mounted nếu cần
  },
};



</script>

