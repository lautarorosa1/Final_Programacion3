<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { obtenerClientes, eliminarCliente } from '@/services/clienteService'

const clientes = ref([])
const mensaje = ref('')
const mensajeClase = ref('')

const router = useRouter()

// Cargar todos los clientes
async function cargarClientes() {
  mensaje.value = ''
  try {
    clientes.value = await obtenerClientes()
  } catch (error) {
    mensaje.value = error.message
    mensajeClase.value = 'error'
  }
}

// Navegar a la vista de un cliente
function verCliente(id) {
  router.push(`/cliente/${id}`)
}

// Navegar a la vista de edición de cliente
function editarCliente(id) {
  router.push(`/editar-cliente/${id}`)
}

// Eliminar cliente
async function borrarCliente(id) {
  if (!confirm("¿Seguro que querés borrar este cliente?")) return
  try {
    await eliminarCliente(id)
    cargarClientes()
  } catch (error) {
    mensaje.value = error.message
    mensajeClase.value = 'error'
  }
}

onMounted(() => {
  cargarClientes()
})
</script>

<template>
  <div>
    <h2>Listado de Clientes</h2>

    <p v-if="mensaje" :class="mensajeClase">{{ mensaje }}</p>

    <table v-if="clientes.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Email</th>
          <th># Transacciones</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="c in clientes" :key="c.id">
          <td>{{ c.id }}</td>
          <td>{{ c.name }}</td>
          <td>{{ c.email }}</td>
          <td>{{ c.transacciones?.length || 0 }}</td>
          <td class="acciones">
            <button class="btn-ver" @click="verCliente(c.id)">Ver</button>
            <button class="btn-editar" @click="editarCliente(c.id)">Editar</button>
            <button class="btn-borrar" @click="borrarCliente(c.id)">Borrar</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else>No hay clientes registrados.</p>
  </div>
</template>