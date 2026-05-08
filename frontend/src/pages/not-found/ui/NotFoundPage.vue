<template>
  <div class="text-center">
    <h1 class="text-6xl font-bold text-gray-300">404</h1>
    <p class="mt-2 text-gray-600">Страница не найдена</p>
    <RouterLink
      :to="ROUTES.HOME"
      class="mt-6 inline-block text-sm text-blue-600 hover:text-blue-700 hover:underline"
    >
      Вернуться на главную
    </RouterLink>
  </div>
</template>

<script setup>
import { ROUTES } from '@/shared/routes'
</script>