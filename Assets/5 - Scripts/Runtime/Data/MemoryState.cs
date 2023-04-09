using System.Collections.Generic;

namespace DynamicMem
{
    public class MemoryState
    {
        public readonly int size;

        public readonly Queue<TaskManager> queue = new();
        public readonly LinkedList<TaskManager> memory = new();

        public MemoryState()
        {
        }

        public MemoryState(IEnumerable<TaskInfo> queue, IEnumerable<TaskInfo> memory)
        {
            
        }
    }
}
