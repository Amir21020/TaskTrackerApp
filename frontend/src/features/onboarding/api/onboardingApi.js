import { apiClient } from "@/shared/api/instance";

export const onboardingApi = {
    completeOnboarding: (roleId) => apiClient.post('/users/me/onboarding', null, { params: { roleId } })
}
