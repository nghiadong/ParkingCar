import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },

  {
  path: '/dashboard',
  name: 'dashboard',
  component: () => import(/* webpackChunkName: "about" */ '../views/Dashboard1.vue')
  },
  {
    path: '/users',
    name: 'users',
    component: () => import(/* webpackChunkName: "about" */ '../views/Users.vue')
  },
  {
    path: '/information',
    name: 'information',
    component: () => import(/* webpackChunkName: "about" */ '../views/Information.vue')
  },
  {
    path: '/car',
    name: 'car',
    component: () => import(/* webpackChunkName: "about" */ '../views/Car.vue')
  },
  {
    path: '/detail/:BaiXeID',
    name: 'detail',
    component: () => import(/* webpackChunkName: "about" */ '../views/Detail.vue')
  },
  {
    path: '/static',
    name: 'static',
    component: () => import(/* webpackChunkName: "about" */ '../views/Static.vue')
  }


  
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
