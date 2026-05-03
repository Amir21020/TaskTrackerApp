import { onMounted, reactive, ref } from "vue"
import { useRoute } from 'vue-router'
import { authApi } from "../api/authApi"

export const useResetPassword = () => {
    const route = useRoute()
    const model = reactive({
        email: '',
        token: '',
        newPassword: '',
        confirmPassword: ''
    })
    
    const isLoading = ref(false)
    const isSuccess = ref(false) 
    const errors = ref({})

    onMounted(() => {
        model.email = route.query.email || ''
        model.token = route.query.token || ''
        
        if (!model.token || !model.email) {
            errors.value = { global: ["Неверная или устаревшая ссылка для сброса."] }
        }
    })

    const handleResetPassword = async () => {
        if (!model.token || !model.email) return;

        isLoading.value = true
        errors.value = {}
        isSuccess.value = false

        try {
            await authApi.resetPassword(model)
            isSuccess.value = true 
        }
        catch(err) {
            if (err.response?.data?.errors) {
                errors.value = err.response.data.errors
            } else if (err.response?.status === 400 || err.response?.status === 401) {
                const message = err.response.data.message || 'Ссылка недействительна или устарела.';
                errors.value = { global: [message] }
            } else {
                errors.value = { global: ['Ошибка сервера. Попробуйте позже.'] }
            }
        }
        finally {
            isLoading.value = false
        }
    }

    return {
        handleResetPassword,
        isLoading,
        isSuccess, 
        model,
        errors
    }
}