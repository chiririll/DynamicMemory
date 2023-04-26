using System.Collections.Generic;

namespace DynamicMem.NewModel
{
    public class Defragmentator
    {
        private IEnumerable<Task> memory;

        public Defragmentator(IEnumerable<Task> memory)
        {
            this.memory = memory;
        }

        public void Tick()
        {
            foreach (var task in memory)
            {
                task.SetStatus(Task.State.Idle);
            }
        }
    }
}
