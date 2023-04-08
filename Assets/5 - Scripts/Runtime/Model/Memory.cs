using System.Collections.Generic;

namespace DynamicMem.Model
{
    public class Memory
    {
        public readonly int size;

        private Queue<Task> queue;
        private LinkedList<Task> memory;

        public Memory(int size, IEnumerable<Task> queue = null) 
        {
            this.size = size;

            this.queue = queue == null ? new() : new(queue);
            memory = new();
        }

        public void Tick()
        {
            // TODO: recheck
            var needSize = queue.Peek()?.Memory;

            foreach (var task in memory)
            {
                // TODO: Unload completed tasks
                task.Tick();
            }
        }



    }
}
