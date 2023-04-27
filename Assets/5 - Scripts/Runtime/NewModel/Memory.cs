using System;
using System.Linq;
using UniRx;

namespace DynamicMem.NewModel
{
    public class Memory
    {
        private MemoryInfo memory;

        private Defragmentator defragmentator;
        
        private Subject<Task> onTaskCompleted = new();

        public Memory(int size)
        {
            memory = new(size);
            defragmentator = new(memory);
        }

        public IObservable<Task> OnTaskCompleted => onTaskCompleted;

        public IObservable<Task> OnTaskEnqueue => memory.OnTaskEnqueue;
        public IObservable<Task> OnTaskLoaded => memory.OnTaskLoaded;
        public IObservable<Task> OnTaskMoved => memory.OnTaskMoved;
        public IObservable<Task> OnTaskUnloaded => memory.OnTaskUnloaded;

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
        }

        private void TickTasks()
        {
            foreach (var task in memory.Memory)
            {
                if (task.Status != Task.State.Running)
                    continue;

                //task.Tick();
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
            if (address > 0)
            {
                memory.LoadTask(address);
                return;
            }

            defragmentator.Start();
        }
    }
}
