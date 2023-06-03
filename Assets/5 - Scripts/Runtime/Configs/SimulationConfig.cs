using System;
using UniRx;

namespace DynamicMem.Config
{
    public class SimulationConfig
    {
        private readonly Subject<float> onSpeedChanged = new();

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
                onSpeedChanged.OnNext(value);
            } 
        }

        public float TickTime { get; private set; }

        public IObservable<float> OnSpeedChanged => onSpeedChanged;
    }
}