using ServiceStack;

namespace Funday.ServiceModel.Login
{
    [Route("/Login/create", "Post")]
    public class CreateLoginRequest : IReturn<CreateLoginResponse>
    {
   
    }
    [Route("/Login/update", "Put")]
    public class UpdateLoginRequest : IReturn<UpdateLoginResponse>
    {
        public int LoginId { get; set; }
    }
    [Route("/Login/delete", "Delete")]
    public class DeleteLoginRequest : IReturn<DeleteLoginResponse>
    {
        public int LoginId { get; set; }
    }
    [Route("/Login/list", "Get")]
    public class ListLoginRequest : IReturn<ListLoginResponse>
    {
        public int Skip { get; set; }
    }
    [Route("/Login/listone", "Get")]
    public class ListOneLoginRequest : IReturn<ListOneLoginResponse>
    {
        public int LoginId { get; set; }
    }
}
    