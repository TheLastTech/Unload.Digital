using Funday.ServiceModel.Audit;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;

namespace Funday.ServiceInterface
{
    public static class ListingExtensions
    {
        public static long CreateStockXListingEvent(string Name, string ChainId, string Sku, int Bid, int UserId,    string Additional = "")
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                var NewAudit = new StockXListingEvent()
                {
                    Amount= Bid,
                    Name = Name,
                    Sku = Sku,
                    UserId=UserId,
                    ChainId=ChainId,
                    Additional = Additional,
                    When = DateTime.Now
                };
                var InsertedId = Db.Insert(NewAudit, true);
                return InsertedId;
            }
        }
    }
}