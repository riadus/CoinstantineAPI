using System.Threading.Tasks;

namespace CoinstantineAPI.Core.Statistics
{
    public interface IStatisticsProvider
    {
        Task<ICoinstantineStatistics> GetStatistics();
        Task<ICoinstantineStatistics> GetForcedStatistics(string id);
    }
}