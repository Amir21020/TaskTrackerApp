<template>
  <button
    type="button"
    @click="$emit('select', role.id)"
    :class="[
      'w-full flex items-center justify-between p-4 rounded-2xl border-2 transition-all duration-200 group',
      isActive 
        ? 'border-blue-600 bg-blue-50/50 shadow-sm' 
        : 'border-gray-50 bg-gray-50 hover:border-gray-200'
    ]"
  >
    <div class="flex items-center gap-4 text-left">
      <img v-if="role.iconUrl" :src="role.iconUrl" class="w-8 h-8 object-contain" :alt="role.label" />
      <span v-else class="text-2xl">{{ role.icon }}</span>
      
      <div class="flex flex-col">
        <span :class="['font-bold text-base transition-colors', isActive ? 'text-blue-700' : 'text-gray-700']">
          {{ role.label }}
        </span>
        <span v-if="role.description" class="text-xs text-gray-400 leading-tight">
          {{ role.description }}
        </span>
      </div>
    </div>
    
    <div 
      class="w-7 h-7 flex min-w-[28px] items-center justify-center rounded-full border-2 transition-all"
      :class="isActive ? 'bg-blue-600 border-blue-600' : 'border-gray-300 bg-white'"
    >
      <Check 
        v-if="isActive" 
        :size="16" 
        stroke-width="4" 
        class="text-white" 
      />
    </div>
  </button>
</template>

<script setup>

import { Check } from '@lucide/vue'

defineProps({
  role: { type: Object, required: true },
  isActive: { type: Boolean, default: false }
});

defineEmits(['select']);
</script>