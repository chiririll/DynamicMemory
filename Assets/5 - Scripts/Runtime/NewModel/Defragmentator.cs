namespace DynamicMem.NewModel
{
    public class Defragmentator
    {
        private MemoryInfo memory;
        
        private int lastAddr;
        private int currentIndex;

        public Defragmentator(MemoryInfo memory)
        {
            this.memory = memory;

            lastAddr = 0;
            currentIndex = 0;
        }

        public bool Running { get; private set; }

        public void Tick()
        {
            if (!Running)
            {
                return;
            }

            if (currentIndex >= memory.Memory.Count)
            {
                Finish();
            }

            memory.MoveTask(currentIndex, lastAddr);
            lastAddr += memory.Memory[currentIndex].Size;
            currentIndex++;
        }

        public void Start()
        {
            Running = true;

            foreach (var task in memory.Memory)
            {
                if (task.Status != Task.State.Running)
                    continue;

                memory.SuspendTask(task);
            }
        }

        private void Finish()
        {
            Running = false;

            lastAddr = 0;
            currentIndex = 0;

            foreach (var task in memory.Memory)
            {
                if (task.Status != Task.State.Idle)
                    continue;

                memory.ResumeTask(task);
            }
        }
    }
}
