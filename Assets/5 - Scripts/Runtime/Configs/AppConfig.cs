namespace DynamicMem.Config
{
    public class AppConfig
    {
        public readonly MemoryConfig memory;
        public readonly SimulationConfig simulation;

        public AppConfig()
        {
            memory = new();
            simulation = new();
        }
    }
}
