using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Core.Database;
using CoinstantineAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoinstantineAPI.Blockchain
{
    public class PrivateKeyRetreiver : IPrivateKeyRetreiver
    {
        private readonly IContextProvider _contextProvider;
        private readonly ILogger _logger;

        public PrivateKeyRetreiver(IContextProvider contextProvider,
                                   ILoggerFactory loggerFactory)
        {
            _contextProvider = contextProvider;
            _logger = loggerFactory.CreateLogger(GetType());
        }
       
        public async Task<BlockchainUser> FromAccount(string accountAddress)
        {
            try
            {
                using (var context = _contextProvider.CoinstantineContext)
                {
                    return await context.BlockchainUsers.FirstOrDefaultAsync(b => b.Address == accountAddress);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in FromAccount()", accountAddress);
                throw ex;
            }
        }
    }
}