using DynamicMem.Model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class AddTaskController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField label;
        [SerializeField] private IntUpDown lifetime;
        [SerializeField] private MemoryUnitInputField memorySize;
        [SerializeField] private Button submit;

        private readonly LazyInject<MemoryManager> memory = new();

        public void Init()
        {
            submit.OnClickAsObservable().Subscribe(_ => AddTask()).AddTo(this);
        }

        private void AddTask()
        {
            if (!ParseInputs(out var lifetime, out var size)) 
                return;

            var task = new Task(size, lifetime, label.text);
            memory.Value.AddTask(task);
        }

        private bool ParseInputs(out int lifetime, out int size)
        {
            var result = true;

            lifetime = this.lifetime.Value;
            
            if (!memorySize.TryGetValue(out size, memory.Value.Size))
            {
                result = false;
            }
            
            return result;
        }
    }
}