using Funday.ServiceInterface.StockxApi;
using Funday.ServiceModel.Inventory;
using Funday.ServiceModel.StockXAccount;
using Funday.ServiceModel.StockxInventoryStates;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using StockxApi;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public class SearchBoy : BackgroundWorker
    {
        private bool active;

        public bool Active
        {
            get => active; set
            {
                active = value;
            }
        }

        private readonly SearchBoy Noance;

        public SearchBoy()
        {
            Noance = this;

            DoWork += RunAccountJobs;
            RunWorkerCompleted += JobsCompletedRunAGain;
            ProgressChanged += FunBoy_ProgressChanged;
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
            RunWorkerAsync();
        }

        public async Task<LoginCookieToken> GetAuth(StockXAccount Auth)
        {
  
          
                var Output = await StockXApi.GetLogin(Auth); ;
                if (Output.Code == System.Net.HttpStatusCode.OK)
                {
                    return Output.RO;
                }

                throw new Exception(Output.Code + " : " + Output.ResultText);
    
       
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

        private void RunAccountJobs(object sender, DoWorkEventArgs e)
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {

                var ActiveInventory = Db.From<Inventory>().Where(A => A.Quantity > 0).Where(A => A.Active);
                
                var Inventories = Db.Select(ActiveInventory);

                foreach (var Inv in Inventories)
                {
                    Task.WaitAll(ProcessInventoryBids(Db, Inv));
                    Task.WaitAll(ProcessInventoryAsks(Db, Inv));

       
                }
            }
        }


        private async Task ProcessInventoryAsks(IDbConnection Db, Inventory Inv)
        {
            try
            {
                var DefaultAuth = new StockXAccount();
                var Bids = await DefaultAuth.GetASks(Inv);
                if (Bids.Code != System.Net.HttpStatusCode.OK)
                {
                    return;
                }
                if (Bids.RO.ProductActivity == null)
                {
                    return;
                }
                Db.Delete<StockXAsk>(A => A.Sku == Inv.Sku);
                foreach (var Act in Bids.RO.ProductActivity)
                {
                    Db.Save(new StockXAsk()
                    {
                        State= Act.State,
                        Sku = Act.SkuUuid,
                        Ask = (long)Act.LocalAmount,
                        ChainId = Act.ChainId,
                    });
                }
                var Pages = Bids.RO.Pagination;
                if (Pages == null)
                {
                    return;
                }
                var i = 2;
                while (Pages.NextPage != null)
                {
                    Bids = await DefaultAuth.GetASks(Inv, i++);
                    Pages = Bids.RO.Pagination;
                    if (Bids.Code != System.Net.HttpStatusCode.OK)
                    {
                        return;
                    }
                    if (Bids.RO.ProductActivity == null)
                    {
                        return;
                    }
                    foreach (var Act in Bids.RO.ProductActivity)
                    {
                        Db.Save(new StockXAsk()
                        {
                            State = Act.State,
                            Sku = Act.SkuUuid,
                            ChainId = Act.ChainId,
                            Ask = (long)Act.LocalAmount
                        });
                    }
                    if (Pages == null)
                    {
                        return;
                    }
                }
            }catch(Exception ex)
             {
                AuditExtensions.CreateAudit(Db, 1, "SearchBoy/ProcessInventoryAsks", "ProcessInventoryAsks", "Error", ex.Message, ex.StackTrace);
            }
            
        }

        private async Task ProcessInventoryBids(IDbConnection Db, Inventory Inv)
        {
            try
            {
                var DefaultAuth = new StockXAccount();
                StockXApiResult<ProductActivityResponse> Bids = await DefaultAuth.GetBids(Inv);
                if (Bids.Code != System.Net.HttpStatusCode.OK)
                {
                    return;
                }
                if (Bids.RO.ProductActivity == null)
                {
                    return;
                }
                Db.Delete<StockXBid>(A => A.Sku == Inv.Sku);
                foreach (var Act in Bids.RO.ProductActivity)
                {
                    Db.Save(new StockXBid()
                    {
                        State = Act.State,
                        ChainId = Act.ChainId,
                        Sku = Act.SkuUuid,
                        Bid = (long)Act.LocalAmount
                    });
                }
                var Pages = Bids.RO.Pagination;
                if (Pages == null)
                {
                    return;
                }
                var i = 2;

                while(Pages.NextPage !=null)
                {
                    Bids = await DefaultAuth.GetBids(Inv, i++);
                    Pages = Bids.RO.Pagination;
                 
                    if (Bids.Code != System.Net.HttpStatusCode.OK)
                    {
                        return;
                    }
                    if (Bids.RO.ProductActivity == null)
                    {
                        return;
                    }
                    foreach (var Act in Bids.RO.ProductActivity)
                    {
                        Db.Save(new StockXBid()
                        {
                            State = Act.State,
                            Sku = Act.SkuUuid,
                            ChainId = Act.ChainId,
                            Bid = (long)Act.LocalAmount
                        });
                    }
                    if (Pages == null)
                    {
                        return;
                    }
                }
                
            }catch(Exception ex)
            {
                AuditExtensions.CreateAudit(Db, 1, "SearchBoy/ProcessInventoryBids", "ProcessInventoryBidsst", "Error", ex.Message, ex.StackTrace);
            }
        }
    }
}