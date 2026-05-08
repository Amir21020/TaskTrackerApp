<template>
  <div class="space-y-6"> 

    <form @submit.prevent="handleSubmit" class="space-y-5">
      <div v-if="isSuccess" class="p-4 bg-green-50 text-green-700 rounded-lg text-sm text-center">
        Письмо отправлено на вашу почту.
      </div>

      <div v-else class="space-y-5">
        <BaseInput 
          v-model="model.email" 
          :error="errors?.Email?.[0]" 
          label="Email" 
          type="email"
          placeholder="example@mail.com"
          required 
          autocomplete="email" 
        />
        
        <BaseButton type="submit" :disabled="isLoading" class="w-full">
          {{ isLoading ? 'Отправка...' : 'Отправить ссылку' }}
        </BaseButton>
      </div>

      <div class="text-center text-sm">
        <RouterLink 
          :to="ROUTES.LOGIN" 
          class="text-blue-600 hover:text-blue-700 font-medium hover:underline transition-colors"
        >
          Вернуться к входу
        </RouterLink>
      </div>
    </form>
  </div>
</template>

<script setup>
import { BaseInput, BaseButton } from '@/shared/ui';
import { useForgotPassword } from '../model/useForgotPassword';
import { ROUTES } from '@/shared/routes'
const { isLoading, handleForgotPassword, model, errors, isSuccess } = useForgotPassword();

const handleSubmit = async () => {
  await handleForgotPassword();
};

</script>