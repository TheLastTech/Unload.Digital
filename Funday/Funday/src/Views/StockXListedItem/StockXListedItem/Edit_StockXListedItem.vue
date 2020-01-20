<template>
    <div class="col-12">
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
              <p v-if="Message.length >0" class="alert-success">{{Message}}</p>
  <b-row class="my-1">
    <b-col sm="2">
      <label for="input-StockXUrl">UserID</label>
    </b-col>
    <b-col sm="10">
      <b-form-input type="text" id="input-StockXUrl" v-model="TransitObject.StockXUrl" placeholder=""></b-form-input>
    </b-col>
  </b-row>  <b-button block @click="UpdateStockXListedItem">Update</b-button>
        <p v-if="Success">StockXListedItem Updated</p>
           <b-button class="float-right btn-danger" @click="DeleteStockXListedItem()">Delete</b-button>
    </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {client} from '@/shared';
import {redirect, Routes} from '@/shared/router';
import {DeleteStockXListedItemRequest, ListOneStockXListedItemRequest, UpdateStockXListedItemRequest} from '@/shared/dtos';


@Component({
    components: {},
})
export default class Edit_StockXListedItem extends Vue {

    Error = '';
    Success = false;
    Id = 0;
    Message = '';

  TransitObject =

        {StockXUrl: '', StockXListedItemId: -1};

    mounted() {
        this.Id = +this.$route.params.Id;
        if (isNaN(this.Id)) {
            this.$router.push(Routes.Forbidden);
            return;
        }
        this.GetStockXListedItem();
    }

    async DeleteStockXListedItem() {
        if (!confirm('Are you sure you want to delete this StockXListedItem? ')) {
            return;
        }
        this.Error = '';
        const Response = await client.delete(new DeleteStockXListedItemRequest({
            StockXListedItemId: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
        redirect(Routes.StockXListedItems);

    }

    async GetStockXListedItem() {

        this.Error = '';
        const Response = await client.get(new ListOneStockXListedItemRequest({
            StockXListedItemId: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
                // @ts-ignore
        this.TransitObject = Response.StockXListedItemItem;

    }


        async UpdateStockXListedItem() {
            this.TransitObject.StockXListedItemId = this.Id;
            try{
        this.Success = false;
        this.Error = '';
        const Response = await client.put(new UpdateStockXListedItemRequest(this.TransitObject));
        if (Response.Success) {
            this.Success = true;
        }else{
            this.Error = Response.Message;
        }
        }catch (e){
            this.Error = e.Message;
        }
        }
}
</script>


