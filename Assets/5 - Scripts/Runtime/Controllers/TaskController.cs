using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicMem
{
    public class TaskController : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text memoryUsage;
        [SerializeField] private Slider progress;

        public void Init()
        {
            progress.gameObject.SetActive(false);
        }
    }
}
