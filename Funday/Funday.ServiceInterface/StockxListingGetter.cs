using Funday.ServiceInterface.StockxApi;
using Funday.ServiceModel.Inventory;
using Funday.ServiceModel.StockXAccount;
using Funday.ServiceModel.StockxInventoryStates;
using Funday.ServiceModel.StockXListedItem;
using Newtonsoft.Json;
using ServiceStack.OrmLite;
using StockxApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public partial class FundayBoy
    {
        public class StockxListingGetter
        {
            public IDbConnection Db { get; }

            public StockxListingGetter(IDbConnection db)
            {
                Db = db;
            }

            public static string ThreadName = "Nancy";

            public StockXAccount GetNextToUpdate()
            {
                try
                {
                    var Sql = Db.From<StockXAccount>().Where(A => (A.AccountThread == null || A.AccountThread == "") && A.Verified && ((A.Active && !A.Disabled) && A.NextAccountInteraction <= DateTime.Now)).OrderBy(A => A.NextAccountInteraction).Take(1);
                    var TotalUpdated = Db.UpdateOnly(() => new StockXAccount() { AccountThread = ThreadName }, Sql);

                    if (TotalUpdated == 0)
                    {
                        return null;
                    }
                    return Db.Single(Db.From<StockXAccount>().Where(A => A.Verified && (A.Active && !A.Disabled)));
                }
                catch (Exception ex)
                {
                }
                return null;
            }

            public async Task<List<PortfolioItem>> UpdateSoldToDb(StockXAccount login)
            {
                List<PortfolioItem> ListedItems = await login.GetAllPending();
                if (ListedItems == null)
                {
                    Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(1) }, A => A.Id == login.Id);

                    return null;
                }

                foreach (var A in ListedItems)
                {
                    if (UpdateToSold(login, A))
                    {
                        Db.UpdateAdd(() => new Inventory() { TotalSold = 1 }, Db.From<Inventory>().Where(Ab => Ab.UserId == login.UserId && Ab.Sku == A.SkuUuid));
                    }
                }

                return ListedItems;
            }

            public async Task<List<PortfolioItem>> UpdateListingToDb(StockXAccount login)
            {
                List<PortfolioItem> ListedItems = await login.GetAllListings();
                if (ListedItems == null)
                {
                    Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(1) }, A => A.Id == login.Id);

                    return null;
                }

                bool Created = await CreateAnyNewInventory(login, ListedItems);
                if (Created)
                {
                    ListedItems = await login.GetAllListings();
                }

                foreach (var A in ListedItems)
                {
                    UpdateListingInDb(login, A);
                }

                return ListedItems;
            }

            private async Task<bool> CreateAnyNewInventory(StockXAccount login, List<PortfolioItem> ListedItems)
            {
                var Created = false;
                var AllInventory = Db.Select(Db.From<Inventory>().Where(A => A.UserId == login.UserId && A.Active && A.Quantity > 0));
                foreach (var tory in AllInventory)
                {
                    if (!ListedItems.Any(A => A.SkuUuid == tory.Sku))
                    {
                        var Listing = await login.MakeListing(tory.StockXUrl, tory.Sku, tory.StartingAsk, StockXAccount.MakeTimeString());
                        if ((int)Listing.Code > 399 && (int)Listing.Code < 500)
                        {
                            throw new NeedsVerificaitonException(login);
                        }
                        if (Listing.Code == System.Net.HttpStatusCode.OK)
                        {
                            Db.UpdateAdd(() => new Inventory() { Quantity = -1 }, Ab => Ab.Id == tory.Id);
                            AuditExtensions.CreateAudit(Db, login.Id, "StockxListingGetter", "Inventory Created", Listing.RO.PortfolioItem.ChainId);
                            Created = true;
                            continue;
                        }
                        return false; //error 500+ -- their server is down end task.
                    }
                }

                return Created;
            }

            public async Task<bool> UpdateListingsBidAsk(StockXAccount login, List<PortfolioItem> ListedItems)
            {
                var UpdatedAny = false;
                foreach (var Item in ListedItems)
                {
                    if (await ProcessListedItem(login, Item))
                    {
                        UpdatedAny = true;
                    }
                }
                return UpdatedAny;
            }

            private async Task<bool> ProcessListedItem(StockXAccount login, PortfolioItem Item)
            {
                Inventory Invntory = Db.Single<Inventory>(A => A.Active && A.Sku == Item.SkuUuid && A.UserId == login.UserId);
                if (Invntory == null)
                {
                    //  continue;
                    return false;
                }
 

                return await ProcessNewBid(login, Item, Invntory) || await ProcessNewAsk(login, Item, Invntory);
            }

            private async Task<bool> ProcessNewAsk(StockXAccount login, PortfolioItem Item, Inventory Invntory)
            {
                var Asks = Db.Select(Db.From<StockXAsk>().Where(I => I.Sku == Item.SkuUuid && I.Ask >= Invntory.MinSell + 1 && I.Ask < Item.Amount).OrderBy(A => A.Ask));
                var Ask = Asks.FirstOrDefault();
                if (Ask != null)
                {
                    var Result = await login.UpdateListing(Item.ChainId, Invntory.Sku, Item.ExpiresAt, (int)Ask.Ask - 1);
                    if ((int)Result.Code > 399 && (int)Result.Code < 500)
                    {
                        throw new NeedsVerificaitonException(login);
                    }
                    if (Result.Code == System.Net.HttpStatusCode.OK)
                    {
                        AuditExtensions.CreateAudit(Db, login.Id, "StockxListingGetter", $"Update Because Ask ({Item.Amount}) -> ({Ask.Ask})", Item.ChainId);
                        return true;
                    }
                    return true;
                }
                return false;
            }

            private async Task<bool> ProcessNewBid(StockXAccount login, PortfolioItem Item, Inventory Invntory)
            {
                var Bids = Db.Select(Db.From<StockXBid>().Where(I => I.Sku == Item.SkuUuid && I.Bid >= Invntory.MinSell && I.Bid < Invntory.StartingAsk).OrderByDescending(A => A.Bid));
                var Bid = Bids.FirstOrDefault();
                if (Bid != null)
                {
                    var Result = await login.UpdateListing(Item.ChainId, Invntory.Sku, Item.ExpiresAt, (int)Bid.Bid);
                    AuditExtensions.CreateAudit(Db, login.Id, "StockxListingGetter", $"Update Because Bid ({Item.Amount}) -> ({Bid.Bid})", JsonConvert.SerializeObject(Result));
                    if ((int)Result.Code > 399 && (int)Result.Code < 500)
                    {
                        throw new NeedsVerificaitonException(login);
                    }
                    if (Result.Code == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                return false;
            }

            private void UpdateListingInDb(StockXAccount login, PortfolioItem A)
            {
                var Existing = Db.Single<StockXListedItem>(b => b.ChainId == A.ChainId && b.UserId == login.UserId);
                if (Existing != null)
                {
                    StockXListedItem itm = A;
                    itm.Id = Existing.Id;
                    itm.Sold = Existing.Sold;
                    itm.UserId = login.UserId;
                    Db.Update(itm);
                }
                else
                {
                    StockXListedItem itm = A;
                    itm.UserId = login.UserId;
                    Db.Insert(itm);
                }
            }

            private bool UpdateToSold(StockXAccount login, PortfolioItem A)
            {
                var Existing = Db.Single<StockXListedItem>(b => b.ChainId == A.ChainId && b.UserId == login.UserId && !b.Sold);
                if (Existing != null)
                {
                    StockXListedItem itm = A;
                    itm.Id = Existing.Id;
                    itm.UserId = login.UserId;
                    itm.Sold = true;
                    Db.Update(itm);
                    return true;
                }
                else
                {
                    StockXListedItem itm = A;
                    itm.Sold = true;
                    itm.UserId = login.UserId;
                    Db.Insert(itm);
                    return false;
                }
            }
        }
    }
}