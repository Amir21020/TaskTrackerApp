<template>
  <form @submit.prevent="handleSignIn" class="space-y-5"> 
    
    <div v-if="errors.General" class="p-3 text-sm text-red-600 bg-red-50 rounded-md border border-red-200">
      {{ errors.General[0] }}
    </div>

    <BaseInput 
      v-model="form.email" 
      :error="errors.Email?.[0]" 
      label="Email" 
      type="email"
      autocomplete="email" 
    />

    <div>
      <PasswordInput 
        v-model="form.password" 
        :error="errors.Password?.[0]" 
        label="Пароль" 
        required 
        autocomplete="current-password" 
      />
     
      <div class="flex justify-between items-center mt-2">
        <BaseCheckbox label="Запомнить меня" v-model="form.rememberMe"></BaseCheckbox>      
        <RouterLink 
          :to="ROUTES.FORGOT_PASSWORD" 
          class="text-sm text-blue-600 hover:text-blue-700 hover:underline"
        >
          Забыли пароль?
        </RouterLink>
      </div>
    </div>

    <BaseButton type="submit" :disabled="isLoading" class="w-full">
      {{ isLoading ? 'Вход...' : 'Войти' }}
    </BaseButton>
  </form>
</template>

<script setup>
import { useSignIn } from '../model/useSignIn';
import { BaseInput, BaseButton, PasswordInput, BaseCheckbox } from '@/shared/ui';
import { ROUTES } from '@/shared/config/routes'

const { handleSignIn, form, errors, isLoading } = useSignIn(); 
</script>