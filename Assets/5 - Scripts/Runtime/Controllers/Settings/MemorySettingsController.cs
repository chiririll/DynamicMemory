using DynamicMem.Config;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class MemorySettingsController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField size;
        //[SerializeField] private TMP_InputField osAllocated;
        [SerializeField] private Button submit;

        private LazyInject<AlertController> alertController = new();
        private LazyInject<AppConfig> config = new();

        public void Init()
        {
            submit.OnClickAsObservable().Subscribe(_ => ChangeSettingsClick()).AddTo(this);
        }

        private void ChangeSettingsClick()
        {
            if (!CheckFields(out var size))
                return;
            alertController.Value.Show(ApplySettings);
        }

        private void ApplySettings()
        {
            if (!CheckFields(out var size))
                return;

            config.Value.memory.Size = size;
        }

        private bool CheckFields(out int size)
        {
            var result = true;

            if (!int.TryParse(this.size.text, out size) || size <= 0)
            {
                result = false;
                this.size.HighlightUntilClick();
            }

            return result;
        }
    }
}
