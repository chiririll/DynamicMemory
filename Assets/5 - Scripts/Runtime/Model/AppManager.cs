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
        private MemoryManager memory;
        private SettingsManager settingsManager;

        public AppManager(AppConfig config) 
        {
            simulationManager = new(config.simulation);
            memory = new(config.memory);
            settingsManager = new();

            // Костыль
            simulationManager.OnSimulationTick.Subscribe(_ => memory.Tick()).AddTo(disp);

            DI.Add(this);
            this.LogMsg("Initialized");
        }

        public MemoryManager Memory => memory;
        public SettingsManager SettingsManager => settingsManager;

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
