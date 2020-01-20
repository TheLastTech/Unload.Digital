const fs = require('fs');
const Path = require('path');
const file = "C:\\Sunday\\Funday\\Funday\\src\\Views\\StockXAccount\\StockXAccount\\Form.Json";
const dir = Path.dirname(file);
const Data = JSON.parse(fs.readFileSync(file, 'utf-8'))
const filename = Path.basename(file, ".Json");


const outputfile = dir + filename + ".vue";
console.log(outputfile);

let Output = '';
let CsOutput = '';
let SetPut = '';
let RuleSET = '';
for (const Item of Data.Items) {
    switch (Item.FormType) {
        case 'password':
            CsOutput += `public string ${Item.Name} { get; set; }`;
            SetPut += ` ${Item.Name} = request.${Item.Name},`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;

            break;
        case 'date':
            CsOutput += `public DateTime ${Item.Name} { get; set; }`;
            SetPut += ` ${Item.Name} = request.${Item.Name},`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
            break;
        case 'time':
            CsOutput += `public DateTime ${Item.Name} { get; set; }`;
            SetPut += ` ${Item.Name} = request.${Item.Name},`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
            break;
        case "number":
            CsOutput += `public long ${Item.Name} { get; set; }`;
            SetPut += ` ${Item.Name} = request.${Item.Name},`;

            break;
        case "email":
            CsOutput += `public string ${Item.Name} { get; set; }`;
            SetPut += ` ${Item.Name} = request.${Item.Name},`;
            RuleSET += `RuleFor(x => x.${Item.Name}).EmailAddress();\n`;
            break;
        case "search":
            CsOutput += `public string ${Item.Name} { get; set; }`;
            SetPut += ` ${Item.Name} = request.${Item.Name},`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
            break;
        case "tel":
            CsOutput += `public string ${Item.Name} { get; set; }`;
            SetPut += ` ${Item.Name} = request.${Item.Name},`;
            break;
        case "url":
            CsOutput += `public string ${Item.Name} { get; set; }\n`;
            SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
            break;
        case 'text':
            CsOutput += `public string ${Item.Name} { get; set; }\n`;
            SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
            break;
        case 'requestSelect':
            CsOutput += `public string ${Item.Name} { get; set; }\n`;
            SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
            break;
        case 'select':
            CsOutput += `public string ${Item.Name} { get; set; }\n`;
            SetPut += ` ${Item.Name} = request.${Item.Name},\n`;
            RuleSET += `RuleFor(x => x.${Item.Name}).NotEmpty();\n`;
            break;

    }
}
for (const Item of Data.Items) {
    switch (Item.FormType) {
        case 'password':
        case 'date':
        case 'time':
        case "number":
        case "email":
        case "search":
        case "tel":
        case "url":
        case 'text':

            Output += `  <b-row class="my-1">
    <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
      <b-form-input type="${Item.FormType}" id="input-${Item.Name}" v-model="CreateObject.${Item.Name}" placeholder="${Item.PlaceHolder}"></b-form-input>
    </b-col>
  </b-row>`
            break;
        case 'requestSelect':
            Output += `  
   <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
     
      <select-by-request :default-value = "${Item.FormArguments.DefaultValue}" :response-field="${Item.FormArguments.ResponseField}" :value-field="${Item.FormArguments.ValueField}"  :display-field="${Item.FormArguments.DisplayField}" class="form-control" id="input-${Item.Name}" v-model="CreateObject.${Item.Name}" >
               
            </select-by-request>
     
    </b-col>
   `
            break;
        case "select":
            Output += `  
   <b-col sm="2">
      <label for="input-${Item.Name}">${Item.Label}</label>
    </b-col>
    <b-col sm="10">
     
      <select class="form-control" id="input-${Item.Name}" v-model="CreateObject.${Item.Name}" >
                <option v-for="(Key,index) in ${Item.Name}s" :key="Key.id" :value="Key.id">
                    {{Key.name}}
                </option>
            </select>
     
    </b-col>
   `


    }
}
var obj = {};
Data.Items.filter(A => A.includeInRequest).forEach(A=>{
    obj[A.Name] = obj.DefaultValue;
})

Output += " \n/*******************************\\*";
Output += `
        CreateObject =  {
        ${JSON.stringify(obj)}
        }
        async ${Data.Function}() {
        this.Success = false;
        this.Error = '';
        const Response = await client.post(new ${Data.Request}(this.CreateObject));
        if (Response.success) {
            this.Success = true;
        }else{
            this.Error = Response.message;
        }

    }
    `

Output += "\n/*******************************\n\n\n\n\n*";
Output += CsOutput


Output += "\n/*******************************\*\n\n\n\n\n\n\n ";
Output += SetPut
Output += "\n/*******************************\*\n\n\n\n\n\n\n ";
Output += RuleSET
fs.writeFileSync(outputfile, Output, 'utf-8')





