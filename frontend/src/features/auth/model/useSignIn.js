import { reactive, ref } from "vue"
import { useRouter } from 'vue-router'
import { authApi } from "../api/authApi"
import { useUserStore } from "@/entities/user"
import { ROUTES } from "@/shared/routes"

export const useSignIn = () => {
    const router = useRouter()
    const { setUser } = useUserStore()
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
            const response = await authApi.login(form);
            setUser(response.data);
            router.push(ROUTES.ONBOARDING);
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