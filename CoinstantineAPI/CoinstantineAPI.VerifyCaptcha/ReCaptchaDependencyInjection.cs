using Microsoft.Extensions.DependencyInjection;

namespace CoinstantineAPI.VerifyCaptcha
{
    public static class ReCaptchaDependencyInjection
    {
        public static IServiceCollection AddReCaptchaValidation(this IServiceCollection services)
        {
            services.AddHttpClient<IReCaptchaValidator, ReCaptchaValidator>();
            services.AddTransient<ICheckCaptcha, CheckCaptcha>();

            return services;
        }
    }
}
