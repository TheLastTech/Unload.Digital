
using System.Collections.Generic;
namespace Funday.ServiceModel.StonksAccount
{
    public class CreateStonksAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId { get; internal set; }
    }
    
      public class UpdateStonksAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    
      public class DeleteStonksAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class ListStonksAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<StonksAccount> StonksAccounts { get; set; }
    }
      public class ListOneStonksAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public StonksAccount StonksAccountItem { get; set; }
    }
    
    
}
    