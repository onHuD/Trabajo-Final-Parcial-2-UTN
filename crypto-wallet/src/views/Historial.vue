<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const transacciones = ref([])
const error = ref('')
const mostrarModal = ref(false)
const idABorrar = ref(null)

async function cargarTransacciones() {
  try {
    const res = await fetch('https://localhost:7278/api/transacciones')
    if (!res.ok) {
      error.value = 'Error al cargar las transacciones'
      return
    }
    transacciones.value = await res.json()
  } catch (e) {
    error.value = 'No se pudo conectar con el servidor'
  }
}

function verDetalle(id) {
  router.push(`/transaccion/${id}`)
}

function editar(id) {
  router.push(`/editar/${id}`)
}

function confirmarBorrar(id) {
  idABorrar.value = id
  mostrarModal.value = true
}

async function borrar() {
  try {
    const res = await fetch(`https://localhost:7278/api/transacciones/${idABorrar.value}`, {
      method: 'DELETE'
    })
    if (!res.ok) {
      error.value = 'Error al borrar la transaccion'
      return
    }
    mostrarModal.value = false
    await cargarTransacciones()
  } catch (e) {
    error.value = 'No se puede conectar con el servidor'
  }
}

function cancelarBorrar() {
  mostrarModal.value = false
  idABorrar.value = null
}

onMounted(() => {
  cargarTransacciones()
})
</script>

<template>
  <div class="contenedor">
    <h2>Historial de Transacciones</h2>

    <p v-if="error" class="error">{{ error }}</p>
    <p v-if="transacciones.length === 0 && !error">No tenes transacciones cargadas</p>

    <table v-if="transacciones.length > 0">
      <thead>
        <tr>
          <th>ID</th>
          <th>Cripto</th>
          <th>Tipo</th>
          <th>Cantidad</th>
          <th>Monto ARS</th>
          <th>Fecha</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="t in transacciones" :key="t.id">
          <td>{{ t.id }}</td>
          <td>{{ t.cryptoCode.toUpperCase() }}</td>
          <td>{{ t.action === 'purchase' ? 'Compra' : 'Venta' }}</td>
          <td>{{ Number(t.cryptoAmount).toFixed(8) }}</td>
          <td>$ {{ t.money.toLocaleString('es-AR') }}</td>
          <td>{{ new Date(t.dateTime).toLocaleString('es-AR') }}</td>
          <td class="acciones">
            <button class="btn-ver" @click="verDetalle(t.id)">Ver</button>
            <button class="btn-editar" @click="editar(t.id)">Editar</button>
            <button class="btn-borrar" @click="confirmarBorrar(t.id)">Borrar</button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Modal de confirmacion para borrar la  trans. -->
    <div v-if="mostrarModal" class="overlay">
      <div class="modal">
        <h3>Confirmar Eliminacion</h3>
        <p>Esta accion no se puede deshacer.</p>
        <div class="modal-botones">
          <button class="btn-borrar" @click="borrar">Si,borrar</button>
          <button class="btn-cancelar" @click="cancelarBorrar">Cancelar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.contenedor {
  max-width: 900px;
  margin: 2rem auto;
  background: white;
  padding: 2rem;
  border-radius: 10px;
}

h2 {
  margin-bottom: 1.5rem;
  color: #1a1a2e;
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

th {
  background-color: #1a1a2e;
  color: white;
  padding: 0.7rem 1rem;
  text-align: left;
}

td {
  padding: 0.7rem 1rem;
  border-bottom: 1px solid #eee;
}

tr:hover {
  background-color: #f9f9f9;
}

.acciones {
  display: flex;
  gap: 0.5rem;
}

button {
  padding: 0.4rem 0.8rem;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 13px;
}

.btn-ver { background-color: #1a1a2e; color: white; }
.btn-editar { background-color: #e0b800; color: #1a1a2e; }
.btn-borrar { background-color: #c0392b; color: white; }
.btn-cancelar { background-color: #333; color: white; }
.error { color: red; }

.overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal {
  background: white;
  padding: 2rem;
  border-radius: 10px;
  max-width: 360px;
  width: 100%;
  text-align: center;
}

.modal h3 {
  margin-bottom: 0.5rem;
  color: #1a1a2e;
}

.modal p {
  margin-bottom: 1.5rem;
  color: #555;
}

.modal-botones {
  display: flex;
  gap: 1rem;
  justify-content: center;
}
</style>