using CoinstantineAPI.Core.Email;
using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.Email
{
    public static class EmailDependencyInjection
    {
        public static IServiceCollection AddEmailServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEmailSender, EmailSender>();
            serviceCollection.AddTransient<IEmailService, EmailService>();
            return serviceCollection;
        }
    }
}
