import { createApp } from 'vue'
import './app/styles/index.css'
import { App, router } from './app'

createApp(App).use(router).mount('#app')
