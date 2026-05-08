import { ref } from "vue";
import { useRouter } from 'vue-router'
import { authApi } from "../api/authApi";
import { useUserStore } from "@/entities/user"
import { ROUTES } from "@/shared/routes"

export const useGoogleLogin = () => {
    const { setUser } = useUserStore()
    const router = useRouter()
    const isLoading = ref(false);
    const error = ref(null);

    const handleGoogleLogin = async (response) => {
        isLoading.value = true;
        error.value = null;

        try{
            const res = await authApi.googleLogin({ code: response.code });
            setUser(res.data);
            router.push(ROUTES.ONBOARDING);
        } catch (err) {
            error.value = err.message;
        } finally {
            isLoading.value = false;
        }
    }

    return {
        isLoading,
        error,
        handleGoogleLogin
    };
}