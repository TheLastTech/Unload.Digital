using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using Funday.ServiceInterface;
using ServiceStack.Script;
using ServiceStack.Web;
using System;
using ServiceStack.Text;
using ServiceStack.Logging;
using System.Reflection;
using ServiceStack.Data;
using System.Threading.Tasks;
using ServiceStack.Auth;
using ServiceStack.OrmLite;
using ServiceStack.Host.Handlers;
using ServiceStack.Logging.NLogger;
using Funday.ServiceModel.StockXAccount;
using Funday.ServiceModel.Inventory;
using Funday.ServiceModel.StockxInventoryStates;
using StockxApi;
 
using Microsoft.Extensions.Logging;
using Funday.ServiceModel.StockXListedItem;
using Funday.ServiceModel.Audit;

namespace Funday
{
    public class Startup : ModularStartup
    { 
        public Startup(IConfiguration configuration) : base(configuration, typeof(MyServices).Assembly) { }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
 
            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }
    public class AppHost : AppHostBase
    {
        public AppHost() : base("MuhBot", typeof(MyServices).Assembly)
        {
        }

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            Plugins.Add(new SharpPagesFeature()); // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages

            LogManager.LogFactory = new NLogFactory()
            {
                
            };

            SetConfig(new HostConfig
            {
                UseSameSiteCookies = true,
                AddRedirectParamsToQueryString = true,
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), HostingEnvironment.IsDevelopment()),
            });
            SetupLogging();
            SetupDatabase(container);
            SetupErrorHandling();
            Seed();
            JsConfig.TextCase = TextCase.PascalCase;
            JsConfig.DateHandler = DateHandler.UnixTimeMs;
            

            container.Register(c => new FundayBoy());
            container.Register(c => new SearchBoy());
            var SB = HostContext.Resolve<SearchBoy>();
            var FB = HostContext.Resolve<FundayBoy>();
        }

        private void Seed()
        {
            using (var Db = HostContext.Resolve<IDbConnectionFactory>().Open())
            {
                //if (!Db.TableExists<StockXAccount>())
        //        Db.DropAndCreateTable<StockXChildListing>();
                //      Db.DropAndCreateTable<StockXAccount>();
         //        Db.DropAndCreateTable<StockXListedItem>();
                //          Db.DropAndCreateTable<Inventory>();
          //      Db.DropAndCreateTable<BoyStartUp>();
                // Db.DropAndCreateTable<StockXProxuct>();
                //     Db.DropAndCreateTable<Audit>();
                //Db.DropAndCreateTable<StockXBid>();
                //Db.DropAndCreateTable<StockXAsk>();
                return;
#if DEBUG
                if (!Db.Exists<StockXAccount>(A => A.Email == "tenshihan@gmail.com"))
                {
                    Db.Insert(new StockXAccount()
                    {
                        Email = "tenshihan@gmail.com",
                        Password = "_xvNYPu27qVFg:c",
                        Currency = "USD",
                        Country = "US",
                       

                    });
                }
#endif
            }
        }

        private async Task<object> Poopsie(IRequest httpReq, object request, Exception ex)
        {
            var Logger = LogManager.GetLogger(request.GetType());
            Logger.Error(ex);
            return new object();
        }

        private async Task Oppsie(IRequest httpReq, IResponse httpRes, string operationName, Exception ex)
        {
            var Logger = LogManager.GetLogger(operationName);
            Logger.Error(ex);
        }
        public override void OnExceptionTypeFilter(Exception ex, ResponseStatus responseStatus)
        {
            var Logger = LogManager.GetLogger(ex.Source.IsEmpty() ? "Undefined" : ex.Source);
            Logger.Error("```" + ex.Message + Environment.NewLine + ex.StackTrace + "```");
            base.OnExceptionTypeFilter(ex, responseStatus);
        }

        private void SetupErrorHandling()
        {
            UncaughtExceptionHandlersAsync.Add(Oppsie);
            ServiceExceptionHandlersAsync.Add(Poopsie);
     
        }
        private static void SetupLogging()
        {
           
     
              
            //var Logger = LogManager.GetLogger("Application Start Up Logger");
            //Logger.Info("Reactor Online");
        }

        private void SetupDatabase(Container container)
        {
            var connectionString = AppSettings.Get<string>("database:connectionString");
            var Db = new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider);
            container.Register<IDbConnectionFactory>(c => Db);
            var OrmAuthRepo = new OrmLiteAuthRepository<AppUser, UserAuthDetails>(Db);
            
            container.Register<IAuthRepository>(c => OrmAuthRepo);
        }

    }

 
}