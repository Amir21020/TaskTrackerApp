<template>
  <div class="space-y-3 mb-10">
    <RoleSelectionItem
      v-for="role in roles"
      :key="role.id"
      :role="role"
      :is-active="modelValue === role.id"
      @select="$emit('update:modelValue', $event)"
    />
  </div>
</template>

<script setup>
import RoleSelectionItem from './RoleSelectionItem.vue';

defineProps({
  roles: { type: Array, required: true },
  modelValue: { type: [String, Number, null], default: null }
});

defineEmits(['update:modelValue']);
</script>