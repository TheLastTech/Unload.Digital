using Funday.ServiceInterface.StockxApi;
using Funday.ServiceModel.StockXAccount;
using Newtonsoft.Json;
using ServiceStack.OrmLite;
using StockxApi;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public class UnAccounter
    {
        public IDbConnection Db { get; }

        public UnAccounter(IDbConnection db)
        {
            Db = db;
        }

        public static string ThreadName = "Joe";

        public StockXAccount UpdateUnAccounts()
        {
            var Sql = Db.From<StockXAccount>().Where(A => (A.AccountThread == null || A.AccountThread == "") && !A.Verified && ((A.Active && !A.Disabled) && A.NextVerification <= DateTime.Now)).OrderBy(A => A.NextAccountInteraction).Take(1);
            var TotalUpdated = Db.UpdateOnly(() => new StockXAccount() { AccountThread = ThreadName }, Sql);

            if (TotalUpdated == 0)
            {
                return null;
            }
            return Db.Single(Db.From<StockXAccount>().Where(A => A.AccountThread == ThreadName && !A.Verified && (A.Active && !A.Disabled)));
        }

        public async Task<StockXAccount> VerifyStockXAccount(StockXAccount Account)
        {
            StockXApiResult<LoginCookieToken> Data;

            Data = await StockxApi.StockXApi.GetLogin(Account);
            switch (Data.Code)
            {
                case HttpStatusCode.OK:
                    Db.UpdateOnly(() => new StockXAccount()
                    {
                        Token = Data.RO.AccessToken,
                        CustomerID = Data.RO.CustomerInfo.Id,
                        Currency = Data.RO.CustomerInfo.DefaultCurrency,
                        Verified = true,
                        LoginFails = 0,
                        Disabled = false,
                        NextVerification=DateTime.Now.AddMinutes(5),
                        NextAccountInteraction=DateTime.Now,
                        AccountThread=""
                        
                    }, A => A.Id == Account.Id);

                    break;
                     
                default:
                    try
                    {
                        var ErrorMessage = JsonConvert.DeserializeObject<FunBoyAutoErrorResponse>(Data.ResultText);
                        AuditExtensions.CreateAudit(Db, Account.Id, "FunBoy/VerifyStockXAccount", "Login Failed", ErrorMessage.error);
                    }
                    catch (Exception ex) 
                    {
                    }
                    AuditExtensions.CreateAudit(Db, Account.Id, "FunBoy/VerifyStockXAccount", "Login Failed", Data.ResultText);
                    Account.LoginFails++;
                    Account.NextVerification = DateTime.Now.AddMinutes(Account.LoginFails - 1 * 1.5 + 1);
                    if (Account.LoginFails > 3)
                    {
                        DisableAccountDuetoLoginFailure(Account);
                        AuditExtensions.CreateAudit(Db, Account.Id, "FunBoy/VerifyStockXAccount", "Login Disabled",Data.ResultText);
                    }
                    else
                    {
                        PushFailedLoginBackIntoqueue(Account);
                    }

                    break;
            }

            return Account;
        }

        private void PushFailedLoginBackIntoqueue(StockXAccount Account)
        {
            Db.UpdateOnly(() => new StockXAccount()
            {
                LoginFails = Account.LoginFails,
                NextVerification = Account.NextVerification,
                AccountThread = ""

            }, A => A.Id == Account.Id);
        }

        private void DisableAccountDuetoLoginFailure(StockXAccount Account)
        {
            Db.UpdateOnly(() => new StockXAccount()
            {
                Disabled = false,
                LoginFails = Account.LoginFails,
                NextVerification = Account.NextVerification,
                AccountThread = ""
            }, A => A.Id == Account.Id);
        }
    }
}