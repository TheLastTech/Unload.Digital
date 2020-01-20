using ServiceStack;

namespace Funday.ServiceModel.StonksAccount
{
    [Route("/StonksAccount/create", "Post")]
    public class CreateStonksAccountRequest : IReturn<CreateStonksAccountResponse>
    {
   
    }
    [Route("/StonksAccount/update", "Put")]
    public class UpdateStonksAccountRequest : IReturn<UpdateStonksAccountResponse>
    {
        public int StonksAccountId { get; set; }
    }
    [Route("/StonksAccount/delete", "Delete")]
    public class DeleteStonksAccountRequest : IReturn<DeleteStonksAccountResponse>
    {
        public int StonksAccountId { get; set; }
    }
    [Route("/StonksAccount/list", "Get")]
    public class ListStonksAccountRequest : IReturn<ListStonksAccountResponse>
    {
        public int Skip { get; set; }
    }
    [Route("/StonksAccount/listone", "Get")]
    public class ListOneStonksAccountRequest : IReturn<ListOneStonksAccountResponse>
    {
        public int StonksAccountId { get; set; }
    }
}
    