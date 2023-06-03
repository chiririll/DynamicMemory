using UniRx;

namespace DynamicMem.Model
{
    public class Task : ITask
    {
        private IntReactiveProperty lifetime;
        private ReactiveProperty<State> status;

        public Task(int size, int maxLifetime, string label = null)
        {
            Id = new();

            status = new(State.Idle);
            Size = size;
            Address = -1;

            Label = string.IsNullOrEmpty(label) ? $"Task #{Id}" : label;
            lifetime = new(0);
            MaxLifetime = maxLifetime;
        }

        public TaskId Id { get; }

        public string Label { get; private set; }
        public int MaxLifetime { get; private set; }

        public IReadOnlyReactiveProperty<int> Lifetime => lifetime;
        public IReadOnlyReactiveProperty<State> Status => status;

        public int Size { get; private set; }
        public int Address { get; private set; }

        public void Tick()
        {
            lifetime.Value++;

            if (lifetime.Value >= MaxLifetime)
            {
                SetStatus(State.Completed);
            }
        }

        public void SetStatus(State status)
        {
            this.status.Value = status;
        }

        public void Load(int address)
        {
            Address = address;
            status.Value = State.Running;
        }

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
