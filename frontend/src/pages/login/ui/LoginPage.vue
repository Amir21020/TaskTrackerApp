<script setup>
import { SignInForm, GoogleButton } from '@/features/auth'
import { AuthCard } from '@/widgets/auth-card'
import { Divider } from '@/shared/ui'
import { ROUTES } from '@/shared/config/routes' 
</script>

<template>
    <auth-card title="Войти">
        <SignInForm />
        
        <div class="mt-5 flex flex-col items-center gap-3 text-sm">          
            <div class="text-gray-600">
                Нет аккаунта? 
                <RouterLink :to="ROUTES.REGISTER" class="text-blue-600 hover:text-blue-700 hover:underline">
                    Зарегистрироваться
                </RouterLink>
            </div>
        </div>

        <Divider text="Или"  />

        <GoogleButton />
    </auth-card>
</template>