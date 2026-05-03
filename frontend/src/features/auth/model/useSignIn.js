import { reactive, ref } from "vue"
import { authApi } from "../api/authApi"

export const useSignIn = () => {
    const form = reactive({
       email: '',
       password: '',
       rememberMe: false,
    })
    const errors = ref({ });
    const isLoading = ref(false);

    const handleSignIn = async () => {
        isLoading.value = true;
        errors.value = { };
        try {
            await authApi.signIn(form);
        } catch (err) {
            if(err.response?.status === 400 && err.response.data.errors) {
                errors.value = err.response.data.errors;
            }
            else{
                errors.value = { global: ['Ошибка сервера. Попробуйте позже.'] }
            }
        } finally {
            isLoading.value = false;
        }
    }

    return {
        form,
        errors,
        isLoading,
        handleSignIn
    }
}