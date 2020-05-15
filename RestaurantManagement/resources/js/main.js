require('./bootstrap');

import Vue from 'vue';

import App from './App.vue';

Vue.config.productionTip = false;


new Vue({
	data: function(){
	    return {
			success: false,
			message: null,
			orders: null,
			invoices: null,
	    }
	},
    render: h => h(App)
}).$mount('#app')


