using Newtonsoft.Json;

namespace Funday.ServiceModel.StockXAccount
{
    public class ListItemRequestMeta
    {
        [JsonProperty("discountCode")]
        public string DiscountCode { get; set; }
    }
}