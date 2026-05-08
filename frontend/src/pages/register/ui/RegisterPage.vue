<template>
    <AuthCard title="Создать аккаунт">
        <p class="text-center text-sm mb-2 text-gray-600">
            Уже есть аккаунт? 
            <router-link to="/login" class="text-blue-600 hover:underline">Войти</router-link>
        </p>
        <SignUpForm />

        

        <Divider text="или" />


        <GoogleButton />

    </AuthCard>
</template>

<script setup>
import { AuthCard } from '@/widgets/auth-card'
import { SignUpForm, GoogleButton} from '@/features/auth'
import { Divider } from '@/shared/ui'
</script>