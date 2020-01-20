using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;

namespace StockxApi
{
    public class LoginCookieToken
    {
        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("Cookies")]
        public List<Cooky> Cookies { get; set; }

        [JsonProperty("CustomerInfo")]
        public CustomerInfo CustomerInfo { get; set; }

        [JsonProperty("Globals")]
        public Globals Globals { get; set; }

        public string error { get; set; }
    }

    public class Cooky
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("hostOnly")]
        public bool HostOnly { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("secure")]
        public bool Secure { get; set; }

        [JsonProperty("httpOnly")]
        public bool HttpOnly { get; set; }

        [JsonProperty("session")]
        public bool Session { get; set; }

        [JsonProperty("expirationDate", NullValueHandling = NullValueHandling.Ignore)]
        public double? ExpirationDate { get; set; }
    }

    public class CustomerInfo
    {
        [JsonProperty("Billing")]
        public Billing Billing { get; set; }

        [JsonProperty("CCOnly")]
        public Billing CcOnly { get; set; }

        [JsonProperty("Merchant")]
        public Merchant Merchant { get; set; }

        [JsonProperty("Meta")]
        public Meta Meta { get; set; }

        [JsonProperty("Shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("createdAtTime")]
        public double CreatedAtTime { get; set; }

        [JsonProperty("defaultCategory")]
        public string DefaultCategory { get; set; }

        [JsonProperty("defaultCurrency")]
        public string DefaultCurrency { get; set; }

        [JsonProperty("defaultSize")]
        public string DefaultSize { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("flagged")]
        public bool Flagged { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("gdprStatus")]
        public object GdprStatus { get; set; }

        [JsonProperty("hasBuyerReward")]
        public bool HasBuyerReward { get; set; }

        [JsonProperty("hidePortfolioBanner")]
        public bool HidePortfolioBanner { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("intercom_token")]
        public string IntercomToken { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("isBuying")]
        public bool IsBuying { get; set; }

        [JsonProperty("isSelling")]
        public bool IsSelling { get; set; }

        [JsonProperty("isTrader")]
        public bool IsTrader { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("md5Email")]
        public string Md5Email { get; set; }

        [JsonProperty("referUrl")]
        public object ReferUrl { get; set; }

        [JsonProperty("sha1Email")]
        public string Sha1Email { get; set; }

        [JsonProperty("shipByDate")]
        public object ShipByDate { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("vacationDate")]
        public object VacationDate { get; set; }
    }

    public class Billing
    {
        [JsonProperty("Address")]
        public Address Address { get; set; }

        [JsonProperty("accountEmail")]
        public object AccountEmail { get; set; }

        [JsonProperty("cardType")]
        public string CardType { get; set; }

        [JsonProperty("cardholderName")]
        public string CardholderName { get; set; }

        [JsonProperty("expirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("last4")]
        public string Last4 { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public class Address
    {
        [JsonProperty("countryCodeAlpha2")]
        public string CountryCodeAlpha2 { get; set; }

        [JsonProperty("extendedAddress")]
        public double ExtendedAddress { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }

        [JsonProperty("postalCode")]
        public double PostalCode { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }
    }

    public class Merchant
    {
        [JsonProperty("accountName")]
        public object AccountName { get; set; }

        [JsonProperty("merchantId")]
        public double MerchantId { get; set; }

        [JsonProperty("paypalEmail")]
        public string PaypalEmail { get; set; }

        [JsonProperty("preferredPayout")]
        public double PreferredPayout { get; set; }
    }

    public class Meta
    {
        [JsonProperty("ccicId")]
        public object CcicId { get; set; }
    }

    public class Shipping
    {
        [JsonProperty("Address")]
        public Address Address { get; set; }
    }

    public class Globals
    {
        [JsonProperty("AFFIRM_PUBLIC_API")]
        public string AffirmPublicApi { get; set; }

        [JsonProperty("AFFIRM_SCRIPT")]
        public Uri AffirmScript { get; set; }

        [JsonProperty("APP_FROM_EMAIL")]
        public string AppFromEmail { get; set; }

        [JsonProperty("APP_MODE")]
        public string AppMode { get; set; }

        [JsonProperty("APP_NAME")]
        public string AppName { get; set; }

        [JsonProperty("APP_TIME")]
        public double AppTime { get; set; }

        [JsonProperty("ASSET_HOST")]
        public string AssetHost { get; set; }

        [JsonProperty("AUTH0")]
        public Auth0 Auth0 { get; set; }

        [JsonProperty("ENVIRONMENT")]
        public string Environment { get; set; }

        [JsonProperty("GA_KEY")]
        public string GaKey { get; set; }

        [JsonProperty("GTM_KEY")]
        public string GtmKey { get; set; }

        [JsonProperty("PN_SUBSCRIBE")]
        public string PnSubscribe { get; set; }

        [JsonProperty("RD20_PRE_BID_END_TIME")]
        public DateTimeOffset Rd20PreBidEndTime { get; set; }

        [JsonProperty("RD20_START_TIME")]
        public DateTimeOffset Rd20StartTime { get; set; }

        [JsonProperty("search")]
        public Search Search { get; set; }
    }

    public class Auth0
    {
        [JsonProperty("AUDIENCE")]
        public string Audience { get; set; }

        [JsonProperty("CLIENTID")]
        public string Clientid { get; set; }

        [JsonProperty("CONNECTION")]
        public string Connection { get; set; }

        [JsonProperty("DOMAIN")]
        public string Domain { get; set; }

        [JsonProperty("HOST")]
        public string Host { get; set; }

        [JsonProperty("REGION")]
        public string Region { get; set; }

        [JsonProperty("RESPONSETYPE")]
        public string Responsetype { get; set; }

        [JsonProperty("SCOPE")]
        public string Scope { get; set; }
    }

    public class Search
    {
        [JsonProperty("APPLICATION_ID")]
        public string ApplicationId { get; set; }

        [JsonProperty("INDEX_NAME")]
        public string IndexName { get; set; }

        [JsonProperty("SEARCH_ONLY_API_KEY")]
        public string SearchOnlyApiKey { get; set; }
    }

    public class ProductActivityResponse
    {
        [JsonProperty("ProductActivity")]
        public List<ProductActivity> ProductActivity { get; set; }

        [JsonProperty("Pagination")]
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        [JsonProperty("limit")]
        public double Limit { get; set; }

        [JsonProperty("page")]
        public double Page { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("sort")]
        public List<string> Sort { get; set; }

        [JsonProperty("order")]
        public List<string> Order { get; set; }

        [JsonProperty("lastPage")]
        public string LastPage { get; set; }

        [JsonProperty("currentPage")]
        public string CurrentPage { get; set; }

        [JsonProperty("nextPage")]
        public string NextPage { get; set; }

        [JsonProperty("prevPage")]
        public string PrevPage { get; set; }
    }

    public class ProductActivity
    {
        [JsonProperty("chainId")]
        public string ChainId { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("shoeSize")]
        public string ShoeSize { get; set; }

        [JsonProperty("productId")]
        public object ProductId { get; set; }

        [JsonProperty("skuUuid")]
        public string SkuUuid { get; set; }

        [JsonProperty("frequency")]
        public double Frequency { get; set; }

        [JsonProperty("state")]
        public double State { get; set; }

        [JsonProperty("customerId")]
        public object CustomerId { get; set; }

        [JsonProperty("localAmount")]
        public double LocalAmount { get; set; }

        [JsonProperty("localCurrency")]
        public string LocalCurrency { get; set; }
    }

    public class ProductList
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("itemCondition")]
        public string ItemCondition { get; set; }

        [JsonProperty("offers")]
        public Offers Offers { get; set; }
    }

    public class Offers
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("lowPrice")]
        public double LowPrice { get; set; }

        [JsonProperty("highPrice")]
        public double HighPrice { get; set; }

        [JsonProperty("priceCurrency")]
        public string PriceCurrency { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("availability")]
        public string Availability { get; set; }

        [JsonProperty("offers")]
        public StockXProxuct[] OffersOffers { get; set; }
    }

    public class StockXProxuct
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("availability")]
        public string Availability { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        public string ParentSku { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("priceCurrency")]
        public string PriceCurrency { get; set; }

        public string Url { get; set; }
    }

    public class GetPagedPortfolioItemsResponse
    {
        [JsonProperty("PortfolioItems")]
        public List<PortfolioItem> PortfolioItems { get; set; }

        [JsonProperty("Pagination")]
        public Pagination Pagination { get; set; }
    }

    public class PortfolioItem
    {
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
    }

    public class Customer
    {
        [JsonProperty("id")]
        public double Id { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("defaultSize")]
        public string DefaultSize { get; set; }

        [JsonProperty("defaultCategory")]
        public string DefaultCategory { get; set; }

        [JsonProperty("defaultCurrency")]
        public string DefaultCurrency { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("vacationDate")]
        public string VacationDate { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("flagged")]
        public bool Flagged { get; set; }

        [JsonProperty("hidePortfolioBanner")]
        public bool HidePortfolioBanner { get; set; }

        [JsonProperty("referUrl")]
        public string ReferUrl { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("createdAtTime")]
        public double CreatedAtTime { get; set; }

        [JsonProperty("isTrader")]
        public bool IsTrader { get; set; }

        [JsonProperty("hasBuyerReward")]
        public bool HasBuyerReward { get; set; }

        [JsonProperty("gdprStatus")]
        public string GdprStatus { get; set; }
    }

    public class PortfolioItemMeta
    {
        [JsonProperty("merchantSku")]
        public string MerchantSku { get; set; }

        [JsonProperty("discountCode")]
        public string DiscountCode { get; set; }

        [JsonProperty("sizePreferences")]
        public string SizePreferences { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("breadcrumbs")]
        public List<Breadcrumb> Breadcrumbs { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("charityCondition")]
        public double CharityCondition { get; set; }

        [JsonProperty("colorway")]
        public string Colorway { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }

        [JsonProperty("countryOfManufacture")]
        public string CountryOfManufacture { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("contentGroup")]
        public string ContentGroup { get; set; }

        [JsonProperty("ipoDate")]
        public string IpoDate { get; set; }

        [JsonProperty("minimumBid")]
        public double MinimumBid { get; set; }

        [JsonProperty("Doppelgangers")]
        public List<object> Doppelgangers { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isLithiumIonBattery")]
        public bool IsLithiumIonBattery { get; set; }

        [JsonProperty("traits")]
        public List<Trait> Traits { get; set; }

        [JsonProperty("PortfolioItems")]
        public List<object> PortfolioItems { get; set; }

        [JsonProperty("primaryCategory")]
        public string PrimaryCategory { get; set; }

        [JsonProperty("secondaryCategory")]
        public string SecondaryCategory { get; set; }

        [JsonProperty("usHtsCode")]
        public string UsHtsCode { get; set; }

        [JsonProperty("usHtsDescription")]
        public string UsHtsDescription { get; set; }

        [JsonProperty("productCategory")]
        public string ProductCategory { get; set; }

        [JsonProperty("releaseDate")]
        public DateTimeOffset? ReleaseDate { get; set; }

        [JsonProperty("retailPrice")]
        public double RetailPrice { get; set; }

        [JsonProperty("shoe")]
        public string Shoe { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("styleId")]
        public string StyleId { get; set; }

        [JsonProperty("tickerSymbol")]
        public string TickerSymbol { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("type")]
        public double Type { get; set; }

        [JsonProperty("sizeTitle")]
        public string SizeTitle { get; set; }

        [JsonProperty("sizeDescriptor")]
        public string SizeDescriptor { get; set; }

        [JsonProperty("sizeAllDescriptor")]
        public string SizeAllDescriptor { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("urlKey")]
        public string UrlKey { get; set; }

        [JsonProperty("year")]
        public double? Year { get; set; }

        [JsonProperty("shippingGroup")]
        public string ShippingGroup { get; set; }

        [JsonProperty("hold")]
        public bool Hold { get; set; }

        [JsonProperty("aLim")]
        public double ALim { get; set; }

        [JsonProperty("meta")]
        public ProductMeta Meta { get; set; }

        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("enhancedImage")]
        public EnhancedImage EnhancedImage { get; set; }

        [JsonProperty("children")]
        public Children Children { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentUuid")]
        public string ParentUuid { get; set; }

        [JsonProperty("sizeSortOrder")]
        public double SizeSortOrder { get; set; }

        [JsonProperty("shoeSize")]
        public string ShoeSize { get; set; }

        [JsonProperty("market")]
        public Market Market { get; set; }

        [JsonProperty("gtins")]
        public List<Gtin> Gtins { get; set; }

        [JsonProperty("upc")]
        public string Upc { get; set; }

        [JsonProperty("upcValid")]
        public bool UpcValid { get; set; }
    }

    public class Breadcrumb
    {
        [JsonProperty("level")]
        public double Level { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("isBrand")]
        public double IsBrand { get; set; }
    }

    public class Children
    {
    }

    public class EnhancedImage
    {
        [JsonProperty("productUuid", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductUuid { get; set; }

        [JsonProperty("imageHost", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ImageHost { get; set; }

        [JsonProperty("imageKey", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageKey { get; set; }

        [JsonProperty("imageCount", NullValueHandling = NullValueHandling.Ignore)]
        public double? ImageCount { get; set; }

        [JsonProperty("updatedAt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? CreatedAt { get; set; }
    }

    public class Gtin
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sku_id")]
        public string SkuId { get; set; }

        [JsonProperty("identifier_value")]
        public string IdentifierValue { get; set; }
    }

    public class Market
    {
        [JsonProperty("productId")]
        public double ProductId { get; set; }

        [JsonProperty("skuUuid")]
        public string SkuUuid { get; set; }

        [JsonProperty("productUuid")]
        public string ProductUuid { get; set; }

        [JsonProperty("lowestAsk")]
        public double LowestAsk { get; set; }

        [JsonProperty("lowestAskFloat")]
        public double LowestAskFloat { get; set; }

        [JsonProperty("lowestAskSize")]
        public string LowestAskSize { get; set; }

        [JsonProperty("parentLowestAsk")]
        public double ParentLowestAsk { get; set; }

        [JsonProperty("numberOfAsks")]
        public double NumberOfAsks { get; set; }

        [JsonProperty("salesThisPeriod")]
        public double SalesThisPeriod { get; set; }

        [JsonProperty("salesLastPeriod")]
        public double SalesLastPeriod { get; set; }

        [JsonProperty("highestBid")]
        public double HighestBid { get; set; }

        [JsonProperty("highestBidFloat")]
        public double HighestBidFloat { get; set; }

        [JsonProperty("highestBidSize")]
        public double? HighestBidSize { get; set; }

        [JsonProperty("numberOfBids")]
        public double NumberOfBids { get; set; }

        [JsonProperty("annualHigh")]
        public double AnnualHigh { get; set; }

        [JsonProperty("annualLow")]
        public double AnnualLow { get; set; }

        [JsonProperty("deadstockRangeLow")]
        public double DeadstockRangeLow { get; set; }

        [JsonProperty("deadstockRangeHigh")]
        public double DeadstockRangeHigh { get; set; }

        [JsonProperty("volatility")]
        public double Volatility { get; set; }

        [JsonProperty("deadstockSold")]
        public double DeadstockSold { get; set; }

        [JsonProperty("pricePremium")]
        public double PricePremium { get; set; }

        [JsonProperty("averageDeadstockPrice")]
        public double AverageDeadstockPrice { get; set; }

        [JsonProperty("lastSale")]
        public double LastSale { get; set; }

        [JsonProperty("lastSaleSize")]
        public double? LastSaleSize { get; set; }

        [JsonProperty("salesLast72Hours")]
        public double SalesLast72Hours { get; set; }

        [JsonProperty("changeValue")]
        public double ChangeValue { get; set; }

        [JsonProperty("changePercentage")]
        public double ChangePercentage { get; set; }

        [JsonProperty("absChangePercentage")]
        public double AbsChangePercentage { get; set; }

        [JsonProperty("totalDollars")]
        public double TotalDollars { get; set; }

        [JsonProperty("updatedAt")]
        public double UpdatedAt { get; set; }

        [JsonProperty("lastLowestAskTime")]
        public double LastLowestAskTime { get; set; }

        [JsonProperty("lastHighestBidTime")]
        public double? LastHighestBidTime { get; set; }

        [JsonProperty("lastSaleDate")]
        public DateTimeOffset? LastSaleDate { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("deadstockSoldRank")]
        public double DeadstockSoldRank { get; set; }

        [JsonProperty("pricePremiumRank")]
        public double PricePremiumRank { get; set; }

        [JsonProperty("averageDeadstockPriceRank")]
        public double AverageDeadstockPriceRank { get; set; }

        [JsonProperty("featured")]
        public double? Featured { get; set; }
    }

    public class Media
    {
        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("smallImageUrl")]
        public Uri SmallImageUrl { get; set; }

        [JsonProperty("thumbUrl")]
        public Uri ThumbUrl { get; set; }

        [JsonProperty("gallery")]
        public List<Uri> Gallery { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("has360")]
        public bool Has360 { get; set; }

        [JsonProperty("key360", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Key360 { get; set; }
    }

    public class ProductMeta
    {
        [JsonProperty("charity")]
        public bool Charity { get; set; }

        [JsonProperty("raffle")]
        public bool Raffle { get; set; }

        [JsonProperty("mobile_only")]
        public bool MobileOnly { get; set; }

        [JsonProperty("restock")]
        public bool Restock { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("hidden")]
        public bool Hidden { get; set; }

        [JsonProperty("lock_buying")]
        public bool LockBuying { get; set; }

        [JsonProperty("lock_selling")]
        public bool LockSelling { get; set; }

        [JsonProperty("redirected")]
        public bool Redirected { get; set; }
    }

    public class Trait
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }

        [JsonProperty("filterable")]
        public bool Filterable { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [JsonProperty("highlight")]
        public bool Highlight { get; set; }

        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }
    }

    public class Tracking
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("labelUrl")]
        public string LabelUrl { get; set; }

        [JsonProperty("formUrl")]
        public string FormUrl { get; set; }

        [JsonProperty("trackUrl")]
        public string TrackUrl { get; set; }

        [JsonProperty("deliveryDate")]
        public string DeliveryDate { get; set; }

        [JsonProperty("deliveryDateTime")]
        public bool DeliveryDateTime { get; set; }
    }

    public struct Value
    {
        public bool? Bool;
        public double? Integer;
        public string String;

        public static implicit operator Value(bool Bool) => new Value { Bool = Bool };

        public static implicit operator Value(double Integer) => new Value { Integer = Integer };

        public static implicit operator Value(string String) => new Value { String = String };
    }
}