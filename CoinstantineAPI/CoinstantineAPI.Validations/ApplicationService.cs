using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Core.Services;
using CoinstantineAPI.Core.Users;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.Users
{
    public class ApplicationService : IApplicationService
    {
        private readonly IContextProvider _contextProvider;
        private readonly ICodeGenerator _codeGenerator;

        public ApplicationService(IContextProvider contextProvider, ICodeGenerator codeGenerator)
        {
            _contextProvider = contextProvider;
            _codeGenerator = codeGenerator;
        }

        public async Task CreateApplication(string name, string description)
        {
            var application = new Application
            {
                Name = name,
                Description = description,
                ApplicationId = Guid.NewGuid().ToString(),
                ApplicationSecret = _codeGenerator.GenerateCode(32)
            };

            await SaveApplication(application);
        }

        private async Task SaveApplication(Application application)
        {
            using (var context = _contextProvider.CoinstantineContext)
            {
                if (await context.Applications.AnyAsync(x => x.Name == application.Name))
                {
                    return;
                }
                await context.Applications.AddAsync(application);
                await context.SaveChangesAsync();
            }
        }

        public Application GenerateIds()
        {
            return new Application
            {
                ApplicationId = Guid.NewGuid().ToString(),
                ApplicationSecret = _codeGenerator.GenerateCode(32)
            };
        }

        public Task CreateWebApplication(string clientId, string secret)
        {
            return SaveApplication(new Application
            {
                Name = "Web",
                Description = "Web Application",
                ApplicationId = clientId,
                ApplicationSecret = secret
            });
        }

        public Task CreateMobileApplication(string clientId, string secret)
        {
            return SaveApplication(new Application
            {
                Name = "Mobile",
                Description = "Mobile Application",
                ApplicationId = clientId,
                ApplicationSecret = secret
            });
        }
    }
}
