namespace DynamicMem.Model
{
    public class Defragmentator
    {
        private MemoryManager memory;
        
        private int lastAddr;
        private int currentIndex;

        public Defragmentator(MemoryManager memory)
        {
            this.memory = memory;

            lastAddr = 0;
            currentIndex = 0;
        }

        public bool Running { get; private set; } = false;

        public void Tick()
        {
            if (!Running)
            {
                return;
            }

            if (currentIndex >= memory.LoadedTasks.Count)
            {
                Finish();
                return;
            }

            var moved = false;
            do
            {
                moved = memory.MoveTask(currentIndex, lastAddr);
                lastAddr += memory.LoadedTasks[currentIndex].Size;
                currentIndex++;
            }
            while (!moved && currentIndex < memory.LoadedTasks.Count);
        }

        public void Start()
        {
            Running = true;

            foreach (var task in memory.LoadedTasks)
            {
                if (task.Status.Value != Task.State.Running)
                    continue;

                memory.SuspendTask(task);
            }
        }

        private void Finish()
        {
            Running = false;

            lastAddr = 0;
            currentIndex = 0;

            foreach (var task in memory.LoadedTasks)
            {
                if (task.Status.Value != Task.State.Idle)
                    continue;

                memory.ResumeTask(task);
            }
        }
    }
}
