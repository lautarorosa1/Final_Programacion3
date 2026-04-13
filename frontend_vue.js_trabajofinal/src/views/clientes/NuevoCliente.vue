<script setup>
import { ref } from 'vue'
import { crearCliente } from '@/services/clienteService'

const name = ref('')
const email = ref('')
const mensaje = ref('')
const mensajeClase = ref('')

function validarFormulario() {
  const errores = []

  if (!name.value || name.value.trim() === '') {
    errores.push('El nombre es obligatorio.')
  }

  if (!email.value || !email.value.includes('@')) {
    errores.push('El email es obligatorio y debe ser válido.')
  }

  return errores
}

async function guardarCliente() {

  const errores = validarFormulario()

  if (errores.length > 0) {
    mensaje.value = errores.join('\n')
    mensajeClase.value = 'error'
    return
  }

  const nuevoCliente = {
    name: name.value.trim(),
    email: email.value.trim()
  }

  try {

    await crearCliente(nuevoCliente)

    mensaje.value = 'Cliente creado con éxito!'
    mensajeClase.value = 'success'

    name.value = ''
    email.value = ''

  } catch (error) {

    mensaje.value = error.message
    mensajeClase.value = 'error al crear cliente'
  }
}
</script>

<template>
  <div>
    <h2>Nuevo Cliente</h2>

    <form @submit.prevent="guardarCliente">
      <label>
        Nombre:
        <input type="text" v-model="name" />
      </label>

      <label>
        Email:
        <input type="email" v-model="email" />
      </label>

      <button class="button_guardar" type="submit">Crear Cliente</button>
    </form>

    <p v-if="mensaje" :class="mensajeClase">{{ mensaje }}</p>
  </div>
</template>