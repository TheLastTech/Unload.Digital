import 'bootstrap/dist/css/bootstrap.css';
import './app.scss';
import 'es6-shim';
import '@/components/hyper/assets/css/main.css';
import Vue from 'vue';
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue';

// Install BootstrapVue
Vue.use(BootstrapVue);
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin);
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import Controls from '@servicestack/vue';
Vue.use(Controls);

import App from './App.vue';

import { router } from './shared/router';

const app = new Vue({
    el: '#app',
    render: (h) => h(App),
    router,
});
