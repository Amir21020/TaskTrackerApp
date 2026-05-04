import { reactive, ref } from 'vue'
import { authApi } from '../api/authApi'

export const useVerifyEmail = () => {
    const isLoading = ref(false)
    const model = reactive({
        email: '',
        code: ''
    })
    const errors = ref({})

    const handleVerifyEmail = async () => {
        isLoading.value = false
        errors.value = {}

        try{
            await authApi.verifyEmail(model)
        }
        catch(err){
            if(err.response.data.errors || err.response.status === 400){
                errors.value = err.response.data.errors
            }
            errors.value = { global: ['Ошибка сервера. Попробуйте позже.'] }
        }finally {
            isLoading.value = false
        }
    }

    const resendLoading = ref(false);

    const handleResendCode = async () => {
        if (!model.email) return;
        
        resendLoading.value = true;
        try {
            await authApi.resendVerificationCode(model.email);
        } catch (err) {
            errors.value = { global: ['Не удалось отправить код. Попробуйте позже.'] };
        } finally {
            resendLoading.value = false;
        }
    };

    return { isLoading, model, errors, handleVerifyEmail, handleResendCode, resendLoading }
}