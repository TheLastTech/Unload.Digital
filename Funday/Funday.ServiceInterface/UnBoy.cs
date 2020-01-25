using Funday.ServiceInterface.StockxApi;
using Funday.ServiceModel.Inventory;
using Funday.ServiceModel.StockXAccount;
using Funday.ServiceModel.StockxInventoryStates;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using StockxApi;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public class UnBoy : BackgroundWorker
    {
        private bool active;

        public bool Active
        {
            get => active; set
            {
                active = value;
            }
        }
         
        private static readonly ILog Logger = LogManager.LogFactory.GetLogger(typeof(FundayBoy));

        public UnBoy()
        {
          

            DoWork += RunAccountJobs;
            RunWorkerCompleted += JobsCompletedRunAGain;
            ProgressChanged += FunBoy_ProgressChanged;
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
            RunWorkerAsync();
        }

      

        private void FunBoy_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void JobsCompletedRunAGain(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(15000);
            RunWorkerAsync();
        }
        public static string ThreadName = "Uncle";
        private void RunAccountJobs(object sender, DoWorkEventArgs e)
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {

                StockXAccount Login = null;
                try
                {
                    var UAccounts = new UnAccounter(Db);
                    Login = Db.GetNextToLogin(ThreadName);
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
        }
        private static void RetryVerificationLater(IDbConnection Db, StockXAccount Login)
        {
            Db.UpdateOnly(() => new StockXAccount()
            {
                NextVerification = DateTime.Now.AddMinutes(5),
                AccountThread = ""
            }, A => A.Id == Login.Id);
        }

    }
}