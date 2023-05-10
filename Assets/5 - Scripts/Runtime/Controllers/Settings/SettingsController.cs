using DynamicMem.Model;
using UnityEngine;

namespace DynamicMem
{
    public class SettingsController : MonoBehaviour
    {
        private SettingsManager manager;

        [SerializeField] private AddTaskController addTask;
        [SerializeField] private SimulationSettingsController simulation;
        [SerializeField] private MemorySettingsController memory;
        [SerializeField] private SimulationStateController simulationState;
        [SerializeField] private AlertController alertController;

        public void SetData(SettingsManager manager)
        {
            this.manager = manager;
            alertController.gameObject.SetActive(true);
            
            addTask.Init();
            simulation.Init();
            memory.Init();
            simulationState.Init();
        }
    }
}
