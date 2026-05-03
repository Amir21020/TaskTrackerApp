<template>
    <AuthCard title="Новый пароль" subtitle="Установите новый надежный пароль для вашей учетной записи.">
        <ResetPasswordForm />
    </AuthCard>
</template>

<script setup>

import { ResetPasswordForm } from '@/features/auth';
import { AuthCard } from '@/widgets/auth-card'


</script>