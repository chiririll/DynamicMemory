using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class MemorySettingsController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField size;
        [SerializeField] private TMP_InputField osAllocated;
        [SerializeField] private Button submit;

        public void Init()
        {
            // TODO
            submit.OnClickAsObservable().Subscribe().AddTo(this);
        }
    }
}