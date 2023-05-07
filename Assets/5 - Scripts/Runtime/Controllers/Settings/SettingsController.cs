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

        public void SetData(SettingsManager manager)
        {
            this.manager = manager;

            addTask.Init();
            simulation.Init();
            memory.Init();
            simulationState.Init();
        }
    }
}
