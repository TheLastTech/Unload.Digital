<template>
    <div>
        <b-form-group :label="Label">
            <select class="form-control" v-model="SelectedItem" @input="Input">
                <option v-for="(Item,index) in RequestResponseAsSelect" :key="index" :value="Item[ValueField]">
                    {{Item[DisplayField]}}
                </option>
            </select>
            <p v-if="Error.length >0" class="alert alert-danger">{{Error}}</p>
        </b-form-group>

    </div>
</template>

<script lang="ts">
import {Component, Prop, Vue} from 'vue-property-decorator';
import {client, Hello} from '@/shared';


@Component({
    components: {},
})
export default class SelectByRequest extends Vue {

    get RequestResponseAsSelect() {
        return [];
    }
    @Prop({default: Hello}) RequestType: any;
    @Prop({default: ''}) ValueField: any;
    @Prop({default: ''}) DisplayField: any;
    @Prop({default: ''}) ResponseField: any;
    @Prop({default: ''}) Label: any;
    @Prop({default: ''}) DefaultValue: any;
    Error = '';
    SelectedItem: any = this.DefaultValue;
    Items: [] = [];
    Input(e: any){
        this.$emit('input', this.SelectedItem);
    }

    async GetListedItems() {
        try {
            const KeysList: any = await client.get(new this.RequestType());
            if (KeysList.success) {
                this.Items = KeysList[this.ResponseField];

            } else {
                this.Error = KeysList.message;
            }
        } catch (Exc) {
            this.Error = Exc.message;
        }
    }

}
</script>
