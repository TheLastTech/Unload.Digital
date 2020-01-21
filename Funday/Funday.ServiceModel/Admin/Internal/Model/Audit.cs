
using ServiceStack.DataAnnotations;
namespace Funday.ServiceModel.Audit
{
    public class Audit 
    { 
        [AutoIncrement]
        [PrimaryKey]
        public int Id {get;set;}
        public long UserID { get; set; }
public string Location { get; set; }
public string ActionTaken { get; set; }
public string Result { get; set; }
        public string Error { get; set; }
        public string StackTrace { get; set; }
    }
}