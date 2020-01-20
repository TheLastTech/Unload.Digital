using ServiceStack.DataAnnotations;
using System;

namespace Funday.ServiceModel.Inventory
{
    public class Inventory
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
        [Index]
        public int UserId { get; set; }
        [Index]
        public string StockXUrl { get; set; }
        public int Quantity { get; set; }
        [Default(typeof(int), "0")]
        public int TotalSold { get; set; }
        public int MinSell { get; set; }
        public int StartingAsk { get; set; }
        [Index]
        public string Size { get; set; }
        [Index]
        public bool Active { get; set; }
        [Index]
        public int StockXAccountId { get; set; }
        public string Status { get; set; }
        [Index]
        public string Sku { get; set; }
        [Index]
        public string ParentSku { get; set; }
    }
}