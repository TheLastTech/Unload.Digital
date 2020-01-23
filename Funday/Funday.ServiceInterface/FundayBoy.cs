using Funday.ServiceModel.StockXAccount;
using Funday.ServiceModel.StockXListedItem;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using StockxApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public partial class FundayBoy : BackgroundWorker
    {
        private bool active;

        public bool Active
        {
            get => active; set
            {
                active = value;
            }
        }

        public string ThreadName = "Fredum";
        private static readonly ILog Logger = LogManager.LogFactory.GetLogger(typeof(FundayBoy));

        public FundayBoy()
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                var TotalUpdated = Db.UpdateOnly(() => new StockXAccount() { AccountThread = null, NextAccountInteraction = DateTime.Now });
            }
        

            DoWork += RunAccountJobs;
            RunWorkerCompleted += JobsCompletedRunAGain;
            ProgressChanged += FunBoy_ProgressChanged;
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
            RunWorkerAsync();
        }
        public static void UpdateThisThreadIsAlive(IDbConnection Db, string Name, string status = "Good", int UserId =-1)
        {
            try
            {
                var Item = new BoyStartUp()
                {
                    LastSeen = DateTime.Now,
                    Name = Name,
                    Status = status
                };
                Db.Save(Item);
            }catch(Exception ex)
            {
                AuditExtensions.CreateAudit(Db, UserId, "FunBoy", "UpdateThisThreadIsAlive", "Error", ex.Message, ex.StackTrace);
            }
        }

        private void RunAccountJobs(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
                {
                    UpdateThisThreadIsAlive(Db, ThreadName, "Starting ProcessUnAccounts");
                    ProcessUnAccounts(Db);
                    UpdateThisThreadIsAlive(Db, ThreadName, "Starting ProcessNextVerifiedAccount");
                    Task.WaitAll(ProcessNextVerifiedAccount(Db));
                    UpdateThisThreadIsAlive(Db, ThreadName);

                }
            }catch(Exception ex)
            {
                Logger.Error(ex);
                using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
                {
                    AuditExtensions.CreateAudit(Db, -1, "FunBoy", "RunAccountJobs", "Error", ex.Message, ex.StackTrace);
                }
           
            }
        }

        private static async Task ProcessNextVerifiedAccount(IDbConnection Db)
        {
            StockXAccount Login = null;

            var SgGetter = new StockxListingGetter(Db);
            try
            {
                Login = SgGetter.GetNextToUpdate();

                if (Login != null)
                {
                    await ProcessSold(Db, Login, SgGetter);

                    await ProcessListsings(Db, Login, SgGetter);
               
                }
                
            }
            catch (NeedsVerificaitonException nx)
            {
           
                Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(5), Verified = false }, A => A.Id == Login.Id);
            }catch(Exception ex)
            {
                if (Login != null)
                {
                    PlaceAccountBakkInQueue(Login, Db);
                }
            }
            if(Login != null)
            {
                PlaceAccountBakkInQueue(Login, Db);
            }
        }

        private static async Task ProcessSold(IDbConnection Db, StockXAccount Login, StockxListingGetter SgGetter)
        {
            List<PortfolioItem> ListedItems;
            try
            {
                ListedItems = await SgGetter.UpdateSoldToDb(Login);
            }
            catch (NeedsVerificaitonException nx)
            {
                throw nx;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                AuditExtensions.CreateAudit(Db, Login.Id, "FunBoy/ProcessNextVerifiedAccount", "ProcessSold", "Error", ex.Message, ex.StackTrace);
                
            }
        }

        private static async Task ProcessListsings(IDbConnection Db, StockXAccount Login, StockxListingGetter SgGetter)
        {
            List<PortfolioItem> ListedItems;
            try
            {
                ListedItems = await SgGetter.UpdateListingToDb(Login);
            }
            catch (NeedsVerificaitonException nx)
            {
                throw nx;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                AuditExtensions.CreateAudit(Db, Login.Id, "FunBoy/ProcessListsings", "UpdateListingtoDb", "Error", ex.Message, ex.StackTrace);
                return;
            }
            if (ListedItems == null)
            {
                return;
            }
            try
            {
                await SgGetter.UpdateListingsBidAsk(Login, ListedItems);
            }
            catch (NeedsVerificaitonException nx)
            {
                throw nx;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                AuditExtensions.CreateAudit(Db, Login.Id, "FunBoy/ProcessListsings", "UpdateListingsBidAsk", "Error", ex.Message, ex.StackTrace);
            }
        }

        private static void PlaceAccountBakkInQueue(StockXAccount Login, IDbConnection Db)
        {
            Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(2) }, A => A.Id == Login.Id);
        }

        private void ProcessUnAccounts(IDbConnection Db)
        {
            StockXAccount Login = null;
            try
            {
                var UAccounts = new UnAccounter(Db);
                Login = UAccounts.UpdateUnAccounts();
                if (Login == null) return;
                Task.WaitAll(UAccounts.VerifyStockXAccount(Login));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                if (Login == null) return;
                RetryVerificationLater(Db, Login);
            }
        }

        private static void RetryVerificationLater(IDbConnection Db, StockXAccount Login)
        {
            Db.UpdateOnly(() => new StockXAccount()
            {
                NextVerification = DateTime.Now.AddMinutes(5),
                AccountThread = ""
            }, A => A.Id == Login.Id);
        }

        private void FunBoy_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void JobsCompletedRunAGain(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(5000);
            RunWorkerAsync();
        }

        private class FunBoyAutoErrorResponse
        {
            public string error { get; set; }
        }
    }
}