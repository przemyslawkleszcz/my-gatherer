import Vue from 'vue'
import Router from 'vue-router'
import Editions from '@/components/Editions'

Vue.use(Router)

import VueMaterial from 'vue-material'
import 'vue-material/dist/vue-material.min.css'
import 'vue-material/dist/theme/default.css'
import VTooltip from 'v-tooltip'

Vue.use(VueMaterial);
Vue.use(VTooltip);

VTooltip.options.disposeTimeout = 0;

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Editions',
      component: Editions
    }
  ]
})
