using DynamicMem.Config;
using UnityEngine;

namespace DynamicMem
{
    public class AppController : MonoBehaviour
    {
        [SerializeField] private MemoryController memoryController;
        [SerializeField] private SettingsController settingsController;

        private AppManager manager;
        private AppConfig config;

        public void Awake()
        {
            Init();
        }

        public void Start()
        {
            memoryController.SetData(manager.Memory);
            settingsController.SetData(manager.SettingsManager);
        }

        public void Update()
        {
            manager.Tick(Time.deltaTime);
        }

        private void Init()
        {
            config = new();
            DI.Add(config);
            
            manager = new(config);
        }
    }
}
