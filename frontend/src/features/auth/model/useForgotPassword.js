import { reactive, ref } from "vue"
import { authApi } from "../api/authApi"

export const useForgotPassword = () => {
    const isSuccess = ref(false)
    const model = reactive({
        email: ''
    })
    const errors = ref({})
    const isLoading = ref(false)

    const handleForgotPassword = async () => {
        isLoading.value = true
        errors.value = {}
        try{
            await authApi.forgotPassword(model.email)
            isSuccess.value = true
        } 
        catch (err) {
            if(err.response?.status === 400 && error.response.data.errors){
                errors.value = err.response.data.errors
            }
            else{
                errors.value = { global: ['Ошибка сервера. Попробуйте позже.'] }
            }
        }
        finally{
            isLoading.value = false
        }
    }
    return { 
        isLoading,
        isSuccess,
        handleForgotPassword, 
        model, 
        errors
    }
}