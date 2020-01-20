
using System.Collections.Generic;
namespace Funday.ServiceModel.Login
{
    public class CreateLoginResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId { get; internal set; }
    }
    
      public class UpdateLoginResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    
      public class DeleteLoginResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class ListLoginResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Login> Logins { get; set; }
    }
      public class ListOneLoginResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public Login LoginItem { get; set; }
    }
    
    
}
    