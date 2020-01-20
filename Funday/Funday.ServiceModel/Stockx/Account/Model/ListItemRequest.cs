using Newtonsoft.Json;

namespace Funday.ServiceModel.StockXAccount
{
    public   class ListItemRequest
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("PortfolioItem")]
        public ListItemRequestPortfolioItem PortfolioItem { get; set; }
    }
}