import { createApp } from 'vue'
import vue3GoogleLogin from 'vue3-google-login'
import './app/styles/index.css'
import { config } from '@/shared/config'
import { App, router } from './app'

const app = createApp(App)
console.log('Google Client ID:', config.googleClientId),

app.use(vue3GoogleLogin, {
  clientId: config.googleClientId
})

app.use(router).mount('#app')
