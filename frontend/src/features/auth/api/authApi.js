import { apiClient } from "@/shared/api/instance";

export const authApi = {
    signUp : (model) => apiClient.post("/auth/sign-up", model)
}