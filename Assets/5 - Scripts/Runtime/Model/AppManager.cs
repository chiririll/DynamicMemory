using DynamicMem.Config;
using DynamicMem.Model;
using System;
using UniRx;

namespace DynamicMem
{
    public class AppManager : IDisposable
    {
        private readonly CompositeDisposable disp = new();

        private AppConfig config;

        private SimulationManager simulationManager;
        private Memory memory;

        public AppManager(AppConfig config) 
        {
            simulationManager = new(config.simulation);
            memory = new(config.memory);

            simulationManager.OnSimulationTick.Subscribe(_ => memory.Tick()).AddTo(disp);

            DI.Add(this);
            this.LogMsg("Initialized");
        }

        public Memory Memory => memory;

        public void Tick(float deltaTime)
        {
            simulationManager.Tick(deltaTime);
        }

        public void Dispose()
        {
            disp.Dispose();
        }
    }
}
