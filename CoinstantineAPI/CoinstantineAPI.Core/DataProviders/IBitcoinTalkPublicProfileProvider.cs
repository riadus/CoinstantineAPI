using System.Threading.Tasks;
using CoinstantineAPI.Data;

namespace CoinstantineAPI.Core.DataProvider
{
    public interface IBitcoinTalkPublicProfileProvider
    {
        Task<BitcoinTalkProfile> GetUser(int id);
    }
}
