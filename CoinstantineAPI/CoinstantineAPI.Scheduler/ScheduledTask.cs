using System;
using System.Threading.Tasks;

namespace CoinstantineAPI.Scheduler
{
    public class ScheduledTask
    {
        public int Id { get; set; }
        public Func<Task<bool?>> Task { get; set; }
        public int Timeout { get; set; }
        public Func<Task> SuccessTask { get; set; }
        public Func<Task> FailedTask { get; set; }
        public Func<Task> TimeoutTask { get; set; }
        public ScheduledTaskStatus Status { get; set; }
        public DateTime? TimeStamp { get; set; }

        public enum ScheduledTaskStatus
        {
            NotSet,
            Queued,
            Pending,
            Succeeded,
            Failed,
            Canceled
        }
    }
}
