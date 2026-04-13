import { createRouter, createWebHistory } from 'vue-router';

//Pagina Principal
import Principal from '@/views/Principal.vue';
// Vistas de Clientes
import NuevoCliente from '@/views/clientes/NuevoCliente.vue';
import ListadoClientes from '@/views/clientes/ListadoClientes.vue';
import VerCliente from '@/views/clientes/VerCliente.vue';
import EditarCliente from '@/views/clientes/EditarCliente.vue';

// Vistas de Transacciones
import NuevaTransaccion from '@/views/transacciones/NuevaTransaccion.vue';
import HistorialMovimientos from '@/views/transacciones/HistorialMovimientos.vue';
import VerTransaccion from '@/views/transacciones/VerTransaccion.vue';
import EditarTransaccion from '@/views/transacciones/EditarTransaccion.vue';

const routes = [
  // Ruta Principal
  { path: '/', component: Principal },
  // Rutas Clientes
  { path: '/nuevo-cliente', component: NuevoCliente },
  { path: '/listado-clientes', component: ListadoClientes },
  { path: '/cliente/:id', component: VerCliente },
  { path: '/editar-cliente/:id', component: EditarCliente },

  // Rutas Transacciones
  { path: '/nueva-transaccion', component: NuevaTransaccion },
  { path: '/historial-movimientos', component: HistorialMovimientos },
  { path: '/transaccion/:id', component: VerTransaccion },
  { path: '/editar-transaccion/:id', component: EditarTransaccion },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;