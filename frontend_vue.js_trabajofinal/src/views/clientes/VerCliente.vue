<script setup>
import { ref, onMounted, watch, nextTick} from 'vue'
import { useRoute } from 'vue-router'
import { obtenerCliente, obtenerEstadoCliente } from '@/services/clienteService'
import { Chart, PieController, ArcElement, Tooltip, Legend } from 'chart.js'

Chart.register(PieController, ArcElement, Tooltip, Legend)
const chartRef = ref(null)
let chartInstance = null

const route = useRoute()

const cliente = ref(null)
const estado = ref(null)

async function cargar() {
  try {
    const id = route.params.id

    cliente.value = await obtenerCliente(id)
    estado.value = await obtenerEstadoCliente(id)

  } catch (error) {
    console.error(error)
  }
}

function crearGrafico() {
  if (!estado.value || !estado.value.criptos.length) return

  const labels = estado.value.criptos.map(c => c.codigo.toUpperCase())
  const data = estado.value.criptos.map(c => c.dineroARS)

  // destruir gráfico anterior si existe
  if (chartInstance) {
    chartInstance.destroy()
  }

  chartInstance = new Chart(chartRef.value, {
    type: 'pie',
    data: {
      labels: labels,
      datasets: [{
        data: data,
        backgroundColor: [
          '#f7931a', // BTC
          '#627eea', // ETH
          '#2775ca'  // USDC
        ]
      }]
    }
  })
}

watch(estado, async () => {
  await nextTick()
  crearGrafico()
})


onMounted(cargar)
</script>

<template>
  <h2>Detalle Cliente</h2>

  <div v-if="cliente">
    <p><b>ID:</b> {{ cliente.id }}</p>
    <p><b>Nombre:</b> {{ cliente.name }}</p>
    <p><b>Email:</b> {{ cliente.email }}</p>
    <p><b>Transacciones:</b> {{ cliente.transacciones?.length || 0 }}</p> 
  </div>

  <div v-if="estado">
    <h3>Estado Financiero</h3>

    <table border="1">
      <thead>
        <tr>
          <th>Cripto</th>
          <th>Cantidad</th>
          <th>Dinero (ARS)</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="c in estado.criptos" :key="c.codigo">
          <td>{{ c.codigo }}</td>
          <td>{{ c.cantidad }}</td>
          <td>${{ $formatoARS(c.dineroARS) }}
          </td>
        </tr>
      </tbody>
    </table>

    <h3>
      <h3>Total (ARS): ${{ $formatoARS(estado.totalARS) }}</h3>
    </h3>

    <h3>Composición de la cartera</h3>
    <canvas ref="chartRef" class="grafico"></canvas>
  </div>
</template>