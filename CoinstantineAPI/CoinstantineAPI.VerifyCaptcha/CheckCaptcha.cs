using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.VerifyCaptcha
{
    public class CheckCaptcha : ICheckCaptcha
    {
        private readonly IContextProvider _contextProvider;

        public CheckCaptcha(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public async Task<bool> ShouldCheck(string clientId, string secret)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                var application = await context.Applications.FirstOrDefaultAsync(x => x.ApplicationId == clientId && x.ApplicationSecret == secret);
                return application != null && application.Name == "Web";
            }
        }
    }
}
