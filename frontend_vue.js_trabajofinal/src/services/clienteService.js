const API_URL_CLIENTES = "https://localhost:7130/api/clientes";

// OBTENER LISTA CLIENTES
export async function obtenerClientes() {
  const response = await fetch(API_URL_CLIENTES)
  if (!response.ok) throw new Error("Error al obtener clientes")
  return await response.json()
}

// OBTENER UN SOLO CLIENTE (ID)
export async function obtenerCliente(id) {
  const response = await fetch(`${API_URL_CLIENTES}/${id}`)
  if (!response.ok) throw new Error("Error al obtener cliente")
  return await response.json()
}

// OBTENER ESTADO CLIENTE
export async function obtenerEstadoCliente(id) {
  const res = await fetch(`${API_URL_CLIENTES}/${id}/estado`)

  if (!res.ok) {
    const errorText = await res.text()
    throw new Error(errorText || "Error al obtener estado del cliente")
  }

  return await res.json()
}

// CREAR CLIENTE
export async function crearCliente(cliente) {
  const response = await fetch(API_URL_CLIENTES, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(cliente),
  })

  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al crear el cliente")
  }
  return await response.json()
}

// EDITAR CLIENTE
export async function editarCliente(id, cliente) {
  const response = await fetch(`${API_URL_CLIENTES}/${id}`, {
    method: "PATCH",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(cliente),
  })

  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al actualizar el cliente")
  }
  return await response.json()
}

// ELIMINAR CLIENTE
export async function eliminarCliente(id) {
  const response = await fetch(`${API_URL_CLIENTES}/${id}`, { method: "DELETE" })
  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || "Error al eliminar el cliente")
  }
  return true
}