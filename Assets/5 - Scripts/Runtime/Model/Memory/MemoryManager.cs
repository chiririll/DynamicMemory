using DynamicMem.Config;
using System;
using System.Collections.Generic;
using UniRx;

namespace DynamicMem.Model
{
    public class MemoryManager
    {
        private readonly MemoryConfig config;

        private readonly Subject<Task> onTaskMoved = new();
        private readonly Subject<MemoryConfig> onCleanupRequested = new();

        private readonly Subject<ITask> onTaskSelected = new();

        private readonly MemoryInfo memory;
        private Defragmentator defragmentator;

        private CompositeDisposable disp = new();

        public MemoryManager(MemoryConfig config)
        {
            this.config = config;
            config.OnSizeChanged.Subscribe(_ => Clear()).AddTo(disp);

            memory = new(config.Size);
            defragmentator = new(this);

            DI.Add(this);
            this.LogMsg("Initialized");
        }

        public IEnumerable<ITask> Queue => memory.Queue;
        public IReadOnlyList<ITask> LoadedTasks => memory.Memory;
        public int Size => memory.Size;

        public bool HasAnyTasks => memory.TasksInQueue > 0 || memory.LoadedTasksCount > 0;
        public int TasksInQueue => memory.TasksInQueue;
        public int TasksInMemory => memory.LoadedTasksCount;

        public int FreeSpace => memory.FreeSpace;
        public float Fragmentation => memory.CountFragmentation();

        public bool IsDefragmentating => defragmentator.Running;

        public IObservable<ITask> OnTaskEnqueue => memory.OnTaskEnqueue;
        public IObservable<ITask> OnTaskLoaded => memory.OnTaskLoaded;
        public IObservable<ITask> OnTaskMoved => onTaskMoved;
        public IObservable<ITask> OnTaskUnloaded => memory.OnTaskUnloaded;
        public IObservable<MemoryConfig> OnCleanupRequested => onCleanupRequested;

        public IObservable<ITask> OnTaskSelected => onTaskSelected;
        public ITask SelectedTask { get; private set; }

        public void Clear()
        {
            memory.Resize(config.Size);
            defragmentator = new(this);

            onCleanupRequested.OnNext(config);
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
                if (task.Status.Value != Task.State.Running)
                    continue;

                task.Tick();
            }
        }

        private void UnloadFinished()
        {
            for (var i = 0; i < memory.Memory.Count;)
            {
                var task = memory.Memory[i];
                if (task.Status.Value != Task.State.Completed &&
                    task.Status.Value != Task.State.Killed)
                {
                    i++;
                    continue;
                }
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

        public bool MoveTask(int taskIndex, int address)
        {
            if (taskIndex < 0 || taskIndex >= memory.Memory.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(taskIndex));
            }

            var task = memory.Memory[taskIndex];

            if (task.Status.Value == Task.State.Running)
            {
                throw new ArgumentException("Cannot move running task");
            }

            if (task.Address == address) 
            { 
                return false; 
            }

            task.Move(address);
            onTaskMoved.OnNext(task);
            return true;
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

        public void SelectTask(ITask task)
        {
            SelectedTask = task;
            onTaskSelected.OnNext(task);
        }
    }
}
