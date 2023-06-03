using DynamicMem.Config;
using DynamicMem.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class SimulationSettingsController : MonoBehaviour
    {
        [SerializeField] private FloatUpDown simulaitonSpeed;
        [SerializeField] private Toggle useAnimations;
        [SerializeField] private Toggle pauseOnDefragmentation;
        [Space]
        [SerializeField] private Button playButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button stopButton;

        private readonly LazyInject<AppConfig> config = new();

        private readonly LazyInject<SimulationManager> simulationManager = new();
        private readonly LazyInject<AnimationManager> animManager = new();
        private readonly LazyInject<MemoryManager> memoryManager = new();
        private readonly LazyInject<AlertController> alertController = new();

        public void Init()
        {
            simulaitonSpeed.SetValue(config.Value.simulation.SimulationSpeed, false);
            simulaitonSpeed.OnValueChanged.Subscribe(_ => UpdateSettings()).AddTo(this);
            useAnimations.onValueChanged.AsObservable().Subscribe(_ => ToggleAnimations()).AddTo(this);
            memoryManager.Value.OnDefragmentationStarted.Subscribe(_ => PauseOnDefragmentation()).AddTo(this);

            playButton.OnClickAsObservable().
                Subscribe(_ => simulationManager.Value.Resume()).AddTo(this);
            pauseButton.OnClickAsObservable().
                Subscribe(_ => simulationManager.Value.Pause()).AddTo(this);
            nextButton.OnClickAsObservable().
                Subscribe(_ => simulationManager.Value.ForceTick()).AddTo(this);
            stopButton.OnClickAsObservable()
                .Subscribe(_ => alertController.Value.Show(() =>
                {
                    simulationManager.Value.Pause();
                    memoryManager.Value.Clear();
                })).AddTo(this);
        }

        private void UpdateSettings()
        {
            config.Value.simulation.SimulationSpeed = simulaitonSpeed.Value;
        }

        private void ToggleAnimations()
        {
            animManager.Value.Enabled = useAnimations.isOn;
        }

        private void PauseOnDefragmentation()
        {
            if (!pauseOnDefragmentation.isOn) return;

            simulationManager.Value.Pause();
        }
    }
}