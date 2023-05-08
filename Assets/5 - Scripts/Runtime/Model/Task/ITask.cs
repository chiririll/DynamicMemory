using UniRx;

namespace DynamicMem.Model
{
    public interface ITask
    {
        public TaskId Id { get; }
        public string Label { get; }

        public int Size { get; }
        public int Address { get; }
        public int MaxLifetime { get; }

        public IReadOnlyReactiveProperty<Task.State> Status { get; }
        public IReadOnlyReactiveProperty<int> Lifetime { get; }

        public void Suspend();
        public void Resume();
        public void Kill();
    }
}
