using System;

namespace DynamicMem.NewModel
{
    public class TaskId : IComparable<TaskId>, IEquatable<TaskId>
    {
        public readonly int Id;

        public TaskId()
        {
            Id = TaskIdUtils.Generate();
        }

        public override string ToString() => Id.ToString();

        public int CompareTo(TaskId other) => Id.CompareTo(other.Id);
        public bool Equals(TaskId other) => Id.Equals(other.Id);

        public static implicit operator int(TaskId id) => id.Id;
    }

    public static class TaskIdUtils
    {
        private static int freeId = 0;
        public static int Generate() => freeId++;
    }
}