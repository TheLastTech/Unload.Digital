
using ServiceStack.DataAnnotations;
namespace Funday.ServiceModel.Login
{
    public class Login 
    { 
        [AutoIncrement]
        [PrimaryKey]
        public int Id {get;set;}
    }
}