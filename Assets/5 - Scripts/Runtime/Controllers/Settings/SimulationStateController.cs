using TMPro;
using UnityEngine;

namespace DynamicMem
{
    public class SimulationStateController : MonoBehaviour
    {
        [SerializeField] private TMP_Text tasksCount;
        [SerializeField] private TMP_Text tasksLoaded;
        [SerializeField] private TMP_Text usedSpace;
        [SerializeField] private TMP_Text freeSpace;
        [SerializeField] private TMP_Text fragmentation;

        public void Init()
        {

        }
    }
}