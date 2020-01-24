using ServiceStack.DataAnnotations;
using System;

namespace Funday.ServiceModel.Audit
{
    public class StockXListingEvent
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
        public string Additional { get; set; }
        public DateTime When { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string ChainId { get; set; }
        public int Amount { get; set; }
    }
}