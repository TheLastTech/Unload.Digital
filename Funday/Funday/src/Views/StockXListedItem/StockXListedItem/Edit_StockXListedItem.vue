<template>
    <div class="col-12">
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
        <p v-if="Message.length >0" class="alert-success">{{Message}}</p>
        <b-table
                responsive


                id="StockXListedItemsHistory-table"
                :items="History"

        >



        </b-table>
    </div>
</template>

<script lang="ts">
    import {Component, Vue} from "vue-property-decorator";
    import {client} from "@/shared";
    import {redirect, Routes} from "@/shared/router";
    import {
        DeleteStockXListedItemRequest,
        ListOneStockXListedItemRequest,
        StockXListingEvent
    } from "@/shared/dtos";


    @Component({
        components: {},
    })
    export default class Edit_StockXListedItem extends Vue {

        Error = "";
        Success = false;
        Id = 0;
        Message = "";

        TransitObject =

            {StockXUrl: "", StockXListedItemId: -1};
         History: StockXListingEvent[]= []

        mounted() {
            this.Id = +this.$route.params.Id;
            if (isNaN(this.Id)) {
                this.$router.push(Routes.Forbidden);
                return;
            }
            this.GetStockXListedItem();
        }



        async GetStockXListedItem() {

            this.Error = "";
            const Response = await client.get(new ListOneStockXListedItemRequest({
                StockXListedItemId: this.Id,
            }));
            if (!Response.Success) {

                this.Error = Response.Message;
                return;
            }
            // @ts-ignore
            this.TransitObject = Response.StockXListedItemItem;
            this.History= Response.History;

        }



    }
</script>


