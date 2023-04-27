using DynamicMem.Model;
using UniRx;
using UnityEngine;

namespace DynamicMem
{
    public class MemoryController : MonoBehaviour
    {
        private Memory memory;
        private CompositeDisposable disp = new();

        [SerializeField] private TaskController taskPrefab;

        public void Init(Memory memory)
        {
            disp.Clear();
            this.memory = memory;

            // TODO: Subscribe
        }

        private void CreateTask()
        {

        }
    }
}
