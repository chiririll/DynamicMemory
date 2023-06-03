using DynamicMem.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class GenerateTasksController : MonoBehaviour
    {
        [SerializeField] private IntUpDown tasksCount;
        [Space]
        [SerializeField] private IntUpDown minRunTime;
        [SerializeField] private IntUpDown maxRunTime;
        [Space]
        [SerializeField] private MemoryUnitInputField minMemory;
        [SerializeField] private MemoryUnitInputField maxMemory;
        [Space]
        [SerializeField] private Button submitButton;
        [Header("Endless generation")]
        [SerializeField] private Button endlessGenBtn;
        [SerializeField] private TMP_Text endlessGenText;
        [SerializeField] private string endlessGenEnabled;
        [SerializeField] private string endlessGenDisabled;

        private readonly LazyInject<MemoryManager> memory = new();
        private readonly LazyInject<SimulationManager> simulationManager = new();

        private bool endlessGen = false;

        public void Init()
        {
            submitButton.OnClickAsObservable().Subscribe(_ => GenerateTasks()).AddTo(this);

            endlessGenBtn.OnClickAsObservable().Subscribe(_ => ToggleEndlessGeneration()).AddTo(this);
            simulationManager.Value.OnSimulationTick.Subscribe(_ => Tick());

            endlessGenText.text = endlessGenDisabled;
        }

        private void GenerateTasks()
        {
            if (!TryGetMemoryMinMax(out var minMem, out var maxMem)) return;

            for (var i = 0; i < tasksCount.Value; i++)
            {
                CreateRandomTask(minRunTime.Value, maxRunTime.Value, minMem, maxMem);
            }
        }

        private void ToggleEndlessGeneration()
        {
            endlessGen = !endlessGen;
            endlessGenText.text = endlessGen ? endlessGenEnabled : endlessGenDisabled;
        }

        private void Tick()
        {
            if (!endlessGen) return;
            if (!TryGetMemoryMinMax(out var minMem, out var maxMem)) return;

            var queueSize = 0;
            foreach (var task in memory.Value.Queue)
            {
                queueSize += task.Size;
            }

            while (queueSize < memory.Value.Size)
            {
                queueSize += CreateRandomTask(minRunTime.Value, maxRunTime.Value, minMem, maxMem).Size;
            }
        }

        private bool TryGetMemoryMinMax(out int min, out int max)
        {
            var minValid = minMemory.TryGetValue(out min, memory.Value.Size);
            var maxValid = maxMemory.TryGetValue(out max, memory.Value.Size);
            return minValid && maxValid;
        }

        private Task CreateRandomTask(int minTime, int maxTime, int minMem, int maxMem)
        {
            var lifetime = Random.Range(minTime, maxTime);
            var memSize = Random.Range(minMem, maxMem);

            var task = new Task(memSize, lifetime);
            memory.Value.AddTask(task);
            return task;
        }
    }
}
