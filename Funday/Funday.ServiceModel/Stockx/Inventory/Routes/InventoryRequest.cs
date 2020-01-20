using ServiceStack;
using System;

namespace Funday.ServiceModel.Inventory
{
    [Route("/Inventory/create", "Post")]
    public class CreateInventoryRequest : IReturn<CreateInventoryResponse>
    {
        public string StockXUrl { get; set; }
        public int Quantity { get; set; }
        public int MinSell { get; set; }
        public int StartingAsk { get; set; }
        public string Size { get; set; }
        public bool Active { get; set; }
        public int StockXAccountId { get; set; }
        public DateTime DateSold { get; set; }
        public string Status { get; set; }
        public string Sku { get; set; }
    }

    [Route("/Inventory/update", "Put")]
    public class UpdateInventoryRequest : IReturn<UpdateInventoryResponse>
    {
        public string StockXUrl { get; set; }
        public int Quantity { get; set; }
        public int MinSell { get; set; }
        public int StartingAsk { get; set; }
        public string Size { get; set; }
        public bool Active { get; set; }
        public int StockXAccountId { get; set; }
        public DateTime DateSold { get; set; }
        public string Status { get; set; }
        public string Sku { get; set; }

        public int InventoryId { get; set; }
    }

    [Route("/Inventory/delete", "Delete")]
    public class DeleteInventoryRequest : IReturn<DeleteInventoryResponse>
    {
        public int InventoryId { get; set; }
    }

    [Route("/Inventory/toggle", "Post")]
    public class ToggleInventoryRequest : IReturn<DeleteInventoryResponse>
    {
        public int InventoryId { get; set; }
    }

    [Route("/Inventory/list", "Get")]
    public class ListInventoryRequest : IReturn<ListInventoryResponse>
    {
        public int Skip { get; set; }
    }

    [Route("/Inventory/listone", "Get")]
    public class ListOneInventoryRequest : IReturn<ListOneInventoryResponse>
    {
        public int InventoryId { get; set; }
    }
}