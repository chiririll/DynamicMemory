namespace DynamicMem.Config
{
    public class MemoryConfig
    {
        public MemoryConfig() 
        {
            Size = 1 << 15;
            OsAllocated = 1 << 12;
        }

        public int Size { get; set; }
        public int OsAllocated { get; set; }
    }
}
