using DynamicMem.Config;
using DynamicMem.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class SimulationSettingsController : MonoBehaviour
    {
        [SerializeField] private FloatUpDown simulaitonSpeed;
        [Space]
        [SerializeField] private Button playButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button stopButton;

        private LazyInject<AppConfig> config = new();

        private LazyInject<SimulationManager> simulationManager = new();
        private LazyInject<MemoryManager> memoryManager = new();
        private LazyInject<AlertController> alertController = new();

        public void Init()
        {
            simulaitonSpeed.Init();

            simulaitonSpeed.SetValue(config.Value.simulation.SimulationSpeed, false);
            simulaitonSpeed.OnValueChanged.Subscribe(_ => UpdateSettings()).AddTo(this);

            playButton.OnClickAsObservable().
                Subscribe(_ => simulationManager.Value.Resume()).AddTo(this);
            pauseButton.OnClickAsObservable().
                Subscribe(_ => simulationManager.Value.Pause()).AddTo(this);
            nextButton.OnClickAsObservable().
                Subscribe(_ => simulationManager.Value.ForceTick()).AddTo(this);
            stopButton.OnClickAsObservable()
                .Subscribe(_ => alertController.Value.Show(() => { 
                    simulationManager.Value.Pause(); 
                    memoryManager.Value.Clear();
                })).AddTo(this);
        }

        private void UpdateSettings()
        {
            config.Value.simulation.SimulationSpeed = simulaitonSpeed.Value;
        }
    }
}