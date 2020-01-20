/* tslint:disable:no-trailing-whitespace */

module.exports.Create = (Name,NameSpace,CreateValidator,UpdateValidator,SetPut,UpdatePut)=>
    `using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Logging; 
using System.Linq;
using ${NameSpace}.ServiceModel;
using ServiceStack.OrmLite;
using ServiceStack.FluentValidation;
using ${NameSpace}.ServiceModel.${Name};

namespace ${NameSpace}.ServiceInterface
{
    public class ${Name}Service : Service
    {
        private readonly IHostingEnvironment _env;
        private readonly IAppSettings _settings;
        private readonly IAuthRepository _authRepository;
        private static ILog Log = LogManager.GetLogger(typeof(${Name}Service));
      

        public ${Name}Service(IAppSettings settings, IAuthRepository authRepository, IHostingEnvironment env )
        {
           
            _env = env;
            _settings = settings;
            _authRepository = authRepository;
        }
        
        private class ValidateList${Name} : AbstractValidator<List${Name}Request>
        {
            public ValidateList${Name}()
            {
                RuleFor(A => A.Skip).GreaterThan(-1);
            }
        }

        [Authenticate]
       // [RequiresAnyRole("")]
        public List${Name}Response Get(List${Name}Request request)
        {
        
              var Validation = new ValidateList${Name}();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new List${Name}Response()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\\n"),
                    Success = false
                };
            }
                    AppUser User = this.GetCurrentAppUser();
            var ${Name}s = Db.Select(Db.From<${Name}>().Where(A=>UserId==User.Id).OrderBy(A => A.Id).Skip(request.Skip).Take(50));
            var CountOf = Db.Count<${Name}>();
            return new List${Name}Response()
            {
                Success = true,
                Total =CountOf,
                ${Name}s = ${Name}s
               
            };
        
        
        }
         private class ValidateUpdate${Name} : AbstractValidator<Update${Name}Request>
        {
            public ValidateUpdate${Name}()
            {
                ${UpdateValidator}
            }
        }
        [Authenticate]
       // [RequiresAnyRole("")]
        public Update${Name}Response Put(Update${Name}Request request)
        {
        
                   var Validation = new ValidateUpdate${Name}();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new Update${Name}Response()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\\n"),
                    Success = false
                };
            }
                         AppUser User = this.GetCurrentAppUser();
            var Existing${Name} = Db.Single<${Name}>(A => A.UserId == User && A.Id == request.${Name}Id);
            if(Existing${Name} ==null)
            {
                return new Update${Name}Response()
                {
                    Success = false,
                    Message = "No Such ${Name}"
                };
            }
            ${UpdatePut}
            var TotalUpdated = Db.Update(Existing${Name});
        
            return new Update${Name}Response()
            {
                TotalUpdated = TotalUpdated,
                Success = true,
               
            };
        }
          private class ValidateCreate${Name} : AbstractValidator<Create${Name}Request>
        {
            public ValidateCreate${Name}()
            {
                ${CreateValidator}
            }
        }
        
        [Authenticate]
       // [RequiresAnyRole("")]
        public Create${Name}Response Post(Create${Name}Request request)
        {
                          var Validation = new ValidateCreate${Name}();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new Create${Name}Response()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\\n"),
                    Success = false
                };
            }
                   AppUser User = this.GetCurrentAppUser();
            var New${Name} = new ${Name}()
            {
                ${SetPut},
                UserId= User.Id
            };
            var InsertedId = Db.Insert(New${Name},true);
            return new Create${Name}Response()
            {
                InsertedId=InsertedId,
                Success = true,
               
            };
        }
           private class ValidateDelete${Name} : AbstractValidator<Delete${Name}Request>
        {
            public ValidateDelete${Name}()
            {

            }
        }
        [Authenticate]
       // [RequiresAnyRole("")]
        public Delete${Name}Response Delete(Delete${Name}Request request)
        {
                            var Validation = new ValidateDelete${Name}();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new Delete${Name}Response()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\\n"),
                    Success = false
                };
            }
            var Existing${Name} = Db.Single<${Name}>(A => A.Id == request.${Name}Id);
            if (Existing${Name} == null)
            {
                return new Delete${Name}Response()
                {
                    Success = false,
                    Message = "No Such ${Name}"
                };
            }
                    AppUser User = this.GetCurrentAppUser();
            var Deleted =Db.Delete<${Name}>(Db.From<$Name}>().Where(A=>A.Id== request.${Name}Id && A.UserId==User.Id));
            
            return new Delete${Name}Response()
            {
                TotalDeleted = Deleted,
                Success = true,
               
            };
        }
        
        [Authenticate]
       // [RequiresAnyRole("")]
        public ListOne${Name}Response Get(ListOne${Name}Request request)
        {
         AppUser User = this.GetCurrentAppUser();
            var Existing${Name} = Db.Single<${Name}>(A => User.Id == A.UserId && A.Id == request.${Name}Id);
            if(Existing${Name} ==null)
            {
                return new ListOne${Name}Response()
                {
                    Success = false,
                    Message = "No Such ${Name}"
                };
            }
            return new ListOne${Name}Response()
            {
                ${Name}Item = Existing${Name},
                Success = true,
               
            };
        }

     

      
    }
}
    `
