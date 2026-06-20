<script setup>
import { ref } from 'vue'

const criptos = ['btc', 'eth', 'usdc']
const cryptoCode = ref('btc')
const cryptoAmountdeVenta = ref('')
const dateTime = ref('')
const mensaje = ref('')
const error = ref('')

async function enviar() {
  mensaje.value = ''
  error.value = ''

  if (!cryptoAmountdeVenta.value || cryptoAmountdeVenta .value <= 0) {
    error.value = 'La cantidad tiene que ser mayor a 0'
    return
  }
  if (!dateTime.value) {
    error.value = 'Ingresa la fecha y hora'
    return
  }

  const guardavalorVenta = {
    cryptoCode: cryptoCode.value,
    action: 'sale',
    CryptoAmount: parseFloat(cryptoAmountdeVenta.value),
    dateTime: dateTime.value
  }

  try {
    const res = await fetch('https://localhost:7278/api/transacciones', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(guardavalorVenta)
    })

    if (!res.ok) {
      error.value = 'Error al guardar la venta'
      return
    }

    mensaje.value = 'Venta registrada correctamente'
    cryptoAmountdeVenta.value = ''
    dateTime.value = ''

  } catch (e) {
    error.value = 'No se puede conectar con el servidor'
  }
}
</script>

<template>
  <div class="contenedor">
    <h2>Vender Criptomoneda</h2>

    <div class="campo">
      <label>Criptomoneda</label>
      <select v-model="cryptoCode">
        <option v-for="c in criptos" :key="c" :value="c">{{ c.toUpperCase() }}</option>
      </select>
    </div>

    <div class="campo">
      <label>Cantidad</label>
      <input type="number" step="0.00000001" v-model="cryptoAmountdeVenta" placeholder="0.00000001" />
    </div>

    <div class="campo">
      <label>Fecha y hora</label>
      <input type="datetime-local" v-model="dateTime" />
    </div>

    <button @click="enviar">Confirmar Venta</button>

    <p v-if="mensaje" class="exito">{{ mensaje }}</p>
    <p v-if="error" class="error">{{ error }}</p>
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

h2 { margin-bottom: 1.5rem; color: #1a1a2e; }

.campo {
  margin-bottom: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

label { font-size: 14px; color: #555; }

input, select {
  padding: 0.6rem;
  border: 1px solid #ccc;
  border-radius: 6px;
  font-size: 14px;
}

button {
  margin-top: 1rem;
  width: 100%;
  padding: 0.8rem;
  background-color: #000000;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 15px;
  cursor: pointer;
}

button:hover { background-color: #e0b800; color: #1a1a2e; }
.exito { margin-top: 1rem; color: green; }
.error { margin-top: 1rem; color: red; }
</style>