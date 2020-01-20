using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.Script;
using ServiceStack.DataAnnotations;
using Funday.ServiceModel;
using System.Threading.Tasks;
using Funday.ServiceModel.StockXAccount;
using Funday.ServiceInterface.StockxApi;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Funday.ServiceInterface
{
    [Exclude(Feature.Metadata)]
    [FallbackRoute("/{PathInfo*}", Matches = "AcceptsHtml")]
    public class FallbackForClientRoutes
    {

        public string PathInfo { get; set; }
    }

    public class MyServices : Service
    {
        //Return index.html for unmatched requests so routing is handled on client
        public object Any(FallbackForClientRoutes request) => Request.GetPageResult("/");

        public async Task<object> Any(Hello request)
        {
            return null;
     
        }

    }
}