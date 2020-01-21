using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funday.ServiceModel.StockxInventoryStates
{
    public class StockXAsk
    {
        [PrimaryKey]
        public string ChainId { get; set; }
        public string Sku { get; set; }
        public long Ask { get; set; } 
        public double State { get; set; }
    }
}
