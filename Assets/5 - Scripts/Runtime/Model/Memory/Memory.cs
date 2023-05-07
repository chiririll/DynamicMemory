using DynamicMem.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace DynamicMem.Model
{
    public class MemoryManager
    {
        private readonly MemoryConfig config;

        private readonly Subject<Task> onTaskMoved = new();

        private MemoryInfo memory;
        private Defragmentator defragmentator;

        public MemoryManager(MemoryConfig config)
        {
            this.config = config;

            ReloadConfig();

            DI.Add(this);
            this.LogMsg("Initialized");
        }

        public IEnumerable<ITask> Queue => memory.Queue;
        public IReadOnlyList<ITask> LoadedTasks => memory.Memory;
        public int Size => memory.Size;

        public IObservable<ITask> OnTaskEnqueue => memory.OnTaskEnqueue;
        public IObservable<ITask> OnTaskLoaded => memory.OnTaskLoaded;
        public IObservable<ITask> OnTaskMoved => onTaskMoved;
        public IObservable<ITask> OnTaskUnloaded => memory.OnTaskUnloaded;

        public void ReloadConfig()
        {
            memory = new(config.Size);
            defragmentator = new(this);

            // TODO: OS task
        }

        #region Tick
        public void Tick()
        {
            if (defragmentator.Running)
            {
                defragmentator.Tick();
                return;
            }
             
            TickTasks();
            UnloadFinished();
            LoadFromQueue();

            UnityEngine.Debug.Log("Memory Tick");
        }

        private void TickTasks()
        {
            foreach (var task in memory.Memory)
            {
                if (task.Status != Task.State.Running)
                    continue;

                task.Tick();
            }
        }

        private void UnloadFinished()
        {
            var finished = memory.Memory.Where(task => task.Status == Task.State.Completed || task.Status == Task.State.Killed);
            foreach (var task in finished)
            {
                memory.UnloadTask(task);
            }
        }

        private void LoadFromQueue()
        {
            if (!memory.HasTasksInQueue)
                return;

            if (memory.FreeSpace < memory.NextTask.Size)
                return;

            var address = memory.FindSuitableAddress(memory.NextTask.Size);
            if (address >= 0)
            {
                memory.LoadTask(address);
                return;
            }

            defragmentator.Start();
        }
        #endregion Tick

        public void AddTask(Task task) => memory.AddTask(task);

        public void MoveTask(int taskIndex, int address)
        {
            if (taskIndex < 0 || taskIndex >= memory.Memory.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(taskIndex));
            }

            var task = memory.Memory[taskIndex];

            if (task.Status == Task.State.Running)
            {
                throw new ArgumentException("Cannot move running task");
            }

            task.Move(address);
            onTaskMoved.OnNext(task);
        }

        public void SuspendTask(ITask task) => SetTaskStatus(task, Task.State.Idle);
        public void ResumeTask(ITask task) => SetTaskStatus(task, Task.State.Running);
        public void KillTask(ITask task) => SetTaskStatus(task, Task.State.Killed);

        public void SetTaskStatus(ITask itask, Task.State status)
        {
            var task = memory.Memory.Find(task => task.Id.Equals(itask.Id));
            if (task == null)
            {
                throw new ArgumentException("Task not loaded or does not exists!");
            }

            task.SetStatus(status);
        }
    }
}
