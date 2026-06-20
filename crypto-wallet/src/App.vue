<script setup>
import { ref } from 'vue'

const saldo = ref(parseFloat(localStorage.getItem('saldoArs')) || 0)

function agregarDinero() {
  const monto = prompt('Cuanto dinero queres agregar?')

  if (monto && parseFloat(monto) > 0) {
    saldo.value += parseFloat(monto)
    localStorage.setItem('saldoArs', saldo.value)
  }
}


</script>

<template>
  <nav>
    <div class="links">
      <RouterLink to="/historial">Historial</RouterLink>
      <RouterLink to="/comprar">Comprar</RouterLink>
      <RouterLink to="/vender">Vender</RouterLink>
      <RouterLink to="/portafolio">Portafolio</RouterLink>
    </div>

    <div class="saldo-info">
      <span class="saldo">$ {{ saldo.toLocaleString('es-AR') }}</span>
      <button class="btn-agregar" @click="agregarDinero">+ Agregar dinero</button>
    </div>
  </nav>

  <RouterView />
</template>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: Arial, sans-serif;
  background-color: #f5f5f5;
}

nav {
  background-color: #1a1a2e;
  padding: 1rem 2rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 1rem;
}

.links {
  display: flex;
  gap: 2rem;
}

nav a {
  color: white;
  text-decoration: none;
  font-size: 15px;
}

nav a:hover {
  color: #e0b800;
}

nav a.router-link-active {
  color: #e0b800;
  font-weight: bold;
}

.saldo-info {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.saldo {
  color: #4caf50;
  font-weight: bold;
  font-size: 15px;
}

.btn-agregar {
  background-color: #1d9e75;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 5px;
  cursor: pointer;
  font-size: 13px;
}

.btn-agregar:hover {
  background-color: #e0b800;
  color: #1a1a2e;
}
</style>