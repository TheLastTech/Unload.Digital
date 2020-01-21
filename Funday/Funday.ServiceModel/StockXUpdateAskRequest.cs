using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funday.ServiceModel
{
   
    public   class StockXUpdateAskRequest
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("PortfolioItem")]
        public StockXUpdateAskPortfolioItem PortfolioItem { get; set; }
    }

    public class StockXUpdateAskPortfolioItem
    {
        [JsonProperty("localAmount")]
        public long LocalAmount { get; set; }

        [JsonProperty("expiresAt")]
        public string ExpiresAt { get; set; }

        [JsonProperty("skuUuid")]
        public string SkuUuid { get; set; }

        [JsonProperty("localCurrency")]
        public string LocalCurrency { get; set; }

        [JsonProperty("meta")]
        public StockXUpdateAskMeta Meta { get; set; }

        [JsonProperty("chainId")]
        public string ChainId { get; set; }
    }

    public partial class StockXUpdateAskMeta
    {
        [JsonProperty("discountCode")]
        public string DiscountCode { get; set; }
    }
}
