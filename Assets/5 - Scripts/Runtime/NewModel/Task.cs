using System;
using UniRx;

namespace DynamicMem.NewModel
{
    public class Task : ITask
    {
        private Subject<State> onStatusChanged;

        public Task(int size, int maxLifetime, string label = null)
        {
            Id = new();

            Status = State.Idle;
            Size = size;
            Address = -1;
            
            Label = string.IsNullOrEmpty(label) ? $"Task {Id}" : label;
            Lifetime = 0;
            MaxLifetime = maxLifetime;
        }

        public TaskId Id { get; }

        public string Label { get; private set; }
        public int Lifetime { get; private set; }
        public int MaxLifetime { get; private set; }

        public State Status { get; private set; }
        public int Size { get; private set; }
        public int Address { get; private set; }

        public IObservable<State> OnStatusChanged => onStatusChanged;

        public void Tick()
        {
            Lifetime++;

            if (Lifetime >= MaxLifetime)
            {
                SetStatus(State.Completed);
            }
        }

        public void SetStatus(State status)
        {
            Status = status;
        }

        public void Load(int address) => Address = address;
        public void Unload() => Address = -1;
        public void Move(int address)
        {
            Address = address;
        }

        public void Suspend() => SetStatus(State.Idle);
        public void Resume() => SetStatus(State.Idle);
        public void Kill() => SetStatus(State.Killed);

        public enum State
        {
            Running,
            Idle,
            Completed,
            Killed
        }
    }
}
