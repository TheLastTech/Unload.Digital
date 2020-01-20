using Funday.ServiceInterface.StockxApi;
using Funday.ServiceModel.StockXAccount;
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

        private readonly FundayBoy Noance;
        private static readonly ILog Logger = LogManager.LogFactory.GetLogger(typeof(FundayBoy));

        public FundayBoy()
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                var TotalUpdated = Db.UpdateOnly(() => new StockXAccount() { AccountThread = null, NextAccountInteraction = DateTime.Now });
            }
            Noance = this;

            DoWork += RunAccountJobs;
            RunWorkerCompleted += JobsCompletedRunAGain;
            ProgressChanged += FunBoy_ProgressChanged;
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
            RunWorkerAsync();
        }

        private void RunAccountJobs(object sender, DoWorkEventArgs e)
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                    ProcessUnAccounts(Db);
                    Task.WaitAll(ProcessNextVerifiedAccount(Db));
                
            }
        }

        private static async Task ProcessNextVerifiedAccount(IDbConnection Db)
        {
            StockXAccount Login = null;
            var SgGetter = new StockxListingGetter(Db);
            try
            {
                Login = SgGetter.GetNextToUpdate();
            }
            catch (Exception ex)
            {
                LogInnerError(Db, Login);
                return;
            }
            if (Login == null) { return; }
            try
            {
                await ProcessSold(Db, Login, SgGetter);

                await ProcessListsings(Db, Login, SgGetter);
            }catch(Exception ex)
            {
                Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(5), Verified = false }, A => A.Id == Login.Id);
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
                Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(5), Verified = false }, A => A.Id == Login.Id);
                return;
            }
            catch (Exception ex)
            {
                LogInnerError(Db, Login);
                return;
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
                Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(5), Verified = false }, A => A.Id == Login.Id);
                return;
            }
            catch (Exception ex)
            {
                LogInnerError(Db, Login);
                return;
            }
            if (ListedItems == null)
            {
                PlaceAccountBakkInQueue(Login, Db);
                return;
            }
            try
            {
                await SgGetter.UpdateListingsBidAsk(Login, ListedItems);
                PlaceAccountBakkInQueue(Login, Db);
            }
            catch (NeedsVerificaitonException nx)
            {
                Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(5), Verified = false }, A => A.Id == Login.Id);
            }
            catch (Exception ex)
            {
                LogInnerError(Db, Login);
            }
        }

        private static void PlaceAccountBakkInQueue(StockXAccount Login, IDbConnection Db)
        {
            Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(2) }, A => A.Id == Login.Id);
        }

        private static void LogInnerError(IDbConnection Db, StockXAccount Login)
        {
            try
            {
                Db.UpdateOnly(() => new StockXAccount() { AccountThread = "", NextAccountInteraction = DateTime.Now.AddMinutes(1) }, A => A.Id == Login.Id);
            }
            catch (Exception ex2)
            {
                Console.Write(ex2);
            }
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