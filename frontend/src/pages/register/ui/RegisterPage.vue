<template>
    <AuthCard title="Создать аккаунт">
        <SignUpForm />

        <p class="mt-4 text-center text-sm text-gray-600">
            Уже есть аккаунт? 
            <router-link to="/login" class="text-blue-600 hover:underline">Войти</router-link>
        </p>

        <button 
            type="button" 
            class="w-full h-11 mt-4 flex items-center justify-center gap-3 bg-white border border-gray-300 rounded-xl text-gray-700 text-sm font-medium hover:bg-gray-50 transition-all"
        >
            <GoogleIcon class="w-5 h-5" />
            Войти через Google
        </button>

    </AuthCard>
</template>

<script setup>
import { GoogleIcon } from '@/shared/assets'
import { AuthCard } from '@/widgets/auth-card'
import { SignUpForm } from '@/features/auth'
</script>