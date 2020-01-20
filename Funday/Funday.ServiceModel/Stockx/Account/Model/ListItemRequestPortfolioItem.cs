using Newtonsoft.Json;
using System;

namespace Funday.ServiceModel.StockXAccount
{

    public class ListItemRequestPortfolioItem
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
        public ListItemRequestMeta Meta { get; set; }
    }
}