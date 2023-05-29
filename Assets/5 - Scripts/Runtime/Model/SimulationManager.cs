using DynamicMem.Config;
using System;
using UniRx;

namespace DynamicMem.Model
{
    public class SimulationManager
    {
        private readonly SimulationConfig config;

        private readonly Subject<float> onSimulationTick = new();
        private readonly Subject<bool> onSimulationStateChanged = new();

        private bool isRunning;
        private float passedTime;

        public SimulationManager(SimulationConfig config) 
        {
            this.config = config;

            DI.Add(this);
            this.LogMsg("Initialized");
        }

        public IObservable<float> OnSimulationTick => onSimulationTick;
        public IObservable<bool> OnSimulationStateChanged => onSimulationStateChanged;

        public bool IsRunning => isRunning;

        public void Tick(float deltaTime)
        {
            if (!isRunning)
            {
                return;
            }

            passedTime += deltaTime;
            
            if (passedTime < config.TickTime)
            {
                return;
            }

            passedTime -= config.TickTime;
            onSimulationTick.OnNext(config.TickTime);
        }

        public void ForceTick()
        {
            passedTime = 0;
            onSimulationTick.OnNext(config.TickTime);
        }

        public void Pause()
        {
            isRunning = false;
            this.LogMsg("Paused simulation");
            onSimulationStateChanged.OnNext(isRunning);
        }

        public void Resume()
        {
            isRunning = true;
            this.LogMsg("Resumed simulation");
            onSimulationStateChanged.OnNext(isRunning);
        }
    }
}
