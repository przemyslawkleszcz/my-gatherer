import Vue from 'vue'
import Router from 'vue-router'

import Home from '@/components/Home'
import Editions from '@/components/Editions'
import Database from '@/components/Database'
import Inventory from '@/components/Inventory'
import Callback from '@/components/callback';
import { requireAuth } from '../../utils/auth';

Vue.use(Router);

import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.min.css'
import 'vue-material/dist/theme/default.css'
import VTooltip from 'v-tooltip'

Vue.use(VueMaterial);
Vue.use(VTooltip);

VTooltip.options.disposeTimeout = 0;

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'Home',
      component: Home
    },
    {
      path: '/Editions',
      name: 'Editions',
      component: Editions
    },
    {
      path: '/Database',
      name: 'Database',
      component: Database
    },
    {
      path: '/Inventory',
      name: 'Inventory',
      beforeEnter: requireAuth,
      component: Inventory
    },
    {
      path: '/callback',
      component: Callback
    }
  ]
})
