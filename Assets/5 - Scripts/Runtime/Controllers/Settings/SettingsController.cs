using UnityEngine;

namespace DynamicMem
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private AddTaskController addTask;
        [SerializeField] private SimulationSettingsController simulation;
        [SerializeField] private MemorySettingsController memory;
        [SerializeField] private SimulationStateController simulationState;
    }
}