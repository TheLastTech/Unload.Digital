using ServiceStack;

namespace Funday.ServiceModel.StockXListedItem
{
    [Route("/StockXListedItem/create", "Post")]
    public class CreateStockXListedItemRequest : IReturn<CreateStockXListedItemResponse>
    {
        public string StockXUrl { get; set; }

    }
    [Route("/StockXListedItem/update", "Put")]
    public class UpdateStockXListedItemRequest : IReturn<UpdateStockXListedItemResponse>
    {
        public string StockXUrl { get; set; }

        public int StockXListedItemId { get; set; }
    }
    [Route("/StockXListedItem/delete", "Delete")]
    public class DeleteStockXListedItemRequest : IReturn<DeleteStockXListedItemResponse>
    {
        public int StockXListedItemId { get; set; }
    }
    [Route("/StockXListedItem/list", "Get")]
    public class ListStockXListedItemRequest : IReturn<ListStockXListedItemResponse>
    {
        public int Skip { get; set; }
    }
    [Route("/StockXListedItem/listone", "Get")]
    public class ListOneStockXListedItemRequest : IReturn<ListOneStockXListedItemResponse>
    {
        public int StockXListedItemId { get; set; }
    }
}
    