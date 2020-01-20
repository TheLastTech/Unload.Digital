module.exports.CreateCreate = (Name, NameSpace, CreateForm, CreateJs) =>
    `<template>
    <div class="offset-2 col-8">
   ${CreateForm}
   <a class="button btn-sm btn-danger" v-if="NewId>0" :href="'/Edit${Name}s/' + NewId">New Item {{this.NewId}}</a>
    </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {client} from '@/shared';
import {Create${Name}Request} from '@/shared/dtos';


@Component({
    components: {},
})
export default class Create_${Name} extends Vue {
    NewId=-1;
    Error = '';
    Success = false;
     ${CreateJs}
     
}
</script>
`

module.exports.CreateEdit = (Name, NameSpace,EditForm,EditJS) =>
    `<template>
    <div class="col-12">
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
              <p v-if="Message.length >0" class="alert-success">{{Message}}</p>
${EditForm}
        <p v-if="Success">${Name} Updated</p>
           <b-button class="float-right btn-danger" @click="Delete${Name}()">Delete</b-button>
    </div>
</template>

<script lang="ts">
import {Component, Vue} from 'vue-property-decorator';
import {client} from '@/shared';
import {redirect, Routes} from '@/shared/router';
import {Delete${Name}Request, ListOne${Name}Request, Update${Name}Request} from '@/shared/dtos';


@Component({
    components: {},
})
export default class Edit_${Name} extends Vue {
    
    Error = '';
    Success = false;
    Id = 0;
    Message="";

    mounted() {
        this.Id = +this.$route.params.Id;
        if (isNaN(this.Id)) {
            this.$router.push(Routes.Forbidden);
            return;
        }
        this.Get${Name}();
    }

    async Delete${Name}() {
        if (!confirm('Are you sure you want to delete this ${Name}? ')) {
            return;
        }
        this.Error = '';
        const Response = await client.delete(new Delete${Name}Request({
            ${Name}Id: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
        redirect(Routes.${Name}s);

    }

    async Get${Name}() {

        this.Error = '';
        const Response = await client.get(new ListOne${Name}Request({
            ${Name}Id: this.Id,
        }));
        if (!Response.Success) {

            this.Error = Response.Message;
            return;
        }
                //@ts-ignore
        this.TransitObject = Response.${Name}Item;
 
    }

  ${EditJS}
}
</script>

/*
  
    //${Name} 
    {path: Routes.${Name}s , component: List_${Name}s, beforeEnter: requiresAuth},
    {path: Routes.Edit${Name}s , component: Edit_${Name}, beforeEnter: requiresAuth},
    {path: Routes.Create${Name}, component: Create_${Name}, beforeEnter: requiresAuth},
  
            <b-nav-item-dropdown text="Lang" right    v-if="userSession">
                    <b-dropdown-item :to="Routes.Create${Name}">Make</b-dropdown-item>
                    <b-dropdown-item :to="Routes.${Name}s">List</b-dropdown-item>
                    
                </b-nav-item-dropdown>
    
    //${Name}
    ${Name}s = '/${Name}s',
    Edit${Name}s = '/Edit${Name}s/:Id',
    Create${Name} = '/Create${Name}',
/*
`


module.exports.CreateList = (Name, NameSpace,BTableFields,BTableTemplates,BTableJS) =>
    `<template>
    <div class="col-12">
        <p v-if="Error.length >0" class="alert-danger">{{Error}}</p>
                <p v-if="Message.length >0" class="alert-success">{{Message}}</p>
        
        <router-link to="/create${Name}" class="float-right" >Create</router-link>
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
       hover
      id="${Name}s-table"
      :items="Rows"
      :per-page="perPage"
      :current-page="currentPage"
      small
    >
    
        <template slot="cell(Id)" slot-scope="data">
                <b-button @click="Edit${Name}(data.item.Id)" variant="dark">Edit {{data.item.Id}}</b-button>
            </template>
            
            ${BTableTemplates}
    
</b-table>

    </div>
</template>

<script lang="ts">
   import {Component, Vue, Watch} from "vue-property-decorator";
import {List${Name}Request} from '@/shared/dtos';
import {client} from '@/shared';


@Component({
    components: {},
})
export default class List_${Name}s extends Vue {
    ${Name}s: ${Name}[] = [];
    Fields = [
        ${BTableFields}
    ]
    Error = '';
      currentPage=0;
    rows=0;
    perPage=50;
     @Watch("currentPage") ChangecurrentPage(newval:number){
        this.List${Name}s();
    }
    get Rows(){
        return this.${Name}s;
    }
    
    mounted() {
        this.List${Name}s();
    }

    Edit${Name}(Id: number){
        this.$router.push('/Edit${Name}/' + Id);
    }
    async List${Name}s() {
        this.Error = '';
        const ${Name}sList = await client.get(new List${Name}Request(  {
                Skip:50 * this.currentPage
            }));
        if (${Name}sList.Success) {
            this.${Name}s = ${Name}sList.${Name}s;
             this.rows = ${Name}sList.Total;
        } else {
            this.Error = ${Name}sList.Message;
        }
    }
    ${BTableJS}

}
</script>
`

