using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace DynamicMem.NewModel
{
    public class MemoryInfo
    {
        private int size;

        private readonly Queue<Task> queue;
        private readonly List<Task> memory;

        private bool memoryChanged;
        private int freeSpace;

        private Subject<Task> onTaskEnqueue = new();
        private Subject<Task> onTaskLoaded = new();
        private Subject<Task> onTaskMoved = new();
        private Subject<Task> onTaskUnloaded = new();

        public MemoryInfo(int size) 
        {
            this.size = size;
            this.freeSpace = size;

            queue = new();
            memory = new();
        }

        public int Size => size;

        public IEnumerable<ITask> Queue => queue;
        public IReadOnlyList<ITask> Memory => memory;

        public IObservable<Task> OnTaskEnqueue => onTaskEnqueue;
        public IObservable<Task> OnTaskLoaded => onTaskLoaded;
        public IObservable<Task> OnTaskMoved => onTaskMoved;
        public IObservable<Task> OnTaskUnloaded => onTaskUnloaded;

        public int FreeSpace { 
            get 
            {
                if (memoryChanged)
                {
                    CalculateFreeSpace();
                }
                return freeSpace; 
            } 
        }

        public bool HasTasksInQueue => queue.Count > 0;
        public Task NextTask => queue.Peek();

        public void AddTask(Task task)
        {
            task.SetStatus(Task.State.Idle);

            queue.Enqueue(task);
            onTaskEnqueue.OnNext(task);
        }

        public void LoadTask(int address)
        {
            var task = queue.Dequeue();

            task.Load(address);
            memory.Add(task);

            memory.Sort((x, y) => x.Address.CompareTo(y.Address));
            memoryChanged = true;
            
            onTaskLoaded.OnNext(task);
        }

        public void UnloadTask(ITask itask)
        {
            var task = memory.Find(task => task.Id.Equals(itask.Id));

            task.Unload();
            memory.Remove(task);
            memoryChanged = true;

            onTaskUnloaded.OnNext(task);
        }

        public void MoveTask(int taskIndex, int address)
        {
            if (taskIndex < 0 || taskIndex >= memory.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(taskIndex));
            }

            var task = memory[taskIndex];

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
            var task = memory.Find(task => task.Id.Equals(itask.Id));
            if (task == null) 
            {
                throw new ArgumentException("Task not loaded or does not exists!");
            }

            task.SetStatus(status);
        }

        public int FindSuitableAddress(int size)
        {
            var addr = 0;

            foreach (var task in memory)
            {
                if (task.Address - addr >= size)
                    return addr;

                addr = task.Address + task.Size;
            }
            
            if (this.size - addr >= size)
            {
                return addr;
            }

            return -1;
        }

        private void CalculateFreeSpace()
        {
            var addr = 0;
            var freeSpace = 0;

            foreach (var task in memory)
            {
                freeSpace += task.Address - addr;
                addr = task.Address + task.Size;
            }
            
            this.freeSpace = freeSpace;
            memoryChanged = false;
        }
    }
}
