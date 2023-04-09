using Newtonsoft.Json;
using UniRx;

namespace DynamicMem
{
    [System.Serializable]
    public class TaskInfo
    {
        [JsonProperty("state")]
        private TaskState currentState;
        [JsonIgnore]
        private Subject<TaskState> onStateChanged;

        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("lifetime")]
        public int Lifetime { get; set; }
        [JsonProperty("memory")]
        public int Memory { get; }

        public TaskState CurrentState 
        {
            get => currentState;
            set 
            { 
                if (currentState == value) return;

                currentState = value;
                onStateChanged.OnNext(value);
            }
        }

        public Subject<TaskState> OnStateChanged => onStateChanged;

        public TaskInfo(string name, int memory, int lifetime)
        {
            this.Name = name;
            this.currentState = TaskState.Idle;

            this.Lifetime = lifetime;
            this.Memory = memory;
        }
    }
}
