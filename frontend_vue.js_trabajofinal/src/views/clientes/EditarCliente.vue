<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

import { obtenerClientes, editarCliente } from '@/services/clienteService'

const route = useRoute()
const router = useRouter()

const cliente = ref({})

async function cargar() {
  try {
    cliente.value = await obtenerClientes(route.params.id)
  } 
  catch (error) {
    console.error(error)
  }
}

async function guardar() {
  try {
    await editarCliente(route.params.id, cliente.value)
    router.push("/listado-clientes")
  } 
  catch (error) {
    console.error(error)
  }
}
onMounted(cargar)
</script>

<template>
  <h2>Editar Cliente</h2>
  <div v-if="cliente">
    <label>Nombre</label>
    <input v-model="cliente.name">
    <label>Email</label>
    <input v-model="cliente.email">
    <br><br>
    <button class="button_guardar" @click="guardar">Guardar</button>
  </div>
</template>