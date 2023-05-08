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
        [SerializeField] private TMP_InputField lifetime;
        [SerializeField] private TMP_InputField size;
        [SerializeField] private Button submit;

        private LazyInject<MemoryManager> memory = new();

        public void Init()
        {
            submit.OnClickAsObservable().Subscribe(_ => AddTask()).AddTo(this);
        }

        private void AddTask()
        {
            // TODO: Check values and highlight error
            var task = new Task(
                System.Convert.ToInt32(size.text), 
                System.Convert.ToInt32(lifetime.text),
                label.text);
            
            memory.Value.AddTask(task);
        }
    }
}