import axios from 'axios';
import { config } from '../config';
    
export const apiClient = axios.create({
    baseURL: config.apiUrl,
    withCredentials: true,
    headers: {
      'Content-Type': 'application/json'
    }
})

let isRefreshing = false
let failedQueue = []

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config

    if (error.response?.status == 401 && !originalRequest._retry){

      if(isRefreshing){
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject})
        }).then(() => apiClient(originalRequest))
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try{
        await apiClient.post('/auth/refresh')

        isRefreshing = false
        processQueue(null)

        return apiClient(originalRequest)
      }
      catch (refreshError) {
        isRefreshing = false;
        processQueue(refreshError);
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error)

  }
)

function processQueue(error) {
  failedQueue.forEach(prom => {
    if (error) prom.reject(error)
    else prom.resolve()
  })

  failedQueue = []
}