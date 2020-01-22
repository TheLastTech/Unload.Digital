using Funday.ServiceModel.Audit;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using System;
using System.Data;
using System.Linq;

namespace Funday.ServiceInterface
{
    public static class AuditExtensions
    {
        public static long CreateAudit(this IDbConnection Db, int UserID, string Location, string ActionTaken,string Result="", string Error="", string StackTrace="")
        {
            var NewAudit = new Audit()
            {
                UserID = UserID,
                Location = Location,
                ActionTaken = ActionTaken,
                Result = Result,
                Error = Error,
                StackTrace = StackTrace,
                When=DateTime.Now
            };
            var InsertedId = Db.Insert(NewAudit, true);
            return InsertedId;
        }
    }
    public class AuditService : Service
    {
        private readonly IHostingEnvironment _env;
        private readonly IAppSettings _settings;
        private readonly IAuthRepository _authRepository;
        private static ILog Log = LogManager.GetLogger(typeof(AuditService));

        public AuditService(IAppSettings settings, IAuthRepository authRepository, IHostingEnvironment env)
        {
            _env = env;
            _settings = settings;
            _authRepository = authRepository;
        }

        private class ValidateListAudit : AbstractValidator<ListAuditRequest>
        {
            public ValidateListAudit()
            {
                RuleFor(A => A.Skip).GreaterThan(-1);
            }
        }

        [Authenticate]
        [RequiresAnyRole("Admin")]
        public ListAuditResponse Get(ListAuditRequest request)
        {
            var Validation = new ValidateListAudit();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new ListAuditResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            AppUser User = this.GetCurrentAppUser();
            var Audits = Db.Select(Db.From<Audit>().OrderBy(A => A.Id).Skip(request.Skip).Take(50));
            var CountOf = Db.Count<Audit>();
            return new ListAuditResponse()
            {
                Success = true,
                Total = CountOf,
                Audits = Audits
            };
        }

    
        [Authenticate]
         [RequiresAnyRole("Admin")]
        public ListOneAuditResponse Get(ListOneAuditRequest request)
        {
            AppUser User = this.GetCurrentAppUser();
            var ExistingAudit = Db.Single<Audit>(A =>  A.Id == request.AuditId);
            if (ExistingAudit == null)
            {
                return new ListOneAuditResponse()
                {
                    Success = false,
                    Message = "No Such Audit"
                };
            }
            return new ListOneAuditResponse()
            {
                AuditItem = ExistingAudit,
                Success = true,
            };
        }
    }
}