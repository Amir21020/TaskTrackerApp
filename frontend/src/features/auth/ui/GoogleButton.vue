<script setup>
import { useGoogleLogin } from '../model/useGoogleLogin';
import { GoogleIcon } from '@/shared/assets'
import { GoogleLogin } from 'vue3-google-login';

const { handleGoogleLogin, isLoading, error } = useGoogleLogin();
</script>

<template>
    <div class="w-full mt-6">
        <GoogleLogin :callback="handleGoogleLogin" class="w-full">
            <button 
                :disabled="isLoading"
                type="button" 
                class="w-full flex items-center justify-center gap-3 px-4 py-2.5 bg-white border border-gray-300 rounded-lg text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-gray-200 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            >
                <span v-if="isLoading" class="flex items-center gap-2">
                    <svg class="animate-spin h-5 w-5 text-gray-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    Загрузка...
                </span>
                
                <!-- Обычное состояние -->
                <template v-else>
                    <GoogleIcon class="w-5 h-5" />
                    Войти через Google
                </template>
            </button>
        </GoogleLogin>

        <p v-if="error" class="mt-2 text-sm text-center text-red-500">{{ error }}</p>
    </div>
</template>