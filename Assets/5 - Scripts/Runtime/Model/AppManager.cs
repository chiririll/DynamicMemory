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

        public AppManager() 
        {
            config = new();

            simulationManager = new(config.simulation);
            memory = new(config.memory);

            simulationManager.OnSimulationTick.Subscribe(_ => memory.Tick()).AddTo(disp);
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
