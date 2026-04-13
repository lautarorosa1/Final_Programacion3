const API_URL = "https://localhost:7130/api/transacciones"

// OBTENER UNA TRANSACCIÓN
export async function obtenerTransaccion(id) {
  const response = await fetch(`${API_URL}/${id}`)

  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al obtener la transacción")
  }

  return await response.json()
}

// OBTENER TODAS LAS TRANSACCIONES (opcionalmente filtradas por cliente) ENTENDER POR QUE AQUI USO DOS GET Y EN CLIENTES SOLO 1
export async function obtenerTransacciones(clientId = '') {
  let url = API_URL
  if (clientId) url += `?clientId=${clientId}`

  const response = await fetch(url)
  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al obtener transacciones")
  }

  return await response.json()
}
// CREAR TRANSACCIÓN
export async function crearTransaccion(transaccion) {
  const response = await fetch(API_URL, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(transaccion)
  })

  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al crear la transacción")
  }

  return await response.json()
}

// ELIMINAR TRANSACCIÓN
export async function eliminarTransaccion(id) {
  const response = await fetch(`${API_URL}/${id}`, {
    method: "DELETE"
  })

  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al eliminar la transacción")
  }

  return true
}

// EDITAR TRANSACCIÓN
export async function editarTransaccion(id, transaccion) {
  const response = await fetch(`${API_URL}/${id}`, {
    method: "PATCH",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(transaccion)
  })

  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al actualizar la transacción")
  }

  return await response.json()
}

export async function obtenerRanking(crypto, tipo) {
  const response = await fetch(`${API_URL}/ranking?crypto=${crypto}&tipo=${tipo}`)

  if (!response.ok) {
    throw new Error('Error al obtener ranking')
  }

  return await response.json()
}