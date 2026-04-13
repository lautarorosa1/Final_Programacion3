<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'

// Services
import { obtenerClientes } from '@/services/clienteService.js'
import { obtenerRanking, crearTransaccion } from '@/services/transaccionService.js'

const router = useRouter()

// Formulario
const clientId = ref('')
const codigoCripto = ref('BTC')
const cantidadCripto = ref('')
const tipoTransaccion = ref('purchase')
const exchangeSeleccionado = ref('') // Para elegir manualmente

// Datos
const clientes = ref([])
const ranking = ref([])
const usarMejorExchange = ref(true)

const mensaje = ref('')
const mensajeClase = ref('')

// Cargar clientes
async function cargarClientes() {
  try {
    clientes.value = await obtenerClientes()
  } catch (error) {
    mensaje.value = 'Error al cargar clientes.'
    mensajeClase.value = 'error'
  }
}

// Cargar ranking
async function cargarRanking() {
  try {
    ranking.value = await obtenerRanking(codigoCripto.value, tipoTransaccion.value)

    // Si usamos el mejor exchange, lo seleccionamos automáticamente
    if (usarMejorExchange.value && ranking.value.length > 0) {
      exchangeSeleccionado.value = ranking.value[0].exchange
    } else {
      exchangeSeleccionado.value = '' // no seleccionar automáticamente
    }
  } catch (error) {
    console.error(error)
    ranking.value = []
    exchangeSeleccionado.value = ''
  }
}

// Watch automático para actualizar ranking
watch([codigoCripto, tipoTransaccion, usarMejorExchange], () => {
  cargarRanking()
})

// Validaciones
function validarFormulario() {
  const errores = []

  if (!clientId.value || isNaN(parseInt(clientId.value))) {
    errores.push('Debe seleccionar un cliente válido.')
  }

  if (!['BTC','ETH','USDC'].includes(codigoCripto.value)) {
    errores.push('Criptomoneda inválida.')
  }

  const cantidad = parseFloat(cantidadCripto.value)
  if (!cantidadCripto.value || isNaN(cantidad) || cantidad <= 0) {
    errores.push('Cantidad inválida.')
  }

  if (!exchangeSeleccionado.value) {
    errores.push('Debe seleccionar un exchange.')
  }

  return errores
}

// Validar saldo
function validarSaldo(cliente) {
  if (tipoTransaccion.value !== 'sale') return true

  const crypto = codigoCripto.value.toLowerCase()

  const comprado = cliente.transacciones
    .filter(t => t.tipoTransaccion === 'purchase' && t.codigoCripto.toLowerCase() === crypto)
    .reduce((sum, t) => sum + t.cantidadCripto, 0)

  const vendido = cliente.transacciones
    .filter(t => t.tipoTransaccion === 'sale' && t.codigoCripto.toLowerCase() === crypto)
    .reduce((sum, t) => sum + t.cantidadCripto, 0)

  return parseFloat(cantidadCripto.value) <= (comprado - vendido)
}

// Enviar transacción
async function enviarTransaccion() {
  mensaje.value = ''

  const errores = validarFormulario()
  if (errores.length) {
    mensaje.value = errores.join('\n')
    mensajeClase.value = 'error'
    return
  }

  const cliente = clientes.value.find(c => c.id === parseInt(clientId.value))
  if (!cliente) {
    mensaje.value = 'Cliente no encontrado.'
    mensajeClase.value = 'error'
    return
  }

  if (!validarSaldo(cliente)) {
    mensaje.value = 'No tiene saldo suficiente.'
    mensajeClase.value = 'error'
    return
  }

  const nuevaTransaccion = {
    clientId: parseInt(clientId.value),
    codigoCripto: codigoCripto.value,
    tipoTransaccion: tipoTransaccion.value,
    cantidadCripto: parseFloat(cantidadCripto.value),
    exchange: exchangeSeleccionado.value
  }

  try {
    await crearTransaccion(nuevaTransaccion)

    mensaje.value = 'Transacción realizada con éxito!'
    mensajeClase.value = 'success'

    // Reset
    clientId.value = ''
    cantidadCripto.value = ''
    codigoCripto.value = 'BTC'
    tipoTransaccion.value = 'purchase'
    exchangeSeleccionado.value = ''
    usarMejorExchange.value = true

  } catch (error) {
    mensaje.value = error.message
    mensajeClase.value = 'error'
  }
}

// Init
onMounted(() => {
  cargarClientes()
  cargarRanking()
})
</script>

<template>
  <div>
    <h2>Nueva Transacción</h2>

    <form @submit.prevent="enviarTransaccion">
      <label>
        Cliente:
        <select v-model="clientId">
          <option value="">Seleccione un cliente</option>
          <option v-for="c in clientes" :key="c.id" :value="c.id">
            {{ c.id }} - {{ c.name }}
          </option>
        </select>
      </label>

      <label>
        Criptomoneda:
        <select v-model="codigoCripto">
          <option value="BTC">BTC</option>
          <option value="ETH">ETH</option>
          <option value="USDC">USDC</option>
        </select>
      </label>

      <label>
        Cantidad:
        <input type="number" step="0.0001" v-model="cantidadCripto" />
      </label>

      <label>
        Tipo de transacción:
        <select v-model="tipoTransaccion">
          <option value="purchase">Comprar</option>
          <option value="sale">Vender</option>
        </select>
      </label>

      <label style="display: grid; grid-auto-flow: column; align-items: center; column-gap: 0.5rem; cursor: pointer;">
        Usar mejor exchange automáticamente
        <input type="checkbox" v-model="usarMejorExchange" />
      </label>

      <label v-if="!usarMejorExchange">
        Exchange:
        <select v-model="exchangeSeleccionado">
          <option value="">Seleccione un exchange</option>
          <option v-for="r in ranking" :key="r.exchange" :value="r.exchange">
            {{ r.exchange }} - ${{ tipoTransaccion === 'purchase' ? $formatoARS(r.precioCompra) : $formatoARS(r.precioVenta) }}
          </option>
        </select>
      </label>

      <button class="button_guardar" type="submit">
        {{ tipoTransaccion === 'purchase' ? 'Comprar' : 'Vender' }}
      </button>
    </form>

    <h3>Mejores opciones</h3>

    <table v-if="ranking.length">
      <thead>
        <tr>
          <th>Exchange</th>
          <th v-if="tipoTransaccion === 'purchase'">Precio Compra</th>
          <th v-else>Precio Venta</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="(r, index) in ranking" :key="r.exchange" :style="index === 0 ? 'font-weight: bold; color: green;' : ''">
          <td>{{ r.exchange }}</td>
          <td v-if="tipoTransaccion === 'purchase'">${{ $formatoARS(r.precioCompra) }}</td>
          <td v-else>${{ $formatoARS(r.precioVenta) }}</td>
        </tr>
      </tbody>
    </table>

    <p v-else>No hay datos disponibles</p>

    <p v-if="mensaje" :class="mensajeClase">{{ mensaje }}</p>
  </div>
</template>