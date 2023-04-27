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

        public void Init()
        {
            submit.OnClickAsObservable().Subscribe(_ => UpdateMemorySettings()).AddTo(this);
        }

        private void UpdateMemorySettings()
        {

        }
    }
}