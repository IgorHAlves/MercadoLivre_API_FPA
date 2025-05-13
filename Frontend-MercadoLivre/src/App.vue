<script setup>
import { ref } from "vue";

import Navbar from "./components/NavbarComponent.vue";
</script>

<template>
  <Navbar />
  <router-view> </router-view>
</template>
