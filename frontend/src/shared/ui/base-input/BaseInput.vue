<script setup>
const model = defineModel()
defineProps({
    label: String,
    error: String,
})

</script>

<template>
    <div>
        <label class="block text-sm font-medium text-gray-700">{{ label }}</label>
        <input
            :type="type"
            v-bind="$attrs"
            v-model="model"

            class="mt-1 w-full p-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 outline-none transition"
        />
        <p v-if="error" class="text-red-500 text-xs mt-1">{{ error }}</p>
    </div>
</template>