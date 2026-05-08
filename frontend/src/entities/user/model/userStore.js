import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useUserStore = defineStore('user', () => {
    const user = ref()
    const onboardingCompleted = ref(localStorage.getItem('onboardingCompleted') === 'true')

    const isAuthenticated = computed(() => !!user.value)

    function setUser(userData) {
        user.value = userData
    }

    function completeOnboarding() {
        onboardingCompleted.value = true
        localStorage.setItem('onboardingCompleted', 'true')
    }

    function clear() {
        user.value = null
        onboardingCompleted.value = false
        localStorage.removeItem('onboardingCompleted')
    }

    return { user, onboardingCompleted, isAuthenticated, setUser, completeOnboarding, clear }
})