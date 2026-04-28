<template>
  <div class="w-full max-w-md bg-white p-8 rounded-xl select-none shadow-lg border border-gray-100">
    <h2 v-if="title" class="text-2xl font-bold mb-6 text-gray-800">
      {{ title }}
    </h2>
    
    <slot />
  </div>
</template>

<script setup>
defineProps({
  title: {
    type: String,
    default: ''
  }
});
</script>