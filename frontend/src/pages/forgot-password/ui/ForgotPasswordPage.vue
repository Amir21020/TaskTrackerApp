<template>
    <auth-card title="Забыли пароль?" subtitle="Мы отправим ссылку для сброса пароля на вашу почту">
        <ForgotPasswordForm />
    </auth-card>
</template>

<script setup>
import { AuthCard } from '@/widgets/auth-card'
import { ForgotPasswordForm } from '@/features/auth';


</script>