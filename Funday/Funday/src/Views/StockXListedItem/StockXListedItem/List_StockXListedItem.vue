<template>
    <div  >
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
        <p v-if="Message.length >0" class="alert-success">{{Message}}</p>

        <b-pagination v-if="perPage<rows"
                v-model="currentPage"
                :total-rows="RowCount"
                :fields="Fields"
                :per-page="perPage"
                class="mt-4"

        >
            <template v-slot:first-text><span class="text-success">First</span></template>
            <template v-slot:prev-text><span class="text-danger">Prev</span></template>
            <template v-slot:next-text><span class="text-warning">Next</span></template>
            <template v-slot:last-text><span class="text-info">Last</span></template>
            <template v-slot:ellipsis-text>
                <b-spinner small type="grow"></b-spinner>
                <b-spinner small type="grow"></b-spinner>
                <b-spinner small type="grow"></b-spinner>
            </template>
            <template v-slot:page="{ page, active }">
                <b v-if="active">{{ page }}</b>
                <i v-else>{{ page }}</i>
            </template>
        </b-pagination>

        <b-table
                responsive

                :fields="Fields"
                id="StockXListedItems-table"
                :items="Rows"
                :per-page="perPage"
                :current-page="currentPage"
                small
        >

            <template slot="head(Item1.Product.Shoe)" slot-scope="data">
                Shoe <b-input v-model="FilterShoe"></b-input>
            </template>
            <template slot="head(Item2.Email)" slot-scope="data">
                Email <b-input v-model="FilterEmail"></b-input>
            </template>
            <template slot="cell(Item1.Id)" slot-scope="data">
                <b-button @click="EditStockXListedItem(data.item.Item1.Id)" small variant="dark">History
                    ({{data.item.Item1.Id}})
                </b-button>
            </template>
            <template slot="cell(Item1.Product.Shoe)" slot-scope="data">
                <router-link :to="`/editInventorys/${data.item.Item3.Id}`" variant="dark">
                    {{data.item.Item1.Product.Shoe}}
                </router-link>
            </template>

            <template slot="cell(Item2.Email)" slot-scope="data">
                <b-button :to="`/editStockXAccounts/${data.item.Item2.Id}`" small variant="dark">
                    {{data.item.Item2.Email}}
                </b-button>
            </template>

        </b-table>

    </div>
</template>

<script lang="ts">
    import {Component, Vue, Watch} from "vue-property-decorator";
    import {ListStockXListedItemRequest, StockXListedItem} from "@/shared/dtos";
    import {client} from "@/shared";


    @Component({
        components: {},
    })
    export default class List_StockXListedItems extends Vue {
        StockXListedItems: any[] = [];
        Fields = [
            {key: "Item1.Id", sortable: true, label: "Id"},
            {key: "Item1.Product.Shoe", sortable: true, label: "Shoe"},
            {key: "Item1.Product.ShoeSize", sortable: true, label: "Size"},
            {key: "Item1.Amount", sortable: true, label: "$$"},
            {key: "Item3.MinSell", sortable: true, label: "MinSell"},
            {key: "Item3.StartingAsk", sortable: true, label: "Start"},
            {key: "Item3.Quantity", sortable: true, label: "Qty"},
            {key: "Item2.Email", sortable: true, label: "Email"},
            {key: "Item1.Sold", sortable: true, label: "Sold"},
        ];
        Error = "";
        currentPage = 0;
        rows = 0;
        perPage = 25;
        Message = "";
        FilterEmail="";
        FilterShoe="";
        get RowCount() {
            return this.Rows.length;
        }
        get Rows() {

            let items =this.StockXListedItems;
            if(this.FilterEmail.trim().length >0){
                items=items.filter(A=>A.Item2.Email.toLowerCase().indexOf(this.FilterEmail)>=0)
            }
            if(this.FilterShoe.trim().length >0){
                items=items.filter(A=>A.Item1.Product.Shoe.toLowerCase().indexOf(this.FilterShoe)>=0)
            }
            return items;
        }

        mounted() {
            this.ListStockXListedItems();
        }

        EditStockXListedItem(Id: number) {
            this.$router.push("/EditStockXListedItems/" + Id);
        }

        async ListStockXListedItems(Page: number = 0) {
            this.Error = "";
            const StockXListedItemsList = await client.get(new ListStockXListedItemRequest({
                Skip: 50 * this.currentPage,
            }));
            if (StockXListedItemsList.Success) {
                this.rows = StockXListedItemsList.Total;
            } else {
                this.Error = StockXListedItemsList.Message;
                return;
            }
            StockXListedItemsList.StockXListedItems.forEach(a => this.StockXListedItems.push(a));
            if ((Page + 1) * 50 < StockXListedItemsList.Total) {


                await this.ListStockXListedItems(Page + 1);
            }

        }


    }
</script>
