using DynamicMem.Model;
using TMPro;
using UniRx;
using UnityEngine;

namespace DynamicMem
{
    public class SimulationStateController : MonoBehaviour
    {
        [SerializeField] private string runningText;
        [SerializeField] private string stoppedText;
        [SerializeField] private string defragmentatingText;
        [Space]
        [SerializeField] private TMP_Text simulationState;
        [Space(5)]
        [SerializeField] private TMP_Text tasksInQueueCount;
        [SerializeField] private TMP_Text tasksLoadedCount;
        [Space(5)]
        [SerializeField] private TMP_Text freeSpace;
        [SerializeField] private TMP_Text usedSpace;
        [Space(5)]
        [SerializeField] private TMP_Text fragmentation;

        private SimulationManager simulationManager;
        private MemoryManager memory;

        public void Init()
        {
            simulationManager = DI.Get<SimulationManager>();
            memory = DI.Get<MemoryManager>();

            simulationManager.OnSimulationStateChanged.Subscribe(_ => UpdateState()).AddTo(this);
            simulationManager.OnSimulationTick.Subscribe(_ => UpdateData()).AddTo(this);
            memory.OnCleanupRequested.Subscribe(_ => UpdateData()).AddTo(this);
            memory.OnDefragmentationStarted.Subscribe(_ => UpdateState()).AddTo(this);
            memory.OnDefragmentationEnded.Subscribe(_ => UpdateState()).AddTo(this);

            UpdateState();
            UpdateData();
        }

        private void UpdateData()
        {
            tasksInQueueCount.text = memory.TasksInQueue.ToString();
            tasksLoadedCount.text = memory.TasksInMemory.ToString();

            freeSpace.text = memory.FreeSpace.ToMemoryString();
            usedSpace.text = (memory.Size - memory.FreeSpace).ToMemoryString();

            fragmentation.text = $"{Mathf.Round(memory.Fragmentation * 100)}%";
        }

        private void UpdateState()
        {
            if (simulationManager.IsRunning)
            {
                if (memory.IsDefragmentating)
                {
                    simulationState.text = defragmentatingText;
                    return;
                }

                simulationState.text = runningText;
                return;
            }

            simulationState.text = stoppedText;
        }
    }
}