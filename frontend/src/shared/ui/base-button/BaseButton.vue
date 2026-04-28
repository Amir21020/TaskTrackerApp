<script setup>

defineProps({
    type: {
        type: String,
        default: 'button'
    },
    disabled: {
        type: Boolean,
        default: false
    }
})


</script>

<template>
    <button
        :type=type
        :disabled=disabled
        class="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition duration-200 font-semibold focus:ring-4 focus:ring-blue-200 disabled:opacity-50"
    >
        <slot />
    </button>
</template>