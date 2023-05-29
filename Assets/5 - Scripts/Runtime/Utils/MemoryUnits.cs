namespace DynamicMem
{
    public static class MemoryUnits
    {
        public static readonly string[] Units = { "B", "KB", "MB", "GB" };
        public const int MemoryMax = 1 << 10;

        public static string ToMemoryString(this int memory)
        {
            if (memory < 0)
                memory = 0;

            int currentUnit = 0;
            while (memory >= MemoryMax)
            {
                currentUnit++;
                memory /= MemoryMax;
            }

            return memory.ToString() + " " + Units[currentUnit];
        }
    }
}
