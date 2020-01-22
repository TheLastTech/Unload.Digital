<template>
    <div class="col-12">
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
        <p v-if="Message.length >0" class="alert-success">{{Message}}</p>
        <router-link to="/createInventory" class="float-right">Create</router-link>
        <b-pagination
                v-model="currentPage"
                :total-rows="rows"
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
                id="Inventorys-table"
                :items="Rows"
                :per-page="perPage"
                :current-page="currentPage"
                small
        >

            <template slot="cell(Id)" slot-scope="data">
                <b-button @click="EditInventory(data.item.Id)" variant="dark">Edit {{data.item.Id}}</b-button>
            </template>
            <template slot="cell(StockXUrl)" slot-scope="data">
                <div v-if="data.item.StockXUrl">{{data.item.StockXUrl.replace("https://stockx.com/","")}}</div>
            </template>
            <template slot="cell(Quantity)" slot-scope="data">
                <div v-if="!data.item.Is_Updating_Now">
                    <input v-if="!data.item.Quantity_Has_Updated" type="text" v-model="data.item.Quantity"
                           @change="UpdateInventory(data.item,'Quantity')"/>
                    <p v-else class="alert alert-success">Item Updated</p>
                    <p v-if="data.item.Quantity_Has_Error && data.item.Quantity_Has_Error.length > 0">
                        {{data.item.Quantity_Has_Error}}</p>
                </div>
                <div v-else>Updating...</div>
            </template>
            <template slot="cell(MinSell)" slot-scope="data">
                <div v-if="!data.item.Is_Updating_Now">
                    <input min v-if="!data.item.MinSell_Has_Updated" type="text" v-model="data.item.MinSell"
                           @change="UpdateInventory(data.item,'MinSell')"/>
                    <p v-else class="alert alert-success">Item Updated</p>
                    <p v-if="data.item.MinSell_Has_Error && data.item.MinSell_Has_Error.length > 0">
                        {{data.item.MinSell_Has_Error}}</p>
                </div>
                <div v-else>Updating...</div>
            </template>
            <template slot="cell(Sku)" slot-scope="data">

            </template>
            <template slot="cell(ParentSku)" slot-scope="data">

            </template>
            <template slot="cell(StartingAsk)" slot-scope="data">
                <div v-if="!data.item.Is_Updating_Now">
                    <b-input v-if="!data.item.StartingAsk_Has_Updated" type="text" v-model="data.item.StartingAsk"
                             @change="UpdateInventory(data.item,'StartingAsk')"></b-input>
                    <p v-else class="alert alert-success">Item Updated</p>
                    <p v-if="data.item.StartingAsk_Has_Error && data.item.StartingAsk_Has_Error.length > 0">
                        {{data.item.StartingAsk_Has_Error}}</p>
                </div>
                <div v-else>Updating...</div>
            </template>
            <template slot="cell(Active)" slot-scope="data">


                <b-button @click="ToggleInventory(data)" :variant="`${data.item.Active?'dark':'light'}`">
                    {{data.item.Active?
                    "Active":"Paused"}}
                </b-button>


            </template>
            <template slot="cell(Account)" slot-scope="data">
                <b-button @click="EditAccount(data.item.Account)" variant="dark">{{data.item.Account.Email}}</b-button>
            </template>

        </b-table>

    </div>
</template>

<script lang="ts">
    import {Component, Vue, Watch} from "vue-property-decorator";
    import {Inventory, ListInventoryRequest, ToggleInventoryRequest, UpdateInventoryRequest} from "@/shared/dtos";
    import {client} from "@/shared";


    @Component({
        components: {},
    })
    export default class List_Inventorys extends Vue {
        Inventorys: any[] = [];
        Fields = [
            {key: "StockXUrl", sortable: true},
            {key: "Quantity", sortable: true},
            {
                key: "MinSell",
                sortable: true,
            },
            {
                key: "TotalSold",
                sortable: true,
            },
            {key: "StartingAsk", sortable: true},
            {key: "Size", sortable: true},
            {
                key: "Active",
                sortable: true,
            },

        ];
        Error = "";
        currentPage = 0;
        rows = 0;
        perPage = 50;
        Rows: any[] = [];
        Message = "";

        async ToggleInventory(Data: any) {

            try {
                this.Error = "";
                const InventorysList = await client.post(new ToggleInventoryRequest({
                    InventoryId: Data.item.Id,

                }));
                if (InventorysList.Success) {
                    const inx = this.Inventorys.findIndex((A) => A.Id = Data.item.Id);
                    this.Inventorys[inx].Item1.Active = !this.Inventorys[inx].Item1.Active;

                } else {
                    this.Error = InventorysList.Message;
                }
            } catch (e) {

            }
        }

        get Rows2() {
            return this.Inventorys.map((A) => {
                A.Item1.Account = A.Item2;

                A.Item1.Is_Updating_Now = false;

                return A.Item1;
            });
        }

        FindByRow(Row: any) {
            return this.Inventorys.find((A) => {
                A.Item1.Account = A.Item2;
                if (Row.Id === A.Id) {
                    return A;
                }
            });
        }

        mounted() {
            this.ListInventorys();
        }

        EditInventory(Id: number) {
            this.$router.push("/EditInventory/" + Id);
        }

        async ListInventorys(Page: number = 0) {
            this.Error = "";
            const InventorysList = await client.get(new ListInventoryRequest({
                Skip: 50 * this.currentPage,
            }));
            if (InventorysList.Success) {

                this.rows = InventorysList.Total;
            } else {
                this.Error = InventorysList.Message;
                return;
            }
            InventorysList.Inventorys.forEach(A => {
                //@ts-ignore
                A.Item1.Account = A.Item2;
                this.Rows.push(A);
            });
            if ((Page + 1) * 50 < InventorysList.Total) {
                await this.ListInventorys(Page + 1);
            }
        }


        async UpdateInventory(Item: any, FieldName: string) {


            try {


                this.Error = "";
                const Response = await client.put(new UpdateInventoryRequest({
                    InventoryId: Item.Id,
                    Quantity: Item.Quantity, MinSell: Item.MinSell, StartingAsk: Item.StartingAsk,
                }));
                if (!Response.Success) {


                    this.Error = Response.Message;

                }
                this.Message = `Field ${FieldName} Updated`;
            } catch (e) {
                this.Error = e.Message;

            }


        }

    }
</script>
