<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="#">NavBar</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>

                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">
                    <b-nav-item-dropdown text="Listed" right    v-if="userSession">

                        <b-dropdown-item :to="Routes.StockXListedItems">List</b-dropdown-item>

                    </b-nav-item-dropdown>
                    <b-nav-item-dropdown text="StockX Accounts" right v-if="userSession">
                        <b-dropdown-item :to="Routes.CreateStockXAccount">Make</b-dropdown-item>
                        <b-dropdown-item :to="Routes.StockXAccounts">List</b-dropdown-item>

                    </b-nav-item-dropdown>
                    <b-nav-item-dropdown text="Inventory" right    v-if="userSession">
                        <b-dropdown-item :to="Routes.CreateInventory">Make</b-dropdown-item>
                        <b-dropdown-item :to="Routes.Inventorys">List</b-dropdown-item>

                    </b-nav-item-dropdown>
                    <b-nav-item-dropdown right v-if="userSession">
                        <!-- Using 'button-content' slot -->
                        <template v-slot:button-content>
                            <em>User</em>
                        </template>

                        <b-dropdown-item @click="SignOut">Sign Out</b-dropdown-item>
                    </b-nav-item-dropdown>
                    <b-nav-item v-else to="/signin" sm primary>Sign In</b-nav-item>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>

        <div id="content" class="container  ">
            <router-view></router-view>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import {Component, Prop} from 'vue-property-decorator';
import {bus, signout, store} from './shared';
import {Routes} from '@/shared/router';

@Component
export class App extends Vue {

    get userSession() {

        return store.userSession;
    }
    get SignOut(){
        return signout;
    }
    get Routes() {
        return Routes;
    }

    get store() {
        return store;
    }
}

export default App;
</script>
