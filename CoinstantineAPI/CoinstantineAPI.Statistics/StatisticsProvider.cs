using System.Threading.Tasks;
using CoinstantineAPI.Core.Statistics;

namespace CoinstantineAPI.Statistics
{
    public class StatisticsProvider : IStatisticsProvider
    {
        private readonly ICoinstantineStatistics _coinstantineStatistics;

        public StatisticsProvider(ICoinstantineStatistics coinstantineStatistics)
        {
            _coinstantineStatistics = coinstantineStatistics;
        }

        public async Task<ICoinstantineStatistics> GetStatistics()
        {
            var statistics = await _coinstantineStatistics.Check();
            return statistics;
        }

        public async Task<ICoinstantineStatistics> GetForcedStatistics(string id)
        {
            var statistics = await _coinstantineStatistics.ForceCheck(id);
            return statistics;
        }
    }
}
