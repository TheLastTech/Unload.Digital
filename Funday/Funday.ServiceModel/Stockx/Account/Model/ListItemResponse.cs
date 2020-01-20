using Newtonsoft.Json;
using StockxApi;

namespace Funday.ServiceModel.StockXAccount
{
    public partial class ListItemResponse
    {
        [JsonProperty("PortfolioItem")]
        public PortfolioItem PortfolioItem { get; set; }
    }
}