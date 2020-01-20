<template>

    <div class="col-12">
        <h3>Sign In</h3>

        <form @submit.prevent="submit" :class="{ error:responseStatus, loading }" >
            <div class="form-group">
                <error-summary except="userName,password" :responseStatus="responseStatus" />
            </div>
            <div class="form-group">
                <v-input id="userName" v-model="userName" placeholder="Username" :responseStatus="responseStatus"
                         label="Email"  help="Email you signed up with" />
            </div>
            <div class="form-group">
                <v-input type="password" id="password" v-model="password" placeholder="Password" :responseStatus="responseStatus"
                         label="Password"  help="6 characters or more" />
            </div>
            <div class="form-group">
                <v-checkbox id="rememberMe" v-model="rememberMe" :responseStatus="responseStatus">
                    Remember Me
                </v-checkbox>
            </div>
            <div class="form-group">
                <button type="submit" class="btn btn-lg btn-primary">Login</button>

            </div>
        </form>


    </div>


</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator';
import {store, bus, client, createCookie} from '../shared';
import { Authenticate } from '../shared/dtos';
import { redirect, Routes } from '../shared/router';

@Component
export class SignIn extends Vue {

    get store() { return store; }

    userName = '';
    password = '';
    rememberMe = true;
    loading = false;
    responseStatus = null;
    async CheckIfLoggedIn() {

        if (!store.userSession) {

            const Me = await client.get(new Authenticate());

            if (Me.SessionId !== undefined) {
                bus.$emit('signin', Me);
                redirect(this.$route.query.redirect as string || Routes.Dashboard);

            }

        }
    }

    protected mounted() {
        this.CheckIfLoggedIn();
    }

    protected async submit() {
        try {
            this.loading = true;
            this.responseStatus = null;

            const response = await client.post(new Authenticate({
                provider: 'credentials',
                UserName: this.userName,
                Password: this.password,
                RememberMe: this.rememberMe,
            }));

            if (response.BearerToken === undefined) {
                return;
            }
            client.setBearerToken(response.BearerToken);
            createCookie('token', response.BearerToken, 100);
            bus.$emit('signin', response);

            redirect(this.$route.query.redirect as string || Routes.Home);

        } catch (e) {
            this.responseStatus = e.responseStatus || e;
        } finally {
            this.loading = false;
        }
    }


}
export default SignIn;
</script>
