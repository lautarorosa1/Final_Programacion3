<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { obtenerTransaccion, editarTransaccion } from '@/services/transaccionService.js'

const route = useRoute()
const router = useRouter()
const transaccion = ref({})

// Cargar la transacción al montar el componente
async function cargar() {
  try {
    transaccion.value = await obtenerTransaccion(route.params.id)
  } catch (error) {
    console.error("Error al cargar la transacción:", error.message)
  }
}

// Guardar los cambios
async function guardar() {
  try {
    await editarTransaccion(route.params.id, transaccion.value)
    router.push("/historial-movimientos")
  } catch (error) {
    console.error("Error al guardar la transacción:", error.message)
  }
}

onMounted(cargar)
</script>

<template>
  <h2>Editar Transacción</h2>

  <div v-if="transaccion && Object.keys(transaccion).length">
    <label>Cripto</label>
    <input v-model="transaccion.codigoCripto">

    <label>Tipo</label>
    <select v-model="transaccion.tipoTransaccion">
      <option value="purchase">Compra</option>
      <option value="sale">Venta</option>
    </select>

    <label>Cantidad</label>
    <input type="number" v-model="transaccion.cantidadCripto">

    <label>Monto ARS</label>
    <input type="number" v-model="transaccion.montoARS">

    <br><br>

    <button class="button_guardar" @click="guardar">Guardar</button>
  </div>
</template>