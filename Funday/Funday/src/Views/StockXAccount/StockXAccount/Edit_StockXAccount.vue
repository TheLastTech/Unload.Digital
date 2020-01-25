<template>
    <div class="  ">
        <h1 class="major">Edit StockX Account</h1>
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Email">Email</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="email" id="input-Email" v-model="TransitObject.Email" placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
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
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-ProxyUsername">Proxy Username</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="text" id="input-ProxyUsername" v-model="TransitObject.ProxyUsername"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-ProxyPassword">Proxy Password</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="password" id="input-ProxyPassword" v-model="TransitObject.ProxyPassword"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-ProxyHost">Proxy Host</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="text" id="input-ProxyHost" v-model="TransitObject.ProxyHost"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-ProxyPort">Proxy Port</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="number" id="input-ProxyPort" v-model="TransitObject.ProxyPort"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>

            <b-col sm="2">
                <label for="input-ProxyActive">Proxy Active</label>
            </b-col>
            <b-col sm="10">
                <b-form-checkbox
                        id="checkbox-ProxyActive"
                        v-model="TransitObject.ProxyActive"

                        :value="true"
                        unchecked-value="false"
                >

                </b-form-checkbox>
            </b-col>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-Active">Account Disabled (Check your login)</label>
                </b-col>
                <b-col sm="10">
                    <b-form-checkbox
                            id="checkbox-Active"
                            v-model="TransitObject.Disabled"

                            :value="true"
                            unchecked-value="false"
                    >

                    </b-form-checkbox>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-Active">Account Active</label>
                </b-col>
                <b-col sm="10">
                    <b-form-checkbox
                            id="checkbox-Active"
                            v-model="TransitObject.Active"

                            :value="true"
                            unchecked-value="false"
                    >

                    </b-form-checkbox>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-CustomerID">Customer ID (this will be auto filled)</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input disabled type="disabled" id="input-CustomerID" v-model="TransitObject.CustomerID"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-Currency">Currency</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input disabled type="text" id="input-Currency" v-model="TransitObject.Currency"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-Country">Country</label>
                </b-col>
                <b-col sm="10">
                    <b-form-input type="text" id="input-Country" v-model="TransitObject.Country"
                                  placeholder=""></b-form-input>
                </b-col>
            </b-row>
            <b-row class="my-1">
                <b-col sm="2">
                    <label for="input-Token">Verified</label>
                </b-col>
                <b-col sm="10">
                    {{TransitObject.Verified}}
                </b-col>
            </b-row>
            <b-button block @click="UpdateStockXAccount">Update</b-button>
            <p v-if="Success">StockXAccount Updated</p>
        </div>
    </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {client} from '@/shared';
import {redirect, Routes} from '@/shared/router';
import {DeleteStockXAccountRequest, ListOneStockXAccountRequest, UpdateStockXAccountRequest} from '@/shared/dtos';


@Component({
    components: {},
})
export default class Edit_StockXAccount extends Vue {

    Error = '';
    Success = false;
    Id = 0;

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
            Country: '',
            UserAgent: '',
            Token: '',
            StockXAccountId: -1,
        };

    mounted() {
        this.Id = +this.$route.params.Id;
        if (isNaN(this.Id)) {
            this.$router.push(Routes.Forbidden);
            return;
        }
        this.GetStockXAccount();
    }

    async DeleteStockXAccount() {
        if (!confirm('Are you sure you want to delete this StockXAccount? ')) {
            return;
        }
        this.Error = '';
        const Response = await client.delete(new DeleteStockXAccountRequest({
            StockXAccountId: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
        redirect(Routes.StockXAccounts);

    }

    async GetStockXAccount() {

        this.Error = '';
        const Response = await client.get(new ListOneStockXAccountRequest({
            StockXAccountId: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
        // @ts-ignore
        this.TransitObject = Response.StockXAccountItem;

    }


    async UpdateStockXAccount() {
        this.TransitObject.StockXAccountId = this.Id;
        try {
            this.Success = false;
            this.Error = '';
            const Response = await client.put(new UpdateStockXAccountRequest(this.TransitObject));
            if (Response.Success) {
                this.Success = true;
            } else {
                this.Error = Response.Message;
            }
        } catch (e) {
            this.Error = e.Message;
        }
    }
}
</script>


