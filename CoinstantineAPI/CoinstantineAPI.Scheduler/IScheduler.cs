using System.Threading.Tasks;

namespace CoinstantineAPI.Scheduler
{
    public interface IScheduler
    {
        Task<int> ScheduleTask(ScheduledTask scheduledTask);
    }
}
