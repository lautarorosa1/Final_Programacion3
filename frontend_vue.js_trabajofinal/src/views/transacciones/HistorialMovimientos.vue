<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'

// Services
import { obtenerClientes } from '@/services/clienteService.js'
import { obtenerTransacciones, eliminarTransaccion } from '@/services/transaccionService.js'

const router = useRouter()
const clientes = ref([])
const transacciones = ref([])
const clientFilter = ref('')
const mensaje = ref('')
const mensajeClase = ref('')

// Cargar clientes
async function cargarClientes() {
  try {
    clientes.value = await obtenerClientes()
  } catch (error) {
    console.error('Error cargando clientes:', error)
    mensaje.value = 'Error al cargar clientes.'
    mensajeClase.value = 'error'
  }
}

// Cargar transacciones
async function cargarTransacciones() {
  try {
    transacciones.value = await obtenerTransacciones(clientFilter.value)
  } catch (error) {
    console.error('Error cargando transacciones:', error)
    mensaje.value = 'Error al cargar transacciones.'
    mensajeClase.value = 'error'
  }
}

// Filtrar por cliente
function filtrar() {
  cargarTransacciones()
}

// Formato de fecha legible
function formatoFecha(fechaStr) {
  const d = new Date(fechaStr)
  return d.toLocaleString()
}

// Ordenar por fecha descendente
const transaccionesFiltradas = computed(() =>
  [...transacciones.value].sort((a, b) => new Date(b.fechaHora) - new Date(a.fechaHora))
)

// Obtener nombre de cliente
function nombreCliente(id) {
  const c = clientes.value.find(c => c.id === id)
  return c ? c.name : id
}

// Navegación
function verTransaccion(id) {
  router.push(`/transaccion/${id}`)
}

function editarTransaccion(id) {
  router.push(`/editar-transaccion/${id}`)
}

// Borrar transacción
async function borrarTransaccion(id) {
  if (!confirm("¿Seguro que querés borrar la transacción?")) return

  try {
    await eliminarTransaccion(id)
    cargarTransacciones()
  } catch (error) {
    console.error(error)
    mensaje.value = "No se pudo borrar la transacción."
    mensajeClase.value = "error"
  }
}

// Cargar datos al montar
onMounted(() => {
  cargarClientes()
  cargarTransacciones()
})
</script>

<template>
  <div>
    <h2>Historial de Movimientos</h2>

    <label>
      Filtrar por cliente:
      <select v-model="clientFilter" @change="filtrar">
        <option value="">Todos los clientes</option>
        <option v-for="c in clientes" :key="c.id" :value="c.id">
          {{ c.id }} - {{ c.name }}
        </option>
      </select>
    </label>

    <p v-if="mensaje" :class="mensajeClase">{{ mensaje }}</p>

    <table v-if="transaccionesFiltradas.length">
      <thead>
        <tr>
          <th>ID</th>
          <th>Cliente</th>
          <th>Criptomoneda</th>
          <th>Exchange</th>
          <th>Tipo</th>
          <th>Cantidad</th>
          <th>Monto ARS</th>
          <th>Fecha/Hora</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="t in transaccionesFiltradas" :key="t.id">
          <td>{{ t.id }}</td>
          <td>{{ nombreCliente(t.clientId) }}</td> 
          <td>{{ t.codigoCripto }}</td>
          <td>{{ t.exchange }}</td>
          <td>
            {{
              t.tipoTransaccion === 'purchase' || t.tipoTransaccion === 0
                ? 'Compra'
                : 'Venta'
            }}
          </td>
          <td>{{ Number(t.cantidadCripto).toLocaleString(undefined, { maximumFractionDigits: 8 }) }}</td>
          <td>${{ t.montoARS.toLocaleString() }}</td>
          <td>{{ formatoFecha(t.fechaHora) }}</td>
          <td class="acciones">
            <button class="btn-ver" @click="verTransaccion(t.id)">Ver</button>
            <button class="btn-editar" @click="editarTransaccion(t.id)">Editar</button>
            <button class="btn-borrar" @click="borrarTransaccion(t.id)">Borrar</button>
          </td>
        </tr>
      </tbody>
    </table>

    <p v-else>No hay transacciones para mostrar.</p>
  </div>
</template>