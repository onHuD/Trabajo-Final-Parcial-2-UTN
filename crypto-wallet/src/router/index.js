import { createRouter, createWebHistory } from 'vue-router'
import Comprar from '../views/Comprar.vue'
import Vender from '../views/Vender.vue'
import Historial from '../views/Historial.vue'
import DetalleTransaccion from '../views/DetalleTransaccion.vue'
import EditarTransaccion from '../views/EditarTransaccion.vue'
import Portafolio from '../views/Portafolio.vue'

const routes = [
  { path: '/', redirect: '/historial' },
  { path: '/historial', component: Historial },
  { path: '/comprar', component: Comprar },
  { path: '/vender', component: Vender },
  { path: '/transaccion/:id', component: DetalleTransaccion },
  { path: '/editar/:id', component: EditarTransaccion },
  { path: '/portafolio', component: Portafolio }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router