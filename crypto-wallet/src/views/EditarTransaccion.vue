<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const criptos = ['btc', 'eth', 'usdc']
const cryptoCode = ref('')
const accion = ref('')
const cryptoAmount = ref('')
const money = ref('')
const dateTime = ref('')
const mensaje = ref('')
const error = ref('')

async function cargar() {
  try {
    const res = await fetch(`https://localhost:7278/api/transacciones/${route.params.id}`)
    if (!res.ok) {
      error.value = 'No se encontro la transacción'
      return
    }
    const data = await res.json()
    cryptoCode.value = data.cryptoCode
    accion.value = data.action
    cryptoAmount.value = data.cryptoAmount
    money.value = data.money
    dateTime.value = data.dateTime.slice(0, 16)
  } catch (e) {
    error.value = 'No se puede conectar con el servidor'
  }
}

async function guardar() {
  mensaje.value = ''
  error.value = ''

  if (!cryptoAmount.value || cryptoAmount.value <= 0) {
    error.value = 'La cantidad tiene que ser mayor a 0'
    return
  }

  const body = {
    cryptoCode: cryptoCode.value,
    action: accion.value,
    cryptoAmount: parseFloat(cryptoAmount.value),
    money: parseFloat(money.value),
    dateTime: dateTime.value
  }
 
  try {
    const res = await fetch(`https://localhost:7278/api/transacciones/${route.params.id}`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(body)
    })

    if (!res.ok) {
      error.value = 'Hubo un error no se guardaron los cambios'
      return
    }

    mensaje.value = 'La transaccion fue actualizada'
    setTimeout(() => router.push('/historial'), 1500)

  } catch (e) {
    error.value = 'No se puede conectar con el servidor'
  }
}

onMounted(() => {
  cargar()
})
</script>

<template>
  <div class="contenedor">
    <h2>Editar Transaccion</h2>

    <p v-if="error" class="error">{{ error }}</p>

    <div class="campo">
      <label>Tipo</label>
      <select v-model="accion">
        <option value="purchase">Compra</option>
        <option value="sale">Venta</option>
      </select>
    </div>

    <div class="campo">
      <label>Criptomoneda</label>
      <select v-model="cryptoCode">
        <option v-for="c in criptos" :key="c" :value="c">{{ c.toUpperCase() }}</option>
      </select>
    </div>

    <div class="campo">
      <label>Cantidad</label>
      <input type="number" step="0.00000001" v-model="cryptoAmount" />
    </div>

    <div class="campo">
      <label>Monto ARS</label>
      <input type="number" step="0.01" v-model="money" />
    </div>

    <div class="campo">
      <label>Fecha y hora</label>
      <input type="datetime-local" v-model="dateTime" />
    </div>

    <div class="botones">
      <button class="btn-guardar" @click="guardar">Guardar cambios</button>
      <button class="btn-cancelar" @click="router.push('/historial')">Cancelar</button>
    </div>

    <p v-if="mensaje" class="exito">{{ mensaje }}</p>
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

.campo {
  margin-bottom: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

label {
  font-size: 14px;
  color: #555;
}

input, select {
  padding: 0.6rem;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 14px;
}

.botones {
  display: flex;
  gap: 1rem;
  margin-top: 1rem;
}

button {
  flex: 1;
  padding: 0.8rem;
  border: none;
  border-radius: 6px;
  font-size: 15px;
  cursor: pointer;
}

.btn-guardar {
  background-color: #1a1a2e;
  color: white;
}

.btn-guardar:hover {
  background-color: #e0b800;
  color: #1a1a2e;
}

.btn-cancelar {
  background-color: #1a1a2e;
  color: white;
}

.btn-cancelar:hover {
  background-color: #e0b800;
  color: #1a1a2e;
}
.exito { color: green; margin-top: 1rem; }
.error { color: red; margin-bottom: 1rem; }
</style>