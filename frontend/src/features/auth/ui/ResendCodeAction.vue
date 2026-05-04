<template>
  <div class="text-sm text-center text-gray-400">
    Не получили код?
    <button 
      v-if="timer === 0"
      type="button"
      :disabled="isLoading"
      class="text-blue-600 font-semibold hover:underline ml-1 disabled:opacity-50 disabled:cursor-not-allowed" 
      @click="handleAction"
    >
      {{ isLoading ? 'Отправка...' : 'Отправить повторно' }}
    </button>
    
    <span v-else class="ml-1 text-gray-500 font-medium">
      Отправить снова через {{ timer }} сек.
    </span>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

const props = defineProps({
  isLoading: { type: Boolean, default: false },
  seconds: { type: Number, default: 60 }
});

const emit = defineEmits(['resend']);

const timer = ref(props.seconds);
let intervalId = null;

const startTimer = () => {
  timer.value = props.seconds;
  if (intervalId) clearInterval(intervalId);
  intervalId = setInterval(() => {
    if (timer.value > 0) timer.value--;
    else clearInterval(intervalId);
  }, 1000);
};

const handleAction = () => {
  emit('resend');
  startTimer(); 
};

onMounted(() => startTimer());
onUnmounted(() => clearInterval(intervalId));
</script>