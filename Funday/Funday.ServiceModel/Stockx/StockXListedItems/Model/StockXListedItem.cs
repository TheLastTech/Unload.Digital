
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using StockxApi;
using System;

namespace Funday.ServiceModel.StockXListedItem
{
    public class StockXListedItem
    {
        public static implicit operator StockXListedItem(PortfolioItem Item)
        {
            return new StockXListedItem()
            {
                ChainId = Item.ChainId,

                CustomerId = Item.CustomerId,

                UserUuid = Item.UserUuid,

                InventoryId = Item.InventoryId,

                ProductId = Item.ProductId,

                SkuUuid = Item.SkuUuid,

                MerchantId = Item.MerchantId,

                Condition = Item.Condition,

                Action = Item.Action,

                ActionBy = Item.ActionBy,

                Amount = Item.Amount,

                LocalCurrency = Item.LocalCurrency,

                LocalAmount = Item.LocalAmount,

                LocalExchangeRate = Item.LocalExchangeRate,

                BidAskSpread = Item.BidAskSpread,

                ExpiresAt = Item.ExpiresAt,

                ExpiresAtTime = Item.ExpiresAtTime,

                FaqLink = Item.FaqLink,

                GainLossDollars = Item.GainLossDollars,

                GainLossPercentage = Item.GainLossPercentage,

                MarketValue = Item.MarketValue,

                MatchedWith = Item.MatchedWith,

                MatchedState = Item.MatchedState,

                MatchedWithDate = Item.MatchedWithDate,

                Owner = Item.Owner,

                UserFullname = Item.UserFullname,

                PurchasePrice = Item.PurchasePrice,

                PurchaseDate = Item.PurchaseDate,

                PurchaseDateTime = Item.PurchaseDateTime,

                ShipByDate = Item.ShipByDate,

                State = Item.State,

                StatusMessage = Item.StatusMessage,

                Text = Item.Text,

                Notes = Item.Notes,

                CountryCode = Item.CountryCode,

                CreatedAt = Item.CreatedAt,

                CreatedAtTime = Item.CreatedAtTime,

                Url = Item.Url,

                ReferUrl = Item.ReferUrl,

                CanEdit = Item.CanEdit,

                CanDelete = Item.CanDelete,

                IsPartOfBulkShipment = Item.IsPartOfBulkShipment,

                Tracking = Item.Tracking,

                Meta = Item.Meta,

                OrderNumber = Item.OrderNumber,

                Total = Item.Total,

                LocalTotal = Item.LocalTotal,

                Product = Item.Product,

                Merchant = Item.Merchant,

                Customer = Item.Customer,

                LocalMarketValue = Item.LocalMarketValue,

                LocalGainLoss = Item.LocalGainLoss,
            };
        }

        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Index]
        [JsonProperty("chainId")]
        public string ChainId { get; set; }

        [JsonProperty("customerId")]
        public double CustomerId { get; set; }

        [JsonProperty("userUuid")]
        public string UserUuid { get; set; }

        [JsonProperty("inventoryId")]
        public string InventoryId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("skuUuid")]
        public string SkuUuid { get; set; }

        [JsonProperty("merchantId")]
        public double MerchantId { get; set; }

        [JsonProperty("condition")]
        public double Condition { get; set; }

        [JsonProperty("action")]
        public double Action { get; set; }

        [JsonProperty("actionBy")]
        public double ActionBy { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("localCurrency")]
        public string LocalCurrency { get; set; }

        [JsonProperty("localAmount")]
        public double LocalAmount { get; set; }

        [JsonProperty("localExchangeRate")]
        public double LocalExchangeRate { get; set; }

        [JsonProperty("bidAskSpread")]
        public double BidAskSpread { get; set; }

        [JsonProperty("expiresAt")]
        public string ExpiresAt { get; set; }

        [JsonProperty("expiresAtTime")]
        public double ExpiresAtTime { get; set; }

        [JsonProperty("faqLink")]
        public string FaqLink { get; set; }

        [JsonProperty("gainLossDollars")]
        public double GainLossDollars { get; set; }

        [JsonProperty("gainLossPercentage")]
        public double GainLossPercentage { get; set; }

        [JsonProperty("marketValue")]
        public double? MarketValue { get; set; }

        [JsonProperty("matchedWith")]
        public string MatchedWith { get; set; }

        [JsonProperty("matchedState")]
        public double MatchedState { get; set; }

        [JsonProperty("matchedWithDate")]
        public string MatchedWithDate { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("userFullname")]
        public string UserFullname { get; set; }

        [JsonProperty("purchasePrice")]
        public double PurchasePrice { get; set; }

        [JsonProperty("purchaseDate")]
        public DateTimeOffset? PurchaseDate { get; set; }

        [JsonProperty("purchaseDateTime")]
        public double? PurchaseDateTime { get; set; }

        [JsonProperty("shipByDate")]
        public string ShipByDate { get; set; }

        [JsonProperty("state")]
        public double State { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("createdAtTime")]
        public double CreatedAtTime { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("referUrl")]
        public string ReferUrl { get; set; }

        [JsonProperty("canEdit")]
        public bool CanEdit { get; set; }

        [JsonProperty("canDelete")]
        public bool CanDelete { get; set; }

        [JsonProperty("isPartOfBulkShipment")]
        public bool IsPartOfBulkShipment { get; set; }

        [JsonProperty("Tracking")]
        public Tracking Tracking { get; set; }

        [JsonProperty("meta")]
        public PortfolioItemMeta Meta { get; set; }

        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("localTotal")]
        public string LocalTotal { get; set; }

        [JsonProperty("product")]
        public Product Product { get; set; }

        [JsonProperty("Merchant")]
        public Merchant Merchant { get; set; }

        [JsonProperty("Customer")]
        public Customer Customer { get; set; }

        [JsonProperty("localMarketValue")]
        public double? LocalMarketValue { get; set; }

        [JsonProperty("localGainLoss")]
        public double LocalGainLoss { get; set; }
        public bool Sold { get; set; }
    }
 
}