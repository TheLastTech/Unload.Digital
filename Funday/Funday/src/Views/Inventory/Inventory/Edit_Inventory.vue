<template>
    <div class="">
        <h1 class="major">Edit An Inventory Slot</h1>
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>

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
                <label for="input-Active">Inventory Active</label>
            </b-col>
            <b-col sm="10">

                <b-button @click="ToggleInventory(TransitObject)" :variant="`${TransitObject.Active?'dark':'light'}`">
                    {{TransitObject.Active?
                    "Active":"Paused"}}
                </b-button>
            </b-col>
        </b-row>

        <b-button class="float-right btn-danger" @click="DeleteInventory()">Delete</b-button>
        <b-button block @click="UpdateInventory">Update</b-button>
        <p v-if="Success">Inventory Updated</p>
    </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {client} from '@/shared';
import {redirect, Routes} from '@/shared/router';
import {
    DeleteInventoryRequest,
    ListOneInventoryRequest,
    ToggleInventoryRequest,
    UpdateInventoryRequest,
} from '@/shared/dtos';


@Component({
    components: {},
})
export default class Edit_Inventory extends Vue {

    Error = '';
    Success = false;
    Id = 0;

    TransitObject =

        {
            StockXUrl: '',
            Quantity: 0,
            MinSell: 0,
            StartingAsk: 0,
            Size: '',
            Active: true,
            StockXAccountId: -1,
            DateSold: '',
            ChainId: '',
            Status: '',
            Sku: '',
            InventoryId: -1,
        };

    async ToggleInventory(Data: any) {

        try {
            this.Error = '';
            const InventorysList = await client.post(new ToggleInventoryRequest({
                InventoryId: Data.item.Id,

            }));
            if (InventorysList.Success) {
                this.GetInventory();

            } else {
                this.Error = InventorysList.Message;
            }
        } catch (e) {

        }
    }
    mounted() {
        this.Id = +this.$route.params.Id;
        if (isNaN(this.Id)) {
            this.$router.push(Routes.Forbidden);
            return;
        }
        this.GetInventory();
    }

    async DeleteInventory() {
        if (!confirm('Are you sure you want to delete this Inventory? ')) {
            return;
        }
        this.Error = '';
        const Response = await client.delete(new DeleteInventoryRequest({
            InventoryId: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
        redirect(Routes.Inventorys);

    }

    async GetInventory() {

        this.Error = '';
        const Response = await client.get(new ListOneInventoryRequest({
            InventoryId: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
        // @ts-ignore
        this.TransitObject = Response.InventoryItem;

    }


    async UpdateInventory() {
        this.TransitObject.InventoryId = this.Id;
        try {
            this.Success = false;
            this.Error = '';
            const Response = await client.put(new UpdateInventoryRequest(this.TransitObject));
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

