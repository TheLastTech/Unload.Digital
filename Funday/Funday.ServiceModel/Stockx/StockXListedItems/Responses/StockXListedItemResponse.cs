
using System;
using System.Collections.Generic;
namespace Funday.ServiceModel.StockXListedItem
{
    public class CreateStockXListedItemResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId {get; set; }
    }
    
      public class UpdateStockXListedItemResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    
      public class DeleteStockXListedItemResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class ListStockXListedItemResponse 
    { 
        public long Total { get;set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Tuple<StockXListedItem, AppUser, Inventory.Inventory>> StockXListedItems { get; set; }
    }
      public class ListOneStockXListedItemResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public StockXListedItem StockXListedItemItem { get; set; }
    }
    
    
}
    