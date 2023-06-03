using DynamicMem.Model;
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

        private readonly LazyInject<MemoryManager> memory = new();

        public void Init()
        {
            submitButton.OnClickAsObservable().Subscribe(_ => GenerateTasks()).AddTo(this);
        }

        public void GenerateTasks()
        {
            if (!minMemory.TryGetValue(out var minMem, memory.Value.Size) || 
                !maxMemory.TryGetValue(out var maxMem, memory.Value.Size))
            {
                return;
            }

            for (var i = 0; i < tasksCount.Value; i++)
            {
                var lifetime = Random.Range(minRunTime.Value, maxRunTime.Value);
                var memSize = Random.Range(minMem, maxMem);
                
                var task = new Task(memSize, lifetime);
                memory.Value.AddTask(task);
            }
        }
    }
}
