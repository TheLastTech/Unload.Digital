using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Web;
using ServiceStack.Auth;
using ServiceStack.Configuration;

namespace Funday
{


    public class AppUserAuthEvents : AuthEvents
    {
        public override void OnAuthenticated(IRequest req, IAuthSession session, IServiceBase authService,
            IAuthTokens tokens, Dictionary<string, string> authInfo)
        {
            var authRepo = HostContext.AppHost.GetAuthRepository(req);
            using (authRepo as IDisposable)
            {
                var userAuth = (AppUser)authRepo.GetUserAuth(session.UserAuthId);
                userAuth.ProfileUrl = session.GetProfileUrl();
                userAuth.LastLoginIp = req.UserHostAddress;
                userAuth.LastLoginDate = DateTime.UtcNow;
                authRepo.SaveUserAuth(userAuth);
            }
        }
    }

    public class ConfigureAuthRepository : IConfigureAppHost, IConfigureServices, IPreInitPlugin
    {
        public void Configure(IServiceCollection services)
        {

        }

        public void Configure(IAppHost appHost)
        {
            var authRepo = appHost.Resolve<IAuthRepository>();
            authRepo.InitSchema();

            CreateUser(authRepo, "kooshesh@react-coder.com", "Ari", "Ender", "ItKN$!Mh1r8EeY^Cd^2", roles: new[] { RoleNames.Admin });

        }

        public void BeforePluginsLoaded(IAppHost appHost)
        {
            appHost.AssertPlugin<AuthFeature>().AuthEvents.Add(new AppUserAuthEvents());
        }

        // Add initial Users to the configured Auth Repository
        public void CreateUser(IAuthRepository authRepo, string email, string Username, string name, string password, string[] roles)
        {
            if (authRepo.GetUserAuthByUserName(email) == null)
            {
                var newAdmin = new AppUser { Email = email, UserName = Username, DisplayName = name };
                var user = authRepo.CreateUserAuth(newAdmin, password);
                authRepo.AssignRoles(user, roles);
            }
        }
    }
}
