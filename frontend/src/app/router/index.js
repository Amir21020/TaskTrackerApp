import { ROUTES } from '@/shared/routes'
import { createRouter, createWebHistory } from 'vue-router'
import { useUserStore } from '@/entities/user'

const routes = [
    { path: ROUTES.HOME, component: () => import('@/pages/home').then(m => m.HomePage) },
    { path: ROUTES.REGISTER, component: () => import('@/pages/register').then(m => m.RegisterPage) },
    { path: ROUTES.LOGIN, component: () => import('@/pages/login').then(m => m.LoginPage) },
    { path: ROUTES.FORGOT_PASSWORD, component: () => import('@/pages/forgot-password').then(m => m.ForgotPasswordPage) },
    { path: ROUTES.RESET_PASSWORD, component: () => import('@/pages/reset-password').then(m => m.ResetPasswordPage )},
    { path: ROUTES.VERIFY_EMAIL, component: () => import('@/pages/verify-email').then(m => m.VerifyEmailPage) },
    { path: ROUTES.ONBOARDING, component: () => import('@/pages/onboarding').then(m => m.OnBoardingPage) },
    { path: '/:pathMatch(.*)*', component: () => import('@/pages/not-found').then(m => m.NotFoundPage)  }
]

export const router = createRouter({
    history: createWebHistory(),
    routes,
})

const publicRoutes = [
    ROUTES.LOGIN,
    ROUTES.REGISTER,
    ROUTES.FORGOT_PASSWORD,
    ROUTES.RESET_PASSWORD,
    ROUTES.VERIFY_EMAIL,
]

router.beforeEach((to) => {
    const store = useUserStore()

    if (publicRoutes.includes(to.path)) return true

    if (!store.user) return ROUTES.LOGIN

    if (to.path === ROUTES.ONBOARDING && store.onboardingCompleted) {
        return ROUTES.HOME
    }

    if (to.path === ROUTES.HOME && !store.onboardingCompleted) {
        return ROUTES.ONBOARDING
    }

    return true
})