using Funday.ServiceModel.DiscordNotifications;
using Microsoft.AspNetCore.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using System.Linq;

namespace Funday.ServiceInterface
{
    public class DiscordNotificationsService : Service
    {
        private readonly IHostingEnvironment _env;
        private readonly IAppSettings _settings;
        private readonly IAuthRepository _authRepository;
        private static ILog Log = LogManager.GetLogger(typeof(DiscordNotificationsService));

        public DiscordNotificationsService(IAppSettings settings, IAuthRepository authRepository, IHostingEnvironment env)
        {
            _env = env;
            _settings = settings;
            _authRepository = authRepository;
        }
        private class ValidateUpdateDiscordNotifications : AbstractValidator<UpdateDiscordNotificationsRequest>
        {
            public ValidateUpdateDiscordNotifications()
            {

            }
        }
        [Authenticate]
        // [RequiresAnyRole("")]
        public UpdateDiscordNotificationsResponse Put(UpdateDiscordNotificationsRequest request)
        {
            var Validation = new ValidateUpdateDiscordNotifications();
            var Valid = Validation.Validate(request);
            if (!Valid.IsValid)
            {
                return new UpdateDiscordNotificationsResponse()
                {
                    Message = Valid.Errors.Select(A => A.ErrorMessage).Join("\n"),
                    Success = false
                };
            }
            AppUser User = this.GetCurrentAppUser();
            var ExistingDiscordNotifications = Db.Single<DiscordNotifications>(A => User.Id == A.UserId);
            if (ExistingDiscordNotifications == null)
            {
                return new UpdateDiscordNotificationsResponse()
                {
                    Success = false,
                    Message = "No Such DiscordNotifications"
                };
            }
            ExistingDiscordNotifications.Sold = request.Sold;
            ExistingDiscordNotifications.Error = request.Error;
            ExistingDiscordNotifications.Listing = request.Listing;

            var TotalUpdated = Db.Update(ExistingDiscordNotifications);

            return new UpdateDiscordNotificationsResponse()
            {
                TotalUpdated = TotalUpdated,
                Success = true,
            };
        }

 
        [Authenticate]
        // [RequiresAnyRole("")]
        public ListOneDiscordNotificationsResponse Get(ListOneDiscordNotificationsRequest request)
        {
            AppUser User = this.GetCurrentAppUser();
            var ExistingDiscordNotifications = Db.Single<DiscordNotifications>(A => User.Id == A.UserId);
            if (ExistingDiscordNotifications == null)
            {
                var NewDiscordNotifications = new DiscordNotifications()
                {
                    UserId = User.Id,
                    Sold = "",
                    Listing = "",
                    Error = ""
                };
                NewDiscordNotifications.Id = (int)Db.Insert(NewDiscordNotifications, true);
                return new ListOneDiscordNotificationsResponse()
                {
                    DiscordNotificationsItem= NewDiscordNotifications,
                    Success = true,
                };
            }
            return new ListOneDiscordNotificationsResponse()
            {
                DiscordNotificationsItem = ExistingDiscordNotifications,
                Success = true,
            };
        }
    }
}