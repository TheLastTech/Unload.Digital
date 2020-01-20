
using System;
using System.Collections.Generic;
namespace Funday.ServiceModel.Inventory
{
    public class CreateInventoryResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId {get; set; }
    }
    
      public class UpdateInventoryResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    public class ToggleInventoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
     
    }
    public class DeleteInventoryResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class ListInventoryResponse 
    { 
        public long Total { get;set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Tuple<Inventory, StockXAccount.StockXAccount>> Inventorys { get; set; }
    }
      public class ListOneInventoryResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public Inventory InventoryItem { get; set; }
    }
    
    
}
    