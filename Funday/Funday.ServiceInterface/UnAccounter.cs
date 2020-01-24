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
                        NextVerification = DateTime.Now.AddMinutes(5),
                        NextAccountInteraction = DateTime.Now,
                        AccountThread = ""
                    }, A => A.Id == Account.Id);

                    break;

                default:
                    try
                    {
                        var ErrorMessage = JsonConvert.DeserializeObject<FunBoyAutoErrorResponse>(Data.ResultText);
                        if (ErrorMessage != null)
                        {
                            AuditExtensions.CreateAudit(Db, Account.Id, "FunBoy/VerifyStockXAccount", "Login Failed", ErrorMessage.error);
                            if (ErrorMessage.error.Contains(".wait() for") || ErrorMessage.error.Contains("capchta but solved"))
                            {
                                PushFailedLoginBackIntoqueue(Account);
                                return Account;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    Account.LoginFails++;
                    Account.NextVerification = DateTime.Now.AddMinutes(Account.LoginFails - 1 * 1.5 + 1);
                    if (Account.LoginFails > 12)
                    {
                        DisableAccountDuetoLoginFailure(Account);
                        AuditExtensions.CreateAudit(Db, Account.Id, "FunBoy/VerifyStockXAccount", "Login Disabled", Data.ResultText);
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
                Disabled = true,
                LoginFails = Account.LoginFails,
                NextVerification = Account.NextVerification,
                AccountThread = ""
            }, A => A.Id == Account.Id);
        }
    }
}