using UniRx;
using UnityEngine;

namespace DynamicMem
{
    public class MemoryController : MonoBehaviour
    {
        private MemoryManager manager;
        private CompositeDisposable disp = new();

        [SerializeField] private TaskController taskPrefab;

        public void Init(MemoryManager manager)
        {
            disp.Clear();
            this.manager = manager;

            // TODO: Subscribe
        }

        private void CreateTask()
        {

        }
    }
}
