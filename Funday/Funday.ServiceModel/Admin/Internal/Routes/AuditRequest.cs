using ServiceStack;

namespace Funday.ServiceModel.Audit
{
    [Route("/Audit/create", "Post")]
    public class CreateAuditRequest : IReturn<CreateAuditResponse>
    {
        public long UserID { get; set; }
public long Location { get; set; }
public string ActionTaken { get; set; }
public string Result { get; set; }

    }
    [Route("/Audit/update", "Put")]
    public class UpdateAuditRequest : IReturn<UpdateAuditResponse>
    {
        public long UserID { get; set; }
public long Location { get; set; }
public string ActionTaken { get; set; }
public string Result { get; set; }

        public int AuditId { get; set; }
    }
    [Route("/Audit/delete", "Delete")]
    public class DeleteAuditRequest : IReturn<DeleteAuditResponse>
    {
        public int AuditId { get; set; }
    }
    [Route("/Audit/list", "Get")]
    public class ListAuditRequest : IReturn<ListAuditResponse>
    {
        public int Skip { get; set; }
    }
    [Route("/Audit/listone", "Get")]
    public class ListOneAuditRequest : IReturn<ListOneAuditResponse>
    {
        public int AuditId { get; set; }
    }
}
    