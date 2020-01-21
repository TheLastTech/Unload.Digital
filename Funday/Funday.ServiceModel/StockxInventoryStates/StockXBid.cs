using ServiceStack.DataAnnotations;

namespace Funday.ServiceModel.StockxInventoryStates
{
    public class StockXBid
    {
   
        public string Sku { get; set; }
        public long Bid { get; set; }
        [PrimaryKey]
        public string ChainId { get; set; }
        public double State { get; set; }
    }
}
