using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Core.Validations;
using CoinstantineAPI.Users.Referals;
using CoinstantineAPI.Users.Unicity;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Users
{
    public static class UsersDependencyInjection
    {
        public static IServiceCollection AddUsersServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUsersService, UsersService>();
            serviceCollection.AddTransient<IUserCreationService, UserCreationService>();
            serviceCollection.AddTransient<ITokenService, TokenService>();
            serviceCollection.AddTransient<IPasswordService, PasswordService>();
            serviceCollection.AddTransient<ICodeGenerator, CodeGenerator>();
            serviceCollection.AddTransient<IAuthService, AuthService>();
            serviceCollection.AddTransient<IUnicityConstraintsChecker, UnicityConstraintsChecker>();
            serviceCollection.AddTransient<IUnicityConstraintsFactory, UnicityConstraintsFactory>();
            serviceCollection.AddTransient<ITwitterService, TwitterService>();
            serviceCollection.AddSingleton<ITelegramService, TelegramService>();
            serviceCollection.AddSingleton<IBitcoinTalkService, BitcoinTalkService>();
            serviceCollection.AddSingleton<IApplicationService, ApplicationService>();
            serviceCollection.AddSingleton<ISingletonTwitterService, SingletonTwitterService>();
            serviceCollection.AddTransient<IReferralService, ReferralService>();
            return serviceCollection;
        }
    }
}
