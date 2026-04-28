<script setup>
import { Eye, EyeOff } from '@lucide/vue'
import { ref } from 'vue'
import BaseInput from '../base-input/BaseInput.vue';

const isVisible = ref(false)
const model = defineModel()

defineProps({
    label: String,
})
</script>

<template>
    <div class="relative">
        <BaseInput
            :type="isVisible ? 'text' : 'password'"
            :label=label
            v-model="model"
            v-bind="$attrs"
        />
        <button
            type="button"
            @click="isVisible = !isVisible"
            class="absolute top-1/2 right-2 text-gray-500 hover:text-gray-700 transition-colors"
            >
            <component :is="isVisible ? EyeOff : Eye" />
        </button>
    </div>
</template>