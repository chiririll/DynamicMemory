using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class SimulationSettingsController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField simulaitonSpeed;
        [SerializeField] private Button submitSimulationSpeed;
        [Space]
        [SerializeField] private Button playButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button stopButton;

        public void Init()
        {
            // TODO: default text
            simulaitonSpeed.text = "1";

            submitSimulationSpeed.OnClickAsObservable().Subscribe().AddTo(this);

            playButton.OnClickAsObservable().Subscribe(/* TODO */).AddTo(this);
            pauseButton.OnClickAsObservable().Subscribe(/* TODO */).AddTo(this);
            nextButton.OnClickAsObservable().Subscribe(/* TODO */).AddTo(this);
            stopButton.OnClickAsObservable().Subscribe(/* TODO */).AddTo(this);
        }
    }
}