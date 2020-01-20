
module.exports.CreateRequest = (Name,NameSpace,CsOutput)=>
    `using ServiceStack;

namespace ${NameSpace}.ServiceModel.${Name}
{
    [Route("/${Name}/create", "Post")]
    public class Create${Name}Request : IReturn<Create${Name}Response>
    {
        ${CsOutput}
    }
    [Route("/${Name}/update", "Put")]
    public class Update${Name}Request : IReturn<Update${Name}Response>
    {
        ${CsOutput}
        public int ${Name}Id { get; set; }
    }
    [Route("/${Name}/delete", "Delete")]
    public class Delete${Name}Request : IReturn<Delete${Name}Response>
    {
        public int ${Name}Id { get; set; }
    }
    [Route("/${Name}/list", "Get")]
    public class List${Name}Request : IReturn<List${Name}Response>
    {
        public int Skip { get; set; }
    }
    [Route("/${Name}/listone", "Get")]
    public class ListOne${Name}Request : IReturn<ListOne${Name}Response>
    {
        public int ${Name}Id { get; set; }
    }
}
    `

module.exports.CreateDto = (Name,NameSpace,CsOutput)=>
    `
using ServiceStack.DataAnnotations;
namespace ${NameSpace}.ServiceModel.${Name}
{
    public class ${Name} 
    { 
        [AutoIncrement]
        [PrimaryKey]
        public int Id {get;set;}
        ${CsOutput}
    }
}`

module.exports.CreateResponse = (Name,NameSpace)=>
    `
using System.Collections.Generic;
namespace ${NameSpace}.ServiceModel.${Name}
{
    public class Create${Name}Response 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public long InsertedId {get; set; }
    }
    
      public class Update${Name}Response 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalUpdated {get; set;}
    }
    
      public class Delete${Name}Response 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TotalDeleted {get; set;}
    }
    
      public class List${Name}Response 
    { 
        public long Total { get;set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<${Name}> ${Name}s { get; set; }
    }
      public class ListOne${Name}Response 
    { 
        public bool Success { get; set; }
        public string Message { get; set; }
        public ${Name} ${Name}Item { get; set; }
    }
    
    
}
    `
