import { createApp } from 'vue'

import App from './App.vue'
import router from './router'
import NumberPlugin from '@/plugins/NumberPlugin.js';

const app = createApp(App)

app.use(router)
app.use(NumberPlugin); // registrar plugin

app.mount('#app')
