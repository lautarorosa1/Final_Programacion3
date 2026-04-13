<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { obtenerTransaccion } from '@/services/transaccionService.js'

const route = useRoute()
const transaccion = ref(null)
const mensaje = ref('')
const mensajeClase = ref('')

// Cargar transacción al montar
async function cargar() {
  try {
    transaccion.value = await obtenerTransaccion(route.params.id)
  } catch (error) {
    console.error('Error al cargar la transacción:', error)
    mensaje.value = 'No se pudo cargar la transacción.'
    mensajeClase.value = 'error'
  }
}

onMounted(cargar)
</script>

<template>
  <h2>Detalle de Transacción</h2>

  <p v-if="mensaje" :class="mensajeClase">{{ mensaje }}</p>

  <div v-else-if="transaccion">
    <p><b>ID:</b> {{ transaccion.id }}</p>
    <p><b>Cliente:</b> {{ transaccion.clientId }}</p>
    <p><b>Cripto:</b> {{ transaccion.codigoCripto }}</p>
    <p><b>Tipo:</b> {{ transaccion.tipoTransaccion }}</p>
    <p><b>Cantidad:</b> {{ transaccion.cantidadCripto }}</p>
    <p><b>Monto ARS:</b> {{ transaccion.montoARS }}</p>
    <p><b>Fecha:</b> {{ new Date(transaccion.fechaHora).toLocaleString() }}</p>
  </div>

  <p v-else>Cargando transacción...</p>
</template>