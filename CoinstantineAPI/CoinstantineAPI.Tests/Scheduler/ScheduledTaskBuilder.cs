using System;
using System.Threading.Tasks;
using CoinstantineAPI.Core.Blockchain;
using CoinstantineAPI.Scheduler;
using FakeItEasy;

namespace CoinstantineAPI.Tests.Scheduler
{
    public class ScheduledTaskBuilder
    {
        private Func<Task<bool?>> _theTask = A.Fake<Func<Task<bool?>>>();
        private Func<Task> _successfulTask = A.Fake<Func<Task>>();
        private Func<Task> _failedTask = A.Fake<Func<Task>>();
        private Func<Task> _timeoutTask = A.Fake<Func<Task>>();
        private int _timeout = 60;

        public ScheduledTask Build()
        {
            return new ScheduledTask
            {
                Task = _theTask,
                SuccessTask = _successfulTask,
                FailedTask = _failedTask,
                Timeout = _timeout,
                TimeoutTask = _timeoutTask
            };
        }

        public ScheduledTaskBuilder WithTask(Func<Task<bool?>> task)
        {
            _theTask = task;
            return this;
        }

        public ScheduledTaskBuilder WithSuccessfulTask(Func<Task> task)
        {
            _successfulTask = task;
            return this;
        }

        public ScheduledTaskBuilder WithFailedTask(Func<Task> task)
        {
            _failedTask = task;
            return this;
        }

        public ScheduledTaskBuilder WithTimeoutTask(Func<Task> task)
        {
            _timeoutTask = task;
            return this;
        }

        public ScheduledTaskBuilder WithTimeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }
    }
}