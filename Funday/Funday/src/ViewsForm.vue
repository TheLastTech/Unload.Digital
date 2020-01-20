  <b-row class="my-1">
    <b-col sm="2">
      <label for="input-UserName">UserName</label>
    </b-col>
    <b-col sm="10">
      <b-form-input type="text" id="input-UserName" v-model="CreateObject.UserName" placeholder=""></b-form-input>
    </b-col>
  </b-row>  <b-row class="my-1">
    <b-col sm="2">
      <label for="input-Password">Password</label>
    </b-col>
    <b-col sm="10">
      <b-form-input type="password" id="input-Password" v-model="CreateObject.Password" placeholder=""></b-form-input>
    </b-col>
  </b-row> 
/*******************************\*
        CreateObject =  {
        UserName: '',Password: ''
        }
        async CreateAccount() {
        this.Success = false;
        this.Error = '';
        const Response = await client.post(new CreateAccountRequest(this.CreateObject));
        if (Response.success) {
            this.Success = true;
        }else{
            this.Error = Response.message;
        }

    }
    
/*******************************




*public string UserName { get; set; }
public string Password { get; set; }
/********************************






  UserName = request.UserName,
 Password = request.Password,