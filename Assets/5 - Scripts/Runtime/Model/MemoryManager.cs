using System.Collections.Generic;

namespace DynamicMem
{
    public class MemoryManager
    {
        private MemoryState state;

        public MemoryManager() 
        {
            state = new();
        }

        public void Tick()
        {
            // TODO: recheck
            var needSize = state.queue.Peek()?.Memory;

            foreach (var task in state.memory)
            {
                // TODO: Unload completed tasks
                task.Tick();
            }
        }

        private void Add(TaskManager task)
        {
            task.Freeze();
            state.queue.Enqueue(task);
        }
    }
}
