import { ref } from "vue";
import { authApi } from "../api/authApi";

export const useGoogleLogin = () => {
    const isLoading = ref(false);
    const error = ref(null);

    const handleGoogleLogin = async (response) => {
        isLoading.value = true;
        error.value = null;

        try{
            const { code } = response;
            await authApi.googleLogin(code);      
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