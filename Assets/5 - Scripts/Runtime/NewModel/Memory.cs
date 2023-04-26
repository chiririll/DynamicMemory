using System;
using System.Collections.Generic;
using UniRx;

namespace DynamicMem.NewModel
{
    public class Memory
    {
        private Queue<Task> queue;
        private List<Task> memory;

        private State state;

        private Subject<Task> onTaskCompleted = new();

        public Memory() 
        {
            queue = new();
            memory = new();
        }

        public IObservable<Task> OnTaskCompleted => onTaskCompleted;

        public void Tick()
        {
            var toRemove = new List<Task>();
            foreach (var task in memory)
            {
                if (task.Status == Task.State.Running)
                {
                    task.Tick();
                    continue;
                }
                if (task.Status == Task.State.Completed || task.Status == Task.State.Killed)
                {
                    toRemove.Add(task);
                }
            }

            for (var i = toRemove.Count - 1; i >= 0; i--)
            {
                onTaskCompleted.OnNext(toRemove[i]);
                toRemove.RemoveAt(i);
            }
        }

        public void Add(Task item)
        {
            queue.Enqueue(item);
        }

        private void ChekcMemory()
        {
            foreach (var task in memory)
            {

            }    
        }

        private void CheckTask(Task task)
        {

        }

        private void AddTaskInMemory()
        {
            
            memory.Sort((x, y) => x.Address.CompareTo(y.Address));
        }

        private int CalculateFreeSpace()
        {
            var addr = 0;
            var freeSpace = 0;
            foreach (var task in memory)
            {
                freeSpace += task.Address - addr;
                addr = task.Address + task.Size;
            }
            return freeSpace;
        }

        private void Defragmentate()
        {
            foreach(var task in memory)
            {
                if (task.Status != Task.State.Running)
                    continue;
                 
                task.SetStatus(Task.State.Idle);
            }

            var addr = 0;
            foreach(var task in memory)
            {
                task.SetAddress(0);
                addr = task.Address + task.Size;
                // TODO: Delay
            }

            foreach (var task in memory)
            {
                task.SetStatus(Task.State.Running);
            }
        }

        public enum State
        {
            Idle,
            Running,
            Defragmentating
        }
    }
}
