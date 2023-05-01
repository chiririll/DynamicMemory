namespace DynamicMem.Config
{
    public class SimulationConfig
    {
        private float simulationSpeed;
        
        public SimulationConfig()
        {
            SimulationSpeed = 1f;
        }

        public float SimulationSpeed 
        { 
            get => simulationSpeed; 
            set 
            { 
                simulationSpeed = value;
                TickTime = 1 / value;
            } 
        }

        public float TickTime { get; private set; }
    }
}