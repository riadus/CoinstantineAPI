using System.Threading.Tasks;

namespace CoinstantineAPI.Core.Statistics
{
    public interface ICoinstantineStatistics
    {
        int Subscriptions { get; set; }
        int Valids { get; set; }
        int TwitterValidations { get; set; }
        int BctValidations { get; set; }
        int TelegramValidations { get; set; }
        int DiscordValidations { get; set; }
        string Print();
        Task<ICoinstantineStatistics> Check();
        Task<ICoinstantineStatistics> ForceCheck(string callerId);
    }
}