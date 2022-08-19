using System.Threading.Tasks;
using CoinstantineAPI.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace CoinstantineAPI.IntegrationTests.Fixture
{
    public static class DbExtensions
    {
        public static async Task EnsureDatabaseEmpty(this IContext context)
        {
            context.Clear(context.AirdropDefinitions);
            context.Clear(context.Airdrops);
            context.Clear(context.AirdropSubscribers);
            context.Clear(context.AirdropSubscriptions);
            context.Clear(context.ApiUsers);
            context.Clear(context.BitcoinTalkAirdropRequirements);
            context.Clear(context.BitcoinTalkProfiles);
            context.Clear(context.BlockchainUsers);
            context.Clear(context.BuyTokensResults);
            context.Clear(context.SmartContracts);
            context.Clear(context.Subscribers);
            context.Clear(context.TelegramAirdropRequirements);
            context.Clear(context.TelegramProfiles);
            context.Clear(context.Tokens);
            context.Clear(context.TransactionReceipts);
            context.Clear(context.Translations);
            context.Clear(context.TwitterAirdropRequirements);
            context.Clear(context.TwitterProfiles);
            context.Clear(context.UserAirdrops);
            context.Clear(context.UserIdentities);
            await context.SaveChangesAsync();
        }

        public static void Clear<T>(this IContext context, DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
