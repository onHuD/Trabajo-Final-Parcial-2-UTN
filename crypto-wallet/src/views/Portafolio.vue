<script setup>
import { ref, onMounted } from 'vue'

const items = ref([])
const total = ref(0)
const error = ref('')
const cargando = ref(true)
const ultimaActualizacion = ref('')

async function cargar() {
  cargando.value = true
  error.value = ''
  try {
    const res = await fetch('https://localhost:7278/api/portfolio')
    if (!res.ok) {
      error.value = 'Error al cargar el portafolio'
      return
    }
    const data = await res.json()
    items.value = data.items
    total.value = data.total
    ultimaActualizacion.value = new Date().toLocaleTimeString('es-AR')
  } catch (e) {
    error.value = 'No se puede conectar con el servidor'
  } finally {
    cargando.value = false
  }
}

onMounted(() => {
  cargar()
})
</script>

<template>
  <div class="contenedor">
    <h2>Estado actual del Portafolio</h2>

    <p v-if="cargando">Actualizando precios....</p>
    <p v-if="error" class="error">{{ error }}</p>

    <div v-if="!cargando && !error">
      <p v-if="items.length === 0">No tenes criptomonedas en el portafolio</p>

      <table v-if="items.length > 0">
        <thead>
          <tr>
            <th>Criptomoneda</th>
            <th>Cantidad</th>
            <th>Precio actual</th>
            <th>Valor en ARS</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in items" :key="item.cryptoCode">
            <td>{{ item.cryptoCode.toUpperCase() }}</td>
            <td>{{ item.cantidad }}</td>
            <td>$ {{ item.precioActual.toLocaleString('es-AR') }}</td>
            <td>$ {{ item.valorEnArs.toLocaleString('es-AR') }}</td>
          </tr>
        </tbody>
        <tfoot>
          <tr>
            <td colspan="3" class="total-label">Total</td>
            <td class="total-valor">$ {{ total.toLocaleString('es-AR') }}</td>
          </tr>
        </tfoot>
      </table>

      <div class="pie">
        <p v-if="ultimaActualizacion" class="actualizacion">
          Ultima actualizacion: {{ ultimaActualizacion }}
        </p>
        <button @click="cargar" class="btn-actualizar">Actualizar precios</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.contenedor {
  max-width: 700px;
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
  margin-bottom: 1.5rem;
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

tfoot tr {
  background-color: #f0f0f0;
}

.total-label {
  font-weight: bold;
  color: #1a1a2e;
}

.total-valor {
  font-weight: bold;
  color: #1a1a2e;
}

.pie {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}

.actualizacion {
  font-size: 13px;
  color: #555;
}

.btn-actualizar {
  padding: 0.8rem 1.5rem;
  background-color: #1a1a2e;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 15px;
  cursor: pointer;
}

.btn-actualizar:hover {
  background-color: #e0b800;
  color: #1a1a2e;
}

.error { color: red; }
</style>