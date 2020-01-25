<template>
    <div >
        <h1 class="major">Create An Inventory Slot</h1>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-StockXUrl">Stock X Url</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="text" id="input-StockXUrl" v-model="TransitObject.StockXUrl"
                              placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Quantity">Quantity</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="number" id="input-Quantity" v-model="TransitObject.Quantity"
                              placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-MinSell">Min Sell</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="number" id="input-MinSell" v-model="TransitObject.MinSell"
                              placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-StartingAsk">StartingAsk</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="number" id="input-StartingAsk" v-model="TransitObject.StartingAsk"
                              placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Size">Size</label>
            </b-col>
            <b-col sm="10">
                <b-form-input type="text" id="input-Size" v-model="TransitObject.Size" placeholder=""></b-form-input>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Active">Select Account</label>
            </b-col>
            <b-col sm="10">
                <select v-model="TransitObject.StockXAccountId" class="form-control">
                    <option v-for="StockXAccount in StockXAccounts" :value="StockXAccount.Id">
                        {{StockXAccount.Email}}
                    </option>
                </select>
            </b-col>
        </b-row>
        <b-row class="my-1">
            <b-col sm="2">
                <label for="input-Active">Inventory Active</label>
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
        <b-button block @click="CreateInventory">Create</b-button>
        <a class="button btn-sm btn-danger" v-if="NewId>0" :href="'/EditInventorys/' + NewId">New Item
            {{this.NewId}}</a>
        <p v-if="Error.length >0">{{Error}}</p>
    </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {client} from '@/shared';
import {CreateInventoryRequest, ListStockXAccountRequest, StockXAccount} from '@/shared/dtos';


@Component({
    components: {},
})
export default class Create_Inventory extends Vue {
    NewId = -1;
    Error = '';
    Success = false;
    TransitObject =
        {
            StockXUrl: '',
            Quantity: 0,
            MinSell: 0,
            StartingAsk: 0,
            Size: '',
            Active: true,

        };
    StockXAccounts: StockXAccount[] = [];
    rows: number=0;
    mounted(){
        this.ListStockXAccounts();
    }
    async ListStockXAccounts() {
        this.Error = '';
        const StockXAccountsList = await client.get(new ListStockXAccountRequest({
            Skip: 0,
        }));
        if (StockXAccountsList.Success) {
            this.StockXAccounts = StockXAccountsList.StockXAccounts;
            this.rows = StockXAccountsList.Total;
        } else {
            this.Error = StockXAccountsList.Message;
        }
    }
    async CreateInventory() {
        try {
            this.Success = false;
            this.Error = '';
            const Response = await client.post(new CreateInventoryRequest(this.TransitObject));
            if (Response.Success) {
                this.Success = true;
                this.NewId = Response.InsertedId;
            } else {
                this.Error = Response.Message;
            }
        } catch (e) {
            this.Error = e.message;
        }
    }

}
</script>
