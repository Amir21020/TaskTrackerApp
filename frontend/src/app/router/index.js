import { ROUTES } from '@/shared/config/routes'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    { path: ROUTES.REGISTER, component: () => import('@/pages/register').then(m => m.RegisterPage) },
    { path: ROUTES.LOGIN, component: () => import('@/pages/login').then(m => m.LoginPage) },
    { path: ROUTES.FORGOT_PASSWORD, component: () => import('@/pages/forgot-password').then(m => m.ForgotPasswordPage) },
    { path: ROUTES.RESET_PASSWORD, component: () => import('@/pages/reset-password').then(m => m.ResetPasswordPage )},
    { path: ROUTES.VERIFY_EMAIL, component: () => import('@/pages/verify-email').then(m => m.VerifyEmailPage) }
]


export const router = createRouter({
    history: createWebHistory(),
    routes,
})