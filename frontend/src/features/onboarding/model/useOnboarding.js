import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { onboardingApi } from '../api/onboardingApi'
import { useUserStore } from '@/entities/user'
import { ROUTES } from '@/shared/routes'

export const useOnboarding = () => {
    const router = useRouter()
    const { completeOnboarding } = useUserStore()
    const selectedRoleId = ref(null)
    const isLoading = ref(false)
    const error = ref(null)
    const submitRole = async () => {
        if (!selectedRoleId.value) return
        isLoading.value = true
        error.value = null

        try {
            await onboardingApi.completeOnboarding(selectedRoleId.value)
            completeOnboarding()
            router.push(ROUTES.HOME)
        } catch (err) {
            error.value = err.response?.data?.message || 'Ошибка при сохранении'
        } finally {
            isLoading.value = false
        }
    }

    return { selectedRoleId, isLoading, error, submitRole }
}
