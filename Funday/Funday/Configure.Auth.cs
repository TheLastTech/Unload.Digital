using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace Funday
{

    // Custom Validator to add custom validators to built-in /register Service requiring DisplayName and ConfirmPassword
    public class CustomRegistrationValidator : RegistrationValidator
    {
        public CustomRegistrationValidator()
        {
            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.ConfirmPassword).NotEmpty();
            });
        }
    }

    public class ConfigureAuth : IConfigureAppHost, IConfigureServices
    {
        public void Configure(IServiceCollection services)
        {
            //services.AddSingleton<ICacheClient>(new MemoryCacheClient()); //Store User Sessions in Memory Cache (default)
        }
        private string EncodeTo64(string toEncode)

        {
            byte[] toEncodeAsBytes

                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }
        public void Configure(IAppHost appHost)
        {
            var UsernameRegex = new Regex(@"[\.\-\s\w\$\!\@\#\%\^\&\*\(\)]", RegexOptions.Compiled);
          
            var authKey = EncodeTo64("TimeToseelMyShoes" );

            var AppSettings = appHost.AppSettings;
            appHost.Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                new IAuthProvider[] {
                    new CredentialsAuthProvider(AppSettings){
                         SkipPasswordVerificationForInProcessRequests = true,
                        },     /* Sign In with Username / Password credentials */
                   new JwtAuthProvider(AppSettings) {
                        AuthKeyBase64 = authKey,
                        RequireSecureConnection =false,
                      },
                })
            {
                IsValidUsernameFn = (username) => UsernameRegex.IsMatch(username)
            });

            //override the default registration validation with your own custom implementation
            appHost.RegisterAs<CustomRegistrationValidator, IValidator<Register>>();
        }
    }
}
