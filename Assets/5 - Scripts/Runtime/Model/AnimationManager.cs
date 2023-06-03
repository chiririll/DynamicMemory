using DynamicMem.Config;
using UniRx;
using UnityEngine;

namespace DynamicMem.Model
{
    public class AnimationManager
    {
        private const float animationsThreshold = .125f;

        private readonly SimulationConfig simulationConfig;
        private readonly CompositeDisposable disp = new();

        private bool animationsEnabled;

        public bool Enabled
        {
            get => animationsEnabled;
            set => animationsEnabled = simulationConfig.TickTime >= animationsThreshold && value;
        }

        public float ShiftColorTime { get; private set; }
        public float ProgressTime { get; private set; }
        public float TaskResizeTime { get; private set; }

        public float UnloadTaskTime { get; private set; }
        public float UnloadTaskDelay { get; private set; }

        public float TaskSlideYTime { get; private set; }
        public float TaskSlideXTime { get; private set; }

        public AnimationManager(SimulationConfig config)
        {
            this.simulationConfig = config;
            simulationConfig.OnSpeedChanged.Subscribe(_ => UpdateSpeed()).AddTo(disp);

            UpdateSpeed();

            DI.Add(this);
            this.LogMsg("Initialized");
        }

        ~AnimationManager()
        {
            disp.Dispose();
        }

        private void UpdateSpeed()
        {
            Enabled = true;
            if (!Enabled)
            {
                return;
            }

            var halfTick = simulationConfig.TickTime / 2f;

            ShiftColorTime = Mathf.Min(simulationConfig.TickTime, 0.2f);
            ProgressTime = Mathf.Min(simulationConfig.TickTime, 0.2f);

            UnloadTaskTime = halfTick;
            UnloadTaskDelay = halfTick;

            TaskSlideYTime = simulationConfig.TickTime / 4f;
            TaskSlideXTime = halfTick;

            TaskResizeTime = halfTick;
        }
    }
}
