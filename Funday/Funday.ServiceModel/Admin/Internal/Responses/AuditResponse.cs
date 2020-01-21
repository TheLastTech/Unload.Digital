
using System.Collections.Generic;
namespace Funday.ServiceModel.Audit
{
    public class CreateAuditResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId {get; set; }
    }
    
      public class UpdateAuditResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    
      public class DeleteAuditResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class ListAuditResponse 
    { 
        public long Total { get;set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Audit> Audits { get; set; }
    }
      public class ListOneAuditResponse 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public Audit AuditItem { get; set; }
    }
    
    
}
    