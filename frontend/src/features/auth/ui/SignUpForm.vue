<template>
  <form @submit.prevent="handleRegister" class="space-y-4 select-none">
    <div class="grid grid-cols-2 gap-4">
      <BaseInput v-model="form.firstName" :error="errors.FirstName?.[0]"  label="Имя" required autocomplete="given-name" />
      <BaseInput v-model="form.lastName" :error="errors.LastName?.[0]" label="Фамилия" required autocomplete="family-name" />
    </div>

    <BaseInput v-model="form.email" :error="errors.Email?.[0]" label="Email" type="email" required autocomplete="email" />

    <BaseInput v-model="form.password" :error="errors.Password?.[0]" label="Пароль" required autocomplete="new-password" />
    
    <PasswordInput v-model="form.confirmPassword" :error="errors.ConfirmPassword?.[0]" label="Подтвердите пароль" required autocomplete="new-password" />

    <BaseButton :disabled="isLoading" type="submit">Зарегистрироваться</BaseButton>
  </form>
</template>

<script setup>
import { BaseInput, PasswordInput, BaseButton } from '@/shared/ui'
import { useSignUp } from '../model/useSignUp';
const { form, handleRegister, isLoading, errors } = useSignUp();

</script>