import Vue from 'vue';
import Router, { Route } from 'vue-router';

import {store, bus, client, Authenticate} from './index';

import { Forbidden } from '@servicestack/vue';
import Home from '../components/Home/index.vue';
import About from '../components/About.vue';
import SignIn from '../components/SignIn.vue';
import SignUp from '../components/SignUp.vue';
import Profile from '../components/Profile.vue';
import Admin from '../components/Admin/index.vue';
import Create_StockXAccount from '@/Views/StockXAccount/StockXAccount/Create_StockXAccount.vue';
import Edit_StockXAccount from '@/Views/StockXAccount/StockXAccount/Edit_StockXAccount.vue';
import List_StockXAccounts from '@/Views/StockXAccount/StockXAccount/List_StockXAccount.vue';
import List_Inventorys from '@/Views/Inventory/Inventory/List_Inventory.vue';
import Edit_Inventory from '@/Views/Inventory/Inventory/Edit_Inventory.vue';
import Create_Inventory from '@/Views/Inventory/Inventory/Create_Inventory.vue';
import List_StockXListedItems from '@/Views/StockXListedItem/StockXListedItem/List_StockXListedItem.vue';
import Edit_StockXListedItem from '@/Views/StockXListedItem/StockXListedItem/Edit_StockXListedItem.vue';

export enum Routes {
  Home = '/',
  About = '/about',
  SignIn = '/signin',
  SignUp = '/signup',
  Profile = '/profile',
  Admin = '/admin',
  Forbidden = '/forbidden',
  Inventorys = '/Inventorys',
  EditInventorys = '/EditInventorys/:Id',
  CreateInventory = '/CreateInventory',
  //StockXListedItem
  StockXListedItems = '/StockXListedItems',
  EditStockXListedItems = '/EditStockXListedItems/:Id',
  CreateStockXListedItem = '/CreateStockXListedItem',
  StockXAccounts = '/StockXAccounts',
    EditStockXAccounts = '/EditStockXAccounts/:Id',
  CreateStockXAccount = '/CreateStockXAccount',
}

Vue.use(Router);


async function requiresAuth(to: Route, from: Route, next: (to?: string) => void) {

  if (!store.userSession) {

    const Me = await client.get(new Authenticate());

    if (Me.SessionId === undefined) {
      next(`${Routes.SignIn}?redirect=${encodeURIComponent(to.path)}`);
      return;
    }
    bus.$emit('signin', Me);

  }
  next();
}

function requiresRole(role: string) {
  return async (to: Route, from: Route, next: (to?: string) => void) => {
    if (!store.userSession) {

      const Me = await client.get(new Authenticate());

      if (Me.SessionId === undefined) {
        next(`${Routes.SignIn}?redirect=${encodeURIComponent(to.path)}`);
        return;
      }
      bus.$emit('signin', Me);
      next();
    } else if (!store.userSession.roles || store.userSession.roles.indexOf(role) < 0) {
      next(`${Routes.Forbidden}?role=${encodeURIComponent(role)}`);
      return;
    }
    next();

  };
}
const routes = [
  { path: Routes.Home, component: Home, beforeEnter: requiresAuth },
  { path: Routes.SignIn, component: SignIn },
  { path: Routes.Forbidden, component: Forbidden },

  // StockXAccount
  {path: Routes.StockXAccounts , component: List_StockXAccounts, beforeEnter: requiresAuth},
  {path: Routes.EditStockXAccounts , component: Edit_StockXAccount, beforeEnter: requiresAuth},
  {path: Routes.CreateStockXAccount, component: Create_StockXAccount, beforeEnter: requiresAuth},
  //StockXListedItem
  {path: Routes.StockXListedItems , component: List_StockXListedItems, beforeEnter: requiresAuth},
  {path: Routes.EditStockXListedItems , component: Edit_StockXListedItem, beforeEnter: requiresAuth},

  {path: Routes.Inventorys , component: List_Inventorys, beforeEnter: requiresAuth},
  {path: Routes.EditInventorys , component: Edit_Inventory, beforeEnter: requiresAuth},
  {path: Routes.CreateInventory, component: Create_Inventory, beforeEnter: requiresAuth},


  { path: '*', redirect: '/' },
];

export const router = new Router ({
    mode: 'history',
    linkActiveClass: 'active',
    routes,
});

export const redirect = (path: string) => {
  const externalUrl = path.indexOf('://') >= 0;
  if (!externalUrl) {
      router.push({ path });
  } else {
      location.href = path;
  }
};

bus.$on('signout', async () => {
  // reload current page after and run route guards after signing out.
  const to = router.currentRoute;
  router.replace('/');
  router.replace(to);
});
