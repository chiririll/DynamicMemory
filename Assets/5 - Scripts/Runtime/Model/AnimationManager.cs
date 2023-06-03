using DynamicMem.Config;
using UniRx;

namespace DynamicMem.Model
{
    public class AnimationManager
    {
        private const float animationsThreshold = .3f;

        private readonly SimulationConfig simulationConfig;
        private readonly CompositeDisposable disp = new();

        private bool animationsEnabled;

        public bool Enabled 
        {
            get => animationsEnabled; 
            set => animationsEnabled = simulationConfig.TickTime >= animationsThreshold && value;
        }
        
        public float ShiftColorTime { get; private set; }
        public float TaskResizeTime { get; private set; }
        
        public float UnloadTaskTime { get; private set; }
        public float TaskSlideYTime { get; private set; }
        public float TaskSlideXTime { get; private set; }


        public AnimationManager(SimulationConfig config)
        {
            this.simulationConfig = config;
            simulationConfig.OnSpeedChanged.Subscribe(_ => UpdateSpeed()).AddTo(disp);

            ShiftColorTime = 0.2f;
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
            UnloadTaskTime = halfTick;
            TaskSlideYTime = simulationConfig.TickTime / 4f;
            TaskSlideXTime = halfTick;
            TaskResizeTime = halfTick;
        }
    }
}
