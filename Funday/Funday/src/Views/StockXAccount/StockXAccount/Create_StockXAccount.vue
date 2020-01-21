<template>
    <div class="offset-2 col-8">
        <p v-if="Error.length > 0">{{Error}}</p>
        <b-row class="my-1" v-if="!Building">
            <b-col sm="2">
                <label for="input-Email">Email</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="email" id="input-Email" v-model="TransitObject.Email" placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1" v-if="!Building">
            <b-col sm="2">
                <label for="input-Password">Password</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="password" id="input-Password" v-model="TransitObject.Password"
                              placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-form-checkbox type="checkbox" v-model="Advanced" v-if="!Building">Advanced</b-form-checkbox>
        <div v-if="Advanced">
            <b-row class="my-1" v-if="!Building">
                <b-col sm="2">
                    <label for="input-ProxyUsername">Proxy Username</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="text" id="input-ProxyUsername" v-model="TransitObject.ProxyUsername"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1" v-if="!Building">
                <b-col sm="2">
                    <label for="input-ProxyPassword">Proxy Password</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="password" id="input-ProxyPassword" v-model="TransitObject.ProxyPassword"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1" v-if="!Building">
                <b-col sm="2">
                    <label for="input-ProxyHost">Proxy Host</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="text" id="input-ProxyHost" v-model="TransitObject.ProxyHost"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1" v-if="!Building">
                <b-col sm="2">
                    <label for="input-ProxyPort">Proxy Port</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="number" id="input-ProxyPort" v-model="TransitObject.ProxyPort"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
        </div>
        <b-row class="my-1" v-if="!Building">
            <b-col sm="2">
                <label for="input-Country">Country </label>
            </b-col>
            <b-col sm="10">
                <b-form-select id="input-Country" v-model="TransitObject.Country"
                               :options="[{text:'US',value:'US'},{text:'UK',value:'GB'}]"
                               placeholder=""></b-form-select>
            </b-col>
        </b-row>
        <div v-if="Building">Creating Account</div>
        <b-button block @click="CreateStockXAccount" v-if="!Building">Create</b-button>
        <a class="button btn-sm btn-danger" v-if="NewId>0" :href="'/EditStockXAccounts/' + NewId">New Item
            {{TransitObject.Email}}</a>. It may take upto 15 minutes for your account to be verified by our system.
    </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {client} from '@/shared';
import {CreateStockXAccountRequest} from '@/shared/dtos';


@Component({
    components: {},
})
export default class Create_StockXAccount extends Vue {
    NewId = -1;
    Error = '';
    Advanced = false;
    Building = false;
    Success = false;
    TransitObject =
        {
            Email: '',
            Password: '',
            ProxyUsername: '',
            ProxyPassword: '',
            ProxyHost: '',
            ProxyPort: 8000,
            ProxyActive: true,
            Active: true,
            CustomerID: 0,
            Currency: '',
            Country: 'US',
            UserAgent: '',
            Token: '',
        };

    async CreateStockXAccount() {
        try {
            this.Success = false;
            this.Building = true;
            this.Error = '';
            const Response = await client.post(new CreateStockXAccountRequest(this.TransitObject));
            if (Response.Success) {
                this.Success = true;
                this.NewId = Response.InsertedId;
            } else {
                this.Error = Response.Message;
            }
        } catch (e) {
            this.Error = e.message;
        }
        this.Building = false;
    }

}
</script>
