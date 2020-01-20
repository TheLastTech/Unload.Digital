
using ServiceStack.DataAnnotations;
namespace Funday.ServiceModel.StonksAccount
{
    public class StonksAccount 
    { 
        [AutoIncrement]
        [PrimaryKey]
        public int Id {get;set;}
    }
}