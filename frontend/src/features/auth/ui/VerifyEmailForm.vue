<template>
    <form @submit.prevent="handleSubmit">
        <div class="flex justify-between gap-2 mb-6">
            <input
            v-for="(digit, idx) in otpDigits"
            :key="idx"
            :ref="(el) => setInputRef(el, idx)"
            v-model="otpDigits[idx]"
            type="text"
            inputmode="numeric"
            autocomplete="one-time-code"
            maxlength="1"
            placeholder="-"
            class="w-12 h-12 text-center text-xl font-bold border border-gray-200 rounded-xl focus:border-blue-600 focus:ring-4 focus:ring-blue-50 outline-none transition-all placeholder-gray-300"
            @input="handleInput($event, idx)"
            @keydown.delete="handleDelete($event, idx)"
            @paste="handlePaste"
            />
        </div>

        <div v-if="errors?.global" class="mb-4 animate-shake">
            <p class="text-sm text-red-600 text-center bg-red-50 p-2 rounded-lg">
            {{ errors.global[0] }}
            </p>
        </div>

        <BaseButton 
            :loading="isLoading" 
            class="w-full mb-6"
            type="submit"
            :disabled="model.code.length !== 6"
        >
            Подтвердить
        </BaseButton>

        <ResendCodeAction 
            :is-loading="resendLoading" 
            @resend="handleResendCode" 
         />
    </form>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';
import ResendCodeAction from './ResendCodeAction.vue'; 
import { useVerifyEmail } from '../model/useVerifyEmail';
import { BaseButton } from '@/shared/ui';

const { 
  isLoading, 
  resendLoading, 
  model, 
  errors, 
  handleVerifyEmail, 
  handleResendCode 
} = useVerifyEmail();

const otpDigits = ref(['', '', '', '', '', '']);
const inputRefs = [];

const setInputRef = (el, idx) => { if (el) inputRefs[idx] = el; };

onMounted(() => {
    setTimeout(() => inputRefs[0]?.focus(), 100);
});


onUnmounted(() => clearInterval(intervalId));

const handleInput = (event, idx) => {
  const input = event.target;
  let val = input.value.slice(-1); 
  val = val.replace(/\D/g, '');    

  otpDigits.value[idx] = val;

  if (val && idx < otpDigits.value.length - 1) {
    inputRefs[idx + 1].focus();
  }
  syncModel();
};

const handleDelete = (event, idx) => {
  if (!otpDigits.value[idx] && idx > 0) {
    inputRefs[idx - 1].focus();
  }
  syncModel();
};

const handlePaste = (event) => {
  event.preventDefault();
  const paste = event.clipboardData.getData('text').slice(0, 6);
  if (!/^\d+$/.test(paste)) return;
  
  paste.split('').forEach((char, i) => {
    if (i < otpDigits.value.length) {
        otpDigits.value[i] = char;
    }
  });
  
  const lastIdx = Math.min(paste.length, 5);
  inputRefs[lastIdx]?.focus();
  
  syncModel();
};

const syncModel = () => { model.code = otpDigits.value.join(''); };

const handleSubmit = () => {
  if (model.code.length === 6) handleVerifyEmail();
};

const resendCode = () => {
  console.log('Запрос на повторную отправку кода...');
  startTimer();
};
</script>

<style scoped>
.animate-shake {
  animation: shake 0.4s cubic-bezier(.36,.07,.19,.97) both;
}

@keyframes shake {
  10%, 90% { transform: translate3d(-1px, 0, 0); }
  20%, 80% { transform: translate3d(2px, 0, 0); }
  30%, 50%, 70% { transform: translate3d(-4px, 0, 0); }
  40%, 60% { transform: translate3d(4px, 0, 0); }
}
</style>