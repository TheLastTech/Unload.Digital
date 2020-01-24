using Funday.ServiceModel.StockXAccount;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using System;
using System.Data;
using System.Linq;

namespace Funday.ServiceInterface
{
    public static class DbExtensions
    {
        private static readonly ILog Logger = LogManager.LogFactory.GetLogger(typeof(FundayBoy));
        public static StockXAccount GetNextToUpdate(this IDbConnection Db, string ThreadName)
        {
            try
            {
                var Sql = Db.From<StockXAccount>().Where(A => (A.AccountThread == null || A.AccountThread.Length == 0) && A.Verified && ((A.Active && !A.Disabled) && A.NextAccountInteraction <= DateTime.Now)).OrderBy(A => A.NextAccountInteraction).Take(1);
                var Item = Db.Single(Sql);
                if (Item == null)
                {
                    return null;
                }
                var TotalUpdated = Db.UpdateOnly(() => new StockXAccount() { AccountThread = ThreadName }, A => A.Id == Item.Id && (A.AccountThread == null || A.AccountThread.Length == 0));

                if (TotalUpdated == 0)
                {
                    return null;
                }
                return Db.Single(Db.From<StockXAccount>().Where(A => A.Verified && A.AccountThread == ThreadName && (A.Active && !A.Disabled)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }
        public static StockXAccount GetNextToLogin(this IDbConnection Db, string ThreadName)
        {
            try
            {
                var Sql = Db.From<StockXAccount>().Where(A => (A.AccountThread == null || A.AccountThread.Length == 0) && !A.Verified && ((A.Active && !A.Disabled) && A.NextVerification <= DateTime.Now)).OrderBy(A => A.NextVerification).Take(1);
                var Item = Db.Single(Sql);
                if (Item == null)
                {
                    return null;
                }
                var TotalUpdated = Db.UpdateOnly(() => new StockXAccount() { AccountThread = ThreadName }, A => A.Id == Item.Id && (A.AccountThread == null || A.AccountThread.Length == 0));

                if (TotalUpdated == 0)
                {
                    return null;
                }
                return Db.Single(Db.From<StockXAccount>().Where(A => A.Verified && A.AccountThread == ThreadName && (A.Active && !A.Disabled)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return null;
        }
    }
}