<template>
    <div>
        <div v-if="isSuccess" class="space-y-5 animate-fade-in">
            <div class="p-4 bg-green-50 border border-green-200 text-green-700 rounded-lg text-sm text-center">
                Пароль успешно изменен! Теперь вы можете войти в систему.
            </div>
            
            <BaseButton class="w-full" @click="$router.push(ROUTES.LOGIN)">
                Вернуться к входу
            </BaseButton>
        </div>
        <form v-else @submit.prevent="handleResetPassword" class="space-y-5">
        
        <BaseInput 
            v-model="model.newPassword" 
            :error="errors?.NewPassword?.[0]" 
            label="Новый пароль" 
            type="text"
            required 
        />
        
        <password-input
            v-model="model.confirmPassword" 
            :error="errors?.ConfirmPassword?.[0]" 
            label="Подтвердите пароль" 
            type="password"
            required 
        />

        <div v-if="errors?.global" class="p-3 bg-red-50 border border-red-200 text-red-600 rounded-lg text-sm text-center">
            <ul class="list-none p-0 m-0">
            <li v-for="(error, index) in errors.global" :key="index">
                {{ error }}
            </li>
            </ul>
        </div>

        <BaseButton 
            type="submit" 
            :disabled="isLoading || errors?.global" 
            class="w-full"
        >
            {{ isLoading ? 'Сохранение...' : 'Сбросить пароль' }}
        </BaseButton>
        
        <div class="text-center text-sm pt-2">
            <RouterLink 
            :to="ROUTES.LOGIN" 
            class="text-blue-600 hover:text-blue-700 font-medium transition-colors"
            >
            Вернуться к входу
            </RouterLink>
        </div>
        </form>
    </div>
</template>
<script setup>

import { useResetPassword } from '../model/useResetPassword';
import { BaseInput, BaseButton, PasswordInput } from '@/shared/ui';
import { ROUTES } from '@/shared/routes';

const { 
  model, 
  isLoading, 
  isSuccess, 
  errors, 
  handleResetPassword 
} = useResetPassword();

</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.3s ease-in-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>