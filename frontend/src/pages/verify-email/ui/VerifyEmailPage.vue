<template>
    <auth-card title="Подтвердите email" subtitle="Введите код, отправленный на вашу почту"> 
        <VerifyEmailForm />
    </auth-card>
</template>

<script setup>

import { AuthCard } from '@/widgets/auth-card'
import { VerifyEmailForm } from '@/features/auth'

</script>