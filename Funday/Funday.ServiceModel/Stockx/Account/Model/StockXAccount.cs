using ServiceStack.DataAnnotations;
using System;

namespace Funday.ServiceModel.StockXAccount
{
    public class StockXAccount
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }
        public string ProxyHost { get; set; }
        public long ProxyPort { get; set; }
        public bool ProxyActive { get; set; }
        public bool Active { get; set; }
        public long CustomerID { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; } 
        public string Token { get; set; }
        public int UserId { get; set; }
        public bool Verified { get; set; }
        public DateTime NextVerification { get; set; }
        public int LoginFails { get; set; }
        public bool Disabled { get; set; }
        [Index]
        public string AccountThread { get; set; }
        public DateTime NextAccountInteraction { get; set; }
        public static string MakeTimeString()
        {
            return DateTime.UtcNow.AddMonths(1).AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss+0000");
        }
    }
}