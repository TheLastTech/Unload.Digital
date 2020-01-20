const fs = require('fs');

const args = require('args');
let ServiceGenerator = require('./ServiceGenerator.js')
let Crud = require('./CRUDS')

args

    .option('JsonFile,', 'The Json File')


let Options = args.parse(process.argv);

let CreateForm = '';
let UpdateForm = '';
let SetPut = '';
let RuleSET = '';
let CsOutput = '';
let CreateJs = '';
let UpdateJs = '';
let UpdatePut = '';
let BTableFields = '';
let BTableTemplates = '';
let BTableJS = '';

if (!Options.J || !fs.existsSync(Options.J)) {
    console.log("need a json file")
}
const Config = GetConfig();


let ModelGenerator = require('./ModelGenerator.js');

if (Config.ModelGenerator) {
    ModelGenerator = require(Config.ModelGenerator);
}
if (Config.ServiceGenerator) {
    ServiceGenerator = require(Config.ServiceGenerator);
}

const Data = JSON.parse(fs.readFileSync(Options.J, 'utf-8'))

Options = Data;

PreProcess()
ProcessForm();

PatchScript();

CreateService();
CreateModels();

function CreateService() {

    const ServicePath = Config.ServicePath + "\\" + Options.subFolder;
    const ServiceFilePath = `${ServicePath}\\${Options.Name}Service.cs`;
    const Service = ServiceGenerator.Create(Options.Name, Config.NameSpace, RuleSET, RuleSET, SetPut, UpdatePut)

    ensureDir(Config.ServicePath + "\\" + Options.subFolder);
    fs.writeFileSync(ServiceFilePath, Service, "utf8");

}

function CreateModels() {

    const ModelsPath = Config.ModelPath + "\\" + Options.subFolder;
    const ViewsPath = Config.ViewsPath + "\\" + Options.Name;

    const CreateItemPath = `${ViewsPath}\\${Options.Name}\\Create_${Options.Name}.vue`;
    const ListItemPath = `${ViewsPath}\\${Options.Name}\\List_${Options.Name}.vue`;
    const EditItemPath = `${ViewsPath}\\${Options.Name}\\Edit_${Options.Name}.vue`;
    /*
    let CreateForm = '';
    let UpdateForm = '';
    let SetPut = '';
    let RuleSET = '';
    let CsOutput ='';
    let CreateJs='';
    let UpdateJs='';
     */
    const CreateItemFile = Crud.CreateCreate(Options.Name, Config.NameSpace, CreateForm, CreateJs)
    const ListItemFile = Crud.CreateList(Options.Name, Config.NameSpace, BTableFields, BTableTemplates, BTableJs);
    const EditItemFile = Crud.CreateEdit(Options.Name, Config.NameSpace, UpdateForm, UpdateJs)


    const DtoModelFilePath = `${ModelsPath}\\Model\\${Options.Name}.cs`;
    const RequestModelFilePath = `${ModelsPath}\\Routes\\${Options.Name}Request.cs`;
    const ResponseModelFilePath = `${ModelsPath}\\Responses\\${Options.Name}Response.cs`;

    const RequestModel = ModelGenerator.CreateRequest(Options.Name, Config.NameSpace, CsOutput)
    const ResponseModel = ModelGenerator.CreateResponse(Options.Name, Config.NameSpace)
    const DtoModel = ModelGenerator.CreateDto(Options.Name, Config.NameSpace, CsOutput)
    ensureDir(ModelsPath);
    ensureDir(ViewsPath);
    ensureDir(`${ViewsPath}\\\\${Options.Name}`);
    ensureDir(`${ModelsPath}\\Routes`);
    ensureDir(`${ModelsPath}\\Responses`);
    ensureDir(`${ModelsPath}\\Model`);
    fs.writeFileSync(RequestModelFilePath, RequestModel, "utf8");
    fs.writeFileSync(ResponseModelFilePath, ResponseModel, "utf8");
    fs.writeFileSync(DtoModelFilePath, DtoModel, "utf-8")

    fs.writeFileSync(CreateItemPath, CreateItemFile, "utf8");
    fs.writeFileSync(ListItemPath, ListItemFile, "utf8");
    fs.writeFileSync(EditItemPath, EditItemFile, "utf-8")
}

function GetConfig() {
    return {
        "ModelPath": "C:\\Sunday\\Funday\\Funday.ServiceModel",
        "ServicePath": "C:\\Sunday\\Funday\\Funday.ServiceInterface",
        "ViewsPath": "C:\\Sunday\\Funday\\Funday\\src\\Views",
        "NameSpace": "Funday"
    }

}


function ensureDir(dirpath) {

    fs.mkdirSync(dirpath, {recursive: true})

}



function PatchScript() {
    var obj = {};
    Data.Items.forEach(A => {
        obj[A.Name] = A.DefaultValue;
    })
    CreateJs = `TransitObject = 
        ${JSON.stringify(obj)}
         
        async Create${Data.Name}() {
        try{
        this.Success = false;
        this.Error = '';
        const Response = await client.post(new Create${Data.Name}Request(this.TransitObject));
        if (Response.Success) {
            this.Success = true;
            this.NewId = Response.InsertedId
        }else{
            this.Error = Response.Message;
        }
        }catch(e){
        this.Error = e.message;
        }
        }`
    obj[`${Data.Name}Id`] = -1;
    UpdateJs = `TransitObject = 
   
        ${JSON.stringify(obj)}
        
           
        async Update${Data.Name}() {
            this.TransitObject.${Data.Name}Id = this.Id;
        try{
        this.Success = false;
        this.Error = '';
        const Response = await client.put(new Update${Data.Name}Request(this.TransitObject));
        if (Response.Success) {
            this.Success = true;
        }else{
            this.Error = Response.Message;
        }
        }catch(e){
            this.Error = e.Message;
        }
        }`

    var Edited = Data.Items.filter(A => !A.NotEditable).map(A => {
        return `${A.Name}: Item.${A.Name} `
    }).join(',')
    BTableJs = ` 
           
        async Update${Data.Name}(Item,FieldName) {
           ;
           Item[FieldName+'_Has_Error'] = '';
           Item[FieldName+'_Has_Updated'] = false;
        try{
        
        this.Success = false;
        this.Error = '';
        const Response = await client.put(new Update${Data.Name}Request({
        ${Data.Name}Id:Item.Id,
        ${Edited}
        }));
        if (Response.Success) {
        Item[FieldName+'_Has_Updated'] = true;
        setTimeout(()=>{
        Item[FieldName+'_Has_Updated'] = false;
        },300)
        
            this.Success = true;
        }else{
        Item[FieldName+'_Has_Error'] = Response.Message
            this.Error = Response.Message;
        }
        }catch(e){
            this.Error = e.Message;
            Item[FieldName+'_Has_Error'] = e.message
        }
        }`
}

function ProcessPassword(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="password"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }

    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessCheckBox(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="checkbox"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public bool ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
    if (!Item.Optional) {
        RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
    }
    UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
}

function ProcessDate(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="date"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public DateTime ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function Processtime(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="time"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public DateTime ${Item.Name} { get; set; }`;
    SetPut += ` ${Item.Name} = request.${Item.Name},`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessNumber(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="number"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public long ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessEmail(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="email"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},`;

    RuleSET += `RuleFor(x => x.${Item.Name}).EmailAddress();\n`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessSearch(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="search"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessTel(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="tel"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessUrl(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="url"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessText(Item) {
    BTableFields += JSON.stringify({key: Item.Name, sortable: true}) + ',';
    if (!Item.NotEditable) {
        BTableTemplates += ` <template slot="cell(${Item.Name})" slot-scope="data">
                <input  v-if="!data.item.${Item.Name}_Has_Updated" type="text"  v-model="data.item.${Item.Name}" @change="Update${Data.Name}(data.item,'${Item.Name}')" />
                <p v-else class="alert alert-success" >Item Updated</p>
                <p v-if="data.item.${Item.Name}_Has_Error && data.item.${Item.Name}_Has_Error.length > 0">{{data.item.${Item.Name}_Has_Error}}</p>
            </template>`
    }
    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function processRequestSelect(Item) {

    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessSelect(Item) {
    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}

function ProcessDisabled(Item) {
    CsOutput += `public string ${Item.Name} { get; set; }\n`;
    SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
    if(Item.NotEditable) {
        UpdatePut += `Existing${Data.Name}.${Item.Name} = request.${Item.Name};\n`;
        if (!Item.Optional) {
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
        }
    }
}


function ProcessCheckboxFormItem(Item) {
    CreateForm += `   
   
       <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
    <b-form-checkbox
                id="checkbox-${Item.Name}"
                v-model="TransitObject.${Item.Name}"
             
                :value="true"
                unchecked-value="false"
                    >
                    
            </b-form-checkbox>
</b-col>`
}

function ProcessDefaultFormItem(Item) {
    if(Item.NotCreatable)return;
    CreateForm += `  <b-row class="my-1">
    <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
      <b-form-input type="${Item.FormType}" id="input-${Item.Name}" v-model="TransitObject.${Item.Name}" placeholder="${Item.PlaceHolder}"></b-form-input>
    </b-col>
  </b-row>`
}

function ProcessDisabledFormItem(Item) {
    if(Item.NotCreatable)return;
    CreateForm += `  <b-row class="my-1">
    <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
      <b-form-input disabled type="${Item.FormType}" id="input-${Item.Name}" v-model="TransitObject.${Item.Name}" placeholder="${Item.PlaceHolder}"></b-form-input>
    </b-col>
  </b-row>`
}

function ProcessRequestSelectFormItem(Item) {
    if(Item.NotCreatable)return;
    CreateForm += `  
   <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
     
      <select-by-request :default-value = "${Item.FormArguments.DefaultValue}" :response-field="${Item.FormArguments.ResponseField}" :value-field="${Item.FormArguments.ValueField}"  :display-field="${Item.FormArguments.DisplayField}" class="form-control" id="input-${Item.Name}" v-model="TransitObject.${Item.Name}" >
               
            </select-by-request>
     
    </b-col>
   `
}

function ProcessSelectFormItem(Item) {
    if(Item.NotCreatable)return;
    CreateForm += `  
   <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
     
      <select class="form-control" id="input-${Item.Name}" v-model="TransitObject.${Item.Name}" >
                <option v-for="(Key,index) in ${Item.Name}s" :key="Key.id" :value="Key.id">
                    {{Key.name}}
                </option>
            </select>
     
    </b-col>

   `
}
function ProcessForm() {
    for (const Item of Data.Items) {
        switch (Item.FormType) {
            case 'checkbox':
                ProcessCheckboxFormItem(Item);
                break;
            case 'password':
            case 'date':
            case 'time':
            case "number":
            case "email":
            case "search":
            case "tel":
            case "url":
            case 'text':

                ProcessDefaultFormItem(Item);
                break;
            case 'disabled':
                ProcessDisabledFormItem(Item);
                break;
            case 'requestSelect':
                ProcessRequestSelectFormItem(Item);
                break;
            case "select":
                ProcessSelectFormItem(Item);
        }
    }
    UpdateForm = CreateForm + `  <b-button block @click="Update${Data.Name}">Update</b-button>`
    CreateForm = CreateForm + `  <b-button block @click="Create${Data.Name}">Create</b-button>`
}

function PreProcess() {
    for (const Item of Data.Items) {
        switch (Item.FormType) {
            case 'password':
                ProcessPassword(Item);
                break;
            case 'checkbox':
                ProcessCheckBox(Item);
                break;
            case 'date':
                ProcessDate(Item);
                break;
            case 'time':
                Processtime(Item);
                break;
            case "number":
                ProcessNumber(Item);
                break;
            case "email":
                ProcessEmail(Item);
                break;
            case "search":
                ProcessSearch(Item);
                break;
            case "tel":
                ProcessTel(Item);
                break;
            case "url":
                ProcessUrl(Item);
                break;
            case 'text':
                ProcessText(Item);
                break;
            case 'requestSelect':
                processRequestSelect(Item);
                break;
            case 'select':
                ProcessSelect(Item);
                break;

            case 'disabled':
                ProcessDisabled(Item);


        }
    }
}
