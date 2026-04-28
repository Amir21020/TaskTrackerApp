import { authApi } from '../api/authApi'
import { ref, reactive } from 'vue'

export const useSignUp = () => {
    const isLoading = ref(false)
    const errors = ref({}) 

    const form = reactive({
        email: '',
        firstName: '',
        lastName: '',
        password: '',
        confirmPassword: '',
    })

    const handleRegister = async () => {
        isLoading.value = true
        errors.value = {}

        try {
            await authApi.signUp(form)
            form.email = ''
            form.firstName = ''
            form.lastName = ''
            form.password = ''
            form.confirmPassword = ''
        } catch (err) {
            if (err.response?.status === 400 && err.response.data.errors) {
                errors.value = err.response.data.errors
            } else {
                errors.value = { global: ['Ошибка сервера. Попробуйте позже.'] }
            }
        } finally {
            isLoading.value = false
        }
    }

    return { form, isLoading, errors, handleRegister }
}