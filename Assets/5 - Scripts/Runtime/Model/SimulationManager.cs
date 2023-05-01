using DynamicMem.Config;
using System;
using UniRx;

namespace DynamicMem.Model
{
    public class SimulationManager
    {
        private readonly SimulationConfig config;

        private readonly Subject<float> onSimulationTick;

        private float passedTime;

        public SimulationManager(SimulationConfig config) 
        {
            this.config = config;
            this.onSimulationTick = new();
        }

        public IObservable<float> OnSimulationTick => onSimulationTick;

        public void Tick(float deltaTime)
        {
            passedTime += deltaTime;
            
            if (passedTime < config.SimulationSpeed)
            {
                return;
            }

            passedTime -= config.SimulationSpeed;
            onSimulationTick.OnNext(config.SimulationSpeed);
        }
    }
}
