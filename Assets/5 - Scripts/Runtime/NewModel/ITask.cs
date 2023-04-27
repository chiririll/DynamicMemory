using System;

namespace DynamicMem.NewModel
{
    public interface ITask
    {
        public TaskId Id { get; }

        public Task.State Status { get; }
        public int Size { get; }
        public int Address { get; }

        public void Suspend();
        public void Resume();
        public void Kill();

        public IObservable<Task.State> OnStatusChanged { get; }
    }
}