<script setup>
import { Loader } from '@lucide/vue'
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
                   <Loader class="w-5 h-5 animate-spin"  />
                    Загрузка...
                </span>
                
                <template v-else>
                    <GoogleIcon class="w-5 h-5" />
                    Войти через Google
                </template>
            </button>
        </GoogleLogin>

        <p v-if="error" class="mt-2 text-sm text-center text-red-500">{{ error }}</p>
    </div>
</template>