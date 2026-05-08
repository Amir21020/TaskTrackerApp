import { apiClient } from '@/shared/api/instance'

export const roleApi = {
    getRoles: () => apiClient.get('/roles')   
}