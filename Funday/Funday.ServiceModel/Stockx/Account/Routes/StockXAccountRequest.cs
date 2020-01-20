using ServiceStack;

namespace Funday.ServiceModel.StockXAccount
{
    [Route("/StockXAccount/create", "Post")]
    public class CreateStockXAccountRequest : IReturn<CreateStockXAccountResponse>
    {
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
public string UserAgent { get; set; }
public string Token { get; set; }

    }
    [Route("/StockXAccount/update", "Put")]
    public class UpdateStockXAccountRequest : IReturn<UpdateStockXAccountResponse>
    {
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
public string UserAgent { get; set; }
public string Token { get; set; }

        public int StockXAccountId { get; set; }
        public bool Disabled { get; set; }
    }
    [Route("/StockXAccount/delete", "Delete")]
    public class DeleteStockXAccountRequest : IReturn<DeleteStockXAccountResponse>
    {
        public int StockXAccountId { get; set; }
    }
    [Route("/StockXAccount/list", "Get")]
    public class ListStockXAccountRequest : IReturn<ListStockXAccountResponse>
    {
        public int Skip { get; set; }
    }
    [Route("/StockXAccount/listone", "Get")]
    public class ListOneStockXAccountRequest : IReturn<ListOneStockXAccountResponse>
    {
        public int StockXAccountId { get; set; }
    }
}
    