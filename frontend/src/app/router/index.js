import { ROUTES } from '@/shared/config/routes'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    { path: ROUTES.REGISTER, component: () => import('@/pages/register').then(m => m.RegisterPage) },
]


export const router = createRouter({
    history: createWebHistory(),
    routes,
})