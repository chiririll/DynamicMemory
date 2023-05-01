using UnityEngine;

namespace DynamicMem
{
    public class AppController : MonoBehaviour
    {
        [SerializeField] private MemoryController memoryController;

        private AppManager manager;

        public void Awake()
        {
            manager = new();
        }

        public void Update()
        {
            manager.Tick(Time.deltaTime);
        }
    }
}
