import { apiClient } from "@/shared/api/instance";

export const authApi = {
    signUp : (model) => apiClient.post("/auth/sign-up", model),
    googleLogin: (model) => apiClient.post("/auth/google-login", model),
    login: (model) => apiClient.post("/auth/login", model),
}