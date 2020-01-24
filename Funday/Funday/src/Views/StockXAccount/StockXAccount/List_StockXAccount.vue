<template>
    <div class="col-12 text-light ">
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
        <router-link to="/createStockXAccount" class="float-right">Create</router-link>
        <b-pagination
                v-model="currentPage"
                :total-rows="rows"
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
        <div class="table-wrapper">

            <b-table
                    responsive
                    :fields="Fields"
                    id="StockXAccounts-table"
                    :items="Rows"
                    :per-page="perPage"
                    :current-page="currentPage"

            >
                <template slot="cell(Token)" slot-scope="data">


                </template>
                <template slot="cell(NextVerification)" slot-scope="data">
                    {{new Date(data.item.NextVerification).toLocaleTimeString()}}

                </template>
                <template slot="cell(NextAccountInteraction)" slot-scope="data">
                    {{new Date(data.item.NextAccountInteraction).toLocaleTimeString()}}

                </template>
            </b-table>
        </div>
    </div>
</template>

<script lang="ts">
    import {Component, Vue, Watch} from "vue-property-decorator";
    import {ListStockXAccountRequest, StockXAccount} from "@/shared/dtos";
    import {client} from "@/shared";


    @Component({
        components: {},
    })
    export default class List_StockXAccounts extends Vue {
        StockXAccounts: StockXAccount[] = [];
        Error = "";
        currentPage = 0;
        rows = 0;
        perPage = 50;
        Fields = [
            {key: "Email", sortable: true},

            {
                key: "Active",
                sortable: true,
            },
            {
                key: "Verified",
                sortable: true,
            },
            {key: "AccountThread", sortable: true},

            {
                key: "NextAccountInteraction",
                sortable: true,
            }, {
                key: "NextVerification",
                sortable: true,
            },

        ];

        get Rows() {
            return this.StockXAccounts;
        }

        mounted() {
            this.ListStockXAccounts();
        }

        EditStockXAccount(Id: number) {
            this.$router.push("/EditStockXAccount/" + Id);
        }

        async ListStockXAccounts(Page: number = 0) {
            this.Error = "";
            const StockXAccountsList = await client.get(new ListStockXAccountRequest({
                Skip: 50 * this.currentPage,
            }));
            if (StockXAccountsList.Success) {
                this.rows = StockXAccountsList.Total;
            } else {
                this.Error = StockXAccountsList.Message;
                return;
            }
            StockXAccountsList.StockXAccounts.forEach(a => this.StockXAccounts.push(a));
            if ((Page + 1) * 50 < StockXAccountsList.Total) {


                await this.ListStockXAccounts(Page + 1);
            }
        }

    }
</script>
<style>
    .table {
        color: white !important;;
    }
</style>
