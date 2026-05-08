<template>
  <div class="space-y-4">
    <RoleSelectionList v-model="selectedRoleId" :roles="roles" />

    <BaseButton
      :disabled="!selectedRoleId || isLoading"
      class="w-full py-4 text-lg font-bold rounded-2xl bg-blue-600 hover:bg-blue-700 text-white shadow-lg"
      @click="submitRole"
    >
      {{ isLoading ? 'Сохранение...' : 'Завершить настройку' }}
    </BaseButton>

    <p v-if="error" class="text-red-500 text-sm text-center">{{ error }}</p>
  </div>
</template>

<script setup>
import { BaseButton } from '@/shared/ui';
import { RoleSelectionList, useRole } from '@/entities/role'; 
import { useOnboarding } from '../model/useOnboarding'; 

const { roles } = useRole(); 
const { selectedRoleId, isLoading, error, submitRole } = useOnboarding();
</script>
