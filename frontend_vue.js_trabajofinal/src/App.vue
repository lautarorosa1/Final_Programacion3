<script setup>
</script>

<template>
  <div class="layout">
    <!-- SIDEBAR -->
    <aside class="sidebar">
      <nav>
        <router-link to="/">CryptoApp</router-link>
        <router-link to="/nuevo-cliente">Nuevo Cliente</router-link>
        <router-link to="/listado-clientes">Clientes</router-link>
        <router-link to="/nueva-transaccion">Nueva Transacción</router-link>
        <router-link to="/historial-movimientos">Historial</router-link>
      </nav>
    </aside>

    <!-- CONTENIDO PRINCIPAL -->
    <main class="contenido">
      <router-view />
    </main>
  </div>
</template>

<script setup>
import './assets/styles/main.css';
</script>