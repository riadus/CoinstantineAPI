using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoinstantineAPI.Scheduler
{
    public class Scheduler : IScheduler
    {
        private readonly Dictionary<int, ScheduledTask> _tasks;
        private readonly Dictionary<int, ScheduledTask> _queuedTasks;
        private readonly Dictionary<int, ScheduledTask> _succeededTasks;
        private readonly Dictionary<int, ScheduledTask> _failedTasks;
        private bool _started;
        private static readonly SemaphoreSlim _semaphoreTasksList = new SemaphoreSlim(1, 1);

        public Scheduler()
        {
            _tasks = new Dictionary<int, ScheduledTask>();
            _queuedTasks = new Dictionary<int, ScheduledTask>();
            _succeededTasks = new Dictionary<int, ScheduledTask>();
            _failedTasks = new Dictionary<int, ScheduledTask>();

        }

        public async Task<int> ScheduleTask(ScheduledTask scheduledTask)
        {
            scheduledTask.Id = DateTime.Now.GetHashCode();
            await AddTask(scheduledTask);
            return scheduledTask.Id;
        }

        public async Task Start()
        {
            if(_started)
            {
                return;
            }
            try
            {
                _started = true;

                while (true)
                {
                    if (_queuedTasks.Any() || _succeededTasks.Any() || _failedTasks.Any())
                    {
                        await Task.Delay(1000);
                        Task.WaitAll(ExecuteQueuedTasks(), ExecuteSuceededTasks(), ExecuteFailedTasks(), ExecuteTimedOutTasks());
                    }
                    else
                    {
                        _started = false;
                        break;
                    }
                }
            }
            catch
            {
                _started = false;
                await Start();
            }
        }

        private async Task AddTask(ScheduledTask scheduledTask)
        {
            try
            {
                await _semaphoreTasksList.WaitAsync();

                _tasks.Add(scheduledTask.Id, scheduledTask);
                scheduledTask.Status = ScheduledTask.ScheduledTaskStatus.Queued;
                _queuedTasks.Add(scheduledTask.Id, scheduledTask);
            }
            finally
            {
                _semaphoreTasksList.Release();
            }
            await Task.Factory.StartNew(async() => await Start());
        }

        private async Task<IEnumerable<(int Id, Func<Task<bool?>> Task)>> GetQueuedTasks()
        {
            try
            {
                await _semaphoreTasksList.WaitAsync();

                var tasks = _queuedTasks.Where(t => !IsTimedOut(t.Value))
                                        .Select(t => (t.Key, t.Value.Task));
                return tasks;
            }
            finally
            {
                _semaphoreTasksList.Release();
            }
        }

        private async Task<IEnumerable<(int Id, Func<Task> Task)>> GetTimedOutTasks()
        {
            try
            {
                await _semaphoreTasksList.WaitAsync();

                var tasks = _queuedTasks.Where(t => IsTimedOut(t.Value))
                                        .Select(t => (t.Key, t.Value.TimeoutTask));
                return tasks;
            }
            finally
            {
                _semaphoreTasksList.Release();
            }
        }

        private bool IsTimedOut(ScheduledTask task)
        {
            return task.TimeStamp != null && (DateTime.Now - task.TimeStamp.Value).TotalSeconds > task.Timeout;
        }

        private async Task ExecuteQueuedTasks()
        {
            try
            {
                var tasks = await GetQueuedTasks();

                await _semaphoreTasksList.WaitAsync();

                var results = await Task.WhenAll(tasks.Select(x => x.Task()));
                for (var i = 0; i < results.Count(); i++)
                {
                    var task = tasks.ElementAt(i);
                    if (results[i] == null)
                    {
                        _tasks[task.Id].Status = ScheduledTask.ScheduledTaskStatus.Pending;
                        _queuedTasks[task.Id].TimeStamp = DateTime.Now;
                        continue;
                    }
                    if(results[i].Value)
                    {
                        _tasks[task.Id].Status = ScheduledTask.ScheduledTaskStatus.Succeeded;
                        _queuedTasks.Remove(task.Id);
                        _succeededTasks.Add(task.Id, _tasks[task.Id]);
                    }
                    else
                    {
                        _tasks[task.Id].Status = ScheduledTask.ScheduledTaskStatus.Failed;
                        _queuedTasks.Remove(task.Id);
                        _failedTasks.Add(task.Id, _tasks[task.Id]);
                    }
                }
            }
            finally
            {
                _semaphoreTasksList.Release();
            }
        }

        private async Task ExecuteSuceededTasks()
        {
            try
            {
                await _semaphoreTasksList.WaitAsync();
                foreach(var task in _succeededTasks)
                {
                    await task.Value.SuccessTask();
                }
                _succeededTasks.Clear();
            }
            finally
            {
                _semaphoreTasksList.Release();
            }
        }

        private async Task ExecuteFailedTasks()
        {
            try
            {
                await _semaphoreTasksList.WaitAsync();
                foreach (var task in _failedTasks)
                {
                    await task.Value.FailedTask();
                }
                _failedTasks.Clear();
            }
            finally
            {
                _semaphoreTasksList.Release();
            }
        }

        private async Task ExecuteTimedOutTasks()
        {
            try
            {
                var tasks = await GetTimedOutTasks();
                await _semaphoreTasksList.WaitAsync();
                foreach (var task in tasks)
                {
                    _queuedTasks.Remove(task.Id);
                    await task.Task();
                }
            }
            finally
            {
                _semaphoreTasksList.Release();
            }
        }
    }
}
