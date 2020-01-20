using Funday.ServiceModel.StockXAccount;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Funday.ServiceInterface
{
    public static class Extensions
    {
        public static AppUser GetCurrentAppUser(this Service srvce)
        {
            CustomUserSession UserSession = (CustomUserSession)srvce.GetSession();
            var User = srvce.Db.Single<AppUser>(A => A.Email == UserSession.Email);
            return User;
        }
    }

    public class StockXAccountService : Service
    {
        private readonly IHostingEnvironment _env;
        private readonly IAppSettings _settings;
        private readonly IAuthRepository _authRepository;
        private static ILog Log = LogManager.GetLogger(typeof(StockXAccountService));

        public StockXAccountService(IAppSettings settings, IAuthRepository authRepository, IHostingEnvironment env)
        {
            _env = env;
            _settings = settings;
            _authRepository = authRepository;
        }

        private class ValidateListStockXAccount : AbstractValidator<ListStockXAccountRequest>
        {
            public ValidateListStockXAccount()
            {
                RuleFor(A => A.Skip).GreaterThan(-1);
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public ListStockXAccountResponse Get(ListStockXAccountRequest request)
        {
            var Validation = new ValidateListStockXAccount();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new ListStockXAccountResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            AppUser User = this.GetCurrentAppUser();
            var StockXAccounts = Db.Select(Db.From<StockXAccount>().Where(A => A.UserId == User.Id).OrderBy(A => A.Id).Skip(request.Skip).Take(50));
            var CountOf = Db.Count<StockXAccount>(A => A.UserId == User.Id);
            return new ListStockXAccountResponse()
            {
                Success = true,
                Total = CountOf,
                StockXAccounts = StockXAccounts
            };
        }

        private class ValidateUpdateStockXAccount : AbstractValidator<UpdateStockXAccountRequest>
        {
            public ValidateUpdateStockXAccount()
            {
               
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.ProxyUsername).NotEmpty();
                RuleFor(x => x.ProxyPassword).NotEmpty();
                RuleFor(x => x.ProxyHost).NotEmpty();
                RuleFor(x => x.ProxyActive).NotEmpty();
                RuleFor(x => x.Active).NotEmpty();
                RuleFor(x => x.Currency).NotEmpty();
                RuleFor(x => x.Country).NotEmpty();
                RuleFor(x => x.UserAgent).NotEmpty();
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public UpdateStockXAccountResponse Put(UpdateStockXAccountRequest request)
        {
            var Validation = new ValidateUpdateStockXAccount();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new UpdateStockXAccountResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            AppUser User = this.GetCurrentAppUser();
            var ExistingStockXAccount = Db.Single<StockXAccount>(A => A.Id == request.StockXAccountId && A.UserId == User.Id);
            if (ExistingStockXAccount == null)
            {
                return new UpdateStockXAccountResponse()
                {
                    Success = false,
                    Message = "No Such StockXAccount"
                };
            }

            ExistingStockXAccount.Email = request.Email;
            ExistingStockXAccount.Password = request.Password;
            ExistingStockXAccount.ProxyUsername = request.ProxyUsername;
            ExistingStockXAccount.ProxyPassword = request.ProxyPassword;
            ExistingStockXAccount.ProxyHost = request.ProxyHost;
            ExistingStockXAccount.ProxyPort = request.ProxyPort;
            ExistingStockXAccount.ProxyActive = request.ProxyActive;
            ExistingStockXAccount.Active = request.Active;
            ExistingStockXAccount.CustomerID = request.CustomerID;
            ExistingStockXAccount.Currency = request.Currency;
            ExistingStockXAccount.Country = request.Country;
        
            ExistingStockXAccount.Token = request.Token;
            ExistingStockXAccount.Disabled = request.Disabled;

            var TotalUpdated = Db.Update(ExistingStockXAccount);

            return new UpdateStockXAccountResponse()
            {
                TotalUpdated = TotalUpdated,
                Success = true,
            };
        }

        private class ValidateCreateStockXAccount : AbstractValidator<CreateStockXAccountRequest>
        {
            public ValidateCreateStockXAccount()
            {
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Password).NotEmpty();

                RuleFor(x => x.Country).NotEmpty();
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public async Task<CreateStockXAccountResponse> Post(CreateStockXAccountRequest request)
        {
            var Validation = new ValidateCreateStockXAccount();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new CreateStockXAccountResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }

            AppUser User = this.GetCurrentAppUser();
            var ExistingStockXAccount = Db.Single<StockXAccount>(A => User.Id == A.UserId && A.Email == request.Email);
            if (ExistingStockXAccount != null)
            {
                return new CreateStockXAccountResponse()
                {
                    Success = false,
                    Message = "StockXAccount already exists"
                };
            }

            var NewStockXAccount = new StockXAccount()
            {
                Email = request.Email,
                UserId = User.Id,
                Password = request.Password,
                ProxyUsername = request.ProxyUsername,
                ProxyPassword = request.ProxyPassword,
                ProxyHost = request.ProxyHost,
                ProxyPort = request.ProxyPort,
                ProxyActive = request.ProxyActive,
                 
            };

            try
            {
                NewStockXAccount.Country = request.Country;
                NewStockXAccount.Active = request.Active;
                NewStockXAccount.CustomerID = request.CustomerID;
                NewStockXAccount.Currency = request.Currency;
                NewStockXAccount.Country = request.Country;
                NewStockXAccount.NextVerification = DateTime.Now;
                NewStockXAccount.NextAccountInteraction = DateTime.Now;
                var InsertedId = Db.Insert(NewStockXAccount, true);
                return new CreateStockXAccountResponse()
                {
                    InsertedId = InsertedId,
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new CreateStockXAccountResponse()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        private class ValidateDeleteStockXAccount : AbstractValidator<DeleteStockXAccountRequest>
        {
            public ValidateDeleteStockXAccount()
            {
            }
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public DeleteStockXAccountResponse Delete(DeleteStockXAccountRequest request)
        {
            var Validation = new ValidateDeleteStockXAccount();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new DeleteStockXAccountResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            var User = this.GetCurrentAppUser();
            var ExistingStockXAccount = Db.Single<StockXAccount>(A => User.Id == A.UserId && A.Id == request.StockXAccountId);
            if (ExistingStockXAccount == null)
            {
                return new DeleteStockXAccountResponse()
                {
                    Success = false,
                    Message = "No Such StockXAccount"
                };
            }

            var Deleted = Db.DeleteById<StockXAccount>(request.StockXAccountId);

            return new DeleteStockXAccountResponse()
            {
                TotalDeleted = Deleted,
                Success = true,
            };
        }

        [Authenticate]
        // [RequiresAnyRole("")]
        public ListOneStockXAccountResponse Get(ListOneStockXAccountRequest request)
        {
            var User = this.GetCurrentAppUser();
            var ExistingStockXAccount = Db.Single<StockXAccount>(A => User.Id == A.UserId && A.Id == request.StockXAccountId);
            if (ExistingStockXAccount == null)
            {
                return new ListOneStockXAccountResponse()
                {
                    Success = false,
                    Message = "No Such StockXAccount"
                };
            }
            return new ListOneStockXAccountResponse()
            {
                StockXAccountItem = ExistingStockXAccount,
                Success = true,
            };
        }
    }
}