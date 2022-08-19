using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CoinstantineAPI.Encryption;
using FakeItEasy;
using Xunit;

namespace CoinstantineAPI.Tests.Scheduler
{
    public class SchedulerTests
    {
        [Fact]
        public async Task Scheduling_A_Task_Should_Succeed()
        {
            var theTaskThatSucceeds = A.Fake<Func<Task<bool?>>>();
            var successfulTask = A.Fake<Func<Task>>();
            A.CallTo(() => theTaskThatSucceeds()).Returns(Task.FromResult((bool?)true));
            A.CallTo(() => successfulTask()).Returns(Task.Delay(1000));

            var scheduledTask = new ScheduledTaskBuilder()
                .WithTask(theTaskThatSucceeds)
                .WithSuccessfulTask(successfulTask)
                .WithTimeout(60)
                .Build();

            var scheduler = new CoinstantineAPI.Scheduler.Scheduler();
            await scheduler.ScheduleTask(scheduledTask);
            await Task.Delay(10000);
            A.CallTo(() => scheduledTask.SuccessTask()).MustHaveHappened();
        }

        [Fact]
        public async Task Scheduling_A_Failing_Task_Should_Succeed()
        {
            var theTaskThatSucceeds = A.Fake<Func<Task<bool?>>>();
            var successfulTask = A.Fake<Func<Task>>();
            var failTask = A.Fake<Func<Task>>();
            A.CallTo(() => theTaskThatSucceeds()).Returns(Task.FromResult((bool?)false));
            A.CallTo(() => failTask()).Returns(Task.Delay(1000));

            var scheduledTask = new ScheduledTaskBuilder()
                .WithTask(theTaskThatSucceeds)
                .WithSuccessfulTask(successfulTask)
                .WithFailedTask(failTask)
                .WithTimeout(60)
                .Build();

            var scheduler = new CoinstantineAPI.Scheduler.Scheduler();
            await scheduler.ScheduleTask(scheduledTask);
            await Task.Delay(10000);
            A.CallTo(() => scheduledTask.SuccessTask()).MustNotHaveHappened();
            A.CallTo(() => scheduledTask.FailedTask()).MustHaveHappened();
        }

        [Fact]
        public async Task Scheduling_A_Task_That_Takes_Time_Should_Succeed()
        {
            var theTaskThatSucceeds = A.Fake<Func<Task<bool?>>>();
            var successfulTask = A.Fake<Func<Task>>();
            var failTask = A.Fake<Func<Task>>();
            A.CallTo(() => theTaskThatSucceeds()).ReturnsLazily(x => TaskThatTakesSomeTime(1000));
            A.CallTo(() => successfulTask()).Returns(Task.Delay(1000));

            var scheduledTask = new ScheduledTaskBuilder()
                .WithTask(theTaskThatSucceeds)
                .WithSuccessfulTask(successfulTask)
                .WithTimeout(60)
                .Build();

            var scheduler = new CoinstantineAPI.Scheduler.Scheduler();
            await scheduler.ScheduleTask(scheduledTask);
            await Task.Delay(10000);
            A.CallTo(() => scheduledTask.SuccessTask()).MustHaveHappened();
        }

        [Fact]
        public async Task Scheduling_A_Task_That_Takes_Too_Much_Time_Should_Time_Out()
        {
            var theTaskThatSucceeds = A.Fake<Func<Task<bool?>>>();
            var timeoutTask = A.Fake<Func<Task>>();
            var failTask = A.Fake<Func<Task>>();
            A.CallTo(() => theTaskThatSucceeds()).ReturnsLazily(x => TaskThatTakesSomeTime(1000));
            A.CallTo(() => timeoutTask()).Returns(Task.Delay(1000));

            var scheduledTask = new ScheduledTaskBuilder()
                .WithTask(theTaskThatSucceeds)
                .WithTimeoutTask(timeoutTask)
                .WithTimeout(1)
                .Build();

            var scheduler = new CoinstantineAPI.Scheduler.Scheduler();
            await scheduler.ScheduleTask(scheduledTask);
            await Task.Delay(10000);
            A.CallTo(() => scheduledTask.TimeoutTask()).MustHaveHappened();
            A.CallTo(() => scheduledTask.SuccessTask()).MustNotHaveHappened();
        }

        int _loops;

        private async Task<bool?> TaskThatTakesSomeTime(int time)
        {
            await Task.Delay(time);
            if (_loops < 2)
            {
                _loops++;
                return null;
            }
            return true;
        }
    }
}
