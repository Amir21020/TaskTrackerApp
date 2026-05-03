<script setup>
const model = defineModel()
defineProps({
    label: String
})


</script>

<template>
    <div>
        <label class="flex items-center cursor-pointer">
            <input
                type="checkbox"
                v-model="model"
                class="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
            />
            <span class="ml-2 text-sm text-gray-700">{{ label }}</span>
        </label>
    </div>
</template>