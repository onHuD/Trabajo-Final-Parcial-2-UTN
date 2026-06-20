<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const transaccion = ref(null)
const error = ref('')

async function cargar() {
  try {
    const res = await fetch(`https://localhost:7278/api/transacciones/${route.params.id}`)
    if (!res.ok) {
      error.value = 'No se encontro la transaccion'
      return
    }
    transaccion.value = await res.json()
  } catch (e) {
    error.value = 'No se pudo conectar con el servidor'
  }
}

onMounted(() => {
  cargar()
})
</script>

<template>
  <div class="contenedor">
    <h2>Detalle de Transaccion</h2>

    <p v-if="error" class="error">{{ error }}</p>

    <div v-if="transaccion" class="detalle">
      <div class="fila">
        <span class="etiqueta">ID</span>
        <span>{{ transaccion.id }}</span>
      </div>
      <div class="fila">
        <span class="etiqueta">Criptomoneda</span>
        <span>{{ transaccion.cryptoCode.toUpperCase() }}</span>
      </div>
      <div class="fila">
        <span class="etiqueta">Tipo</span>
        <span>{{ transaccion.action === 'purchase' ? 'Compra' : 'Venta' }}</span>
      </div>
      <div class="fila">
        <span class="etiqueta">Cantidad</span>
        <span>{{ transaccion.cryptoAmount }}</span>
      </div>
      <div class="fila">
        <span class="etiqueta">Monto ARS</span>
        <span>$ {{ transaccion.money.toLocaleString('es-AR') }}</span>
      </div>
      <div class="fila">
        <span class="etiqueta">Fecha y hora</span>
        <span>{{ new Date(transaccion.dateTime).toLocaleString('es-AR') }}</span>
      </div>
    </div>

    <button @click="router.push('/historial')">Volver al historial</button>
  </div>
</template>

<style scoped>
.contenedor {
  max-width: 480px;
  margin: 2rem auto;
  background: white;
  padding: 2rem;
  border-radius: 10px;
}

h2 {
  margin-bottom: 1.5rem;
  color: #1a1a2e;
}

.detalle {
  margin-bottom: 1.5rem;
}

.fila {
  display: flex;
  justify-content: space-between;
  padding: 0.7rem 0;
  border-bottom: 1px solid #eee;
  font-size: 14px;
}

.etiqueta {
  color: #555;
  font-weight: bold;
}

button {
  width: 100%;
  padding: 0.8rem;
  background-color: #1a1a2e;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 15px;
  cursor: pointer;
}

button:hover {
  background-color: #e0b800;
  color: #1a1a2e;
}

.error { color: red; }
</style>