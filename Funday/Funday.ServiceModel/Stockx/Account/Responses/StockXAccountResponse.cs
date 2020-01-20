
using System.Collections.Generic;
namespace Funday.ServiceModel.StockXAccount
{
    public class CreateStockXAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId {get; set; }
    }
    
      public class UpdateStockXAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    
      public class DeleteStockXAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class ListStockXAccountResponse 
    { 
        public long Total { get;set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<StockXAccount> StockXAccounts { get; set; }
    }
      public class ListOneStockXAccountResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public StockXAccount StockXAccountItem { get; set; }
    }
    
    
}
    