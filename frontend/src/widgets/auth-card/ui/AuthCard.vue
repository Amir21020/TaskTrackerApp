<template>
  <div class="w-full max-w-md bg-white p-8 rounded-xl select-none shadow-lg border border-gray-100">    
    <h2 class="text-2xl font-bold mb-2 text-gray-900">
      {{ title }}
    </h2>
    <p class="text-sm text-gray-500 mb-8">
      {{ subtitle }}
    </p>
  
    
    <slot />
  </div>
</template>

<script setup>
defineProps({
  title: {
    type: String,
    default: ''
  },
  subtitle: {
    type: String,
    default: ''
  }
});
</script>