using UniRx;

namespace DynamicMem
{
    public class TaskManager
    {
        private TaskInfo task;

        public Subject<TaskState> OnStateChanged => task.OnStateChanged;

        public string Name => task.Name;
        public int Memory => task.Memory;

        public TaskManager(TaskInfo task)
        {
            this.task = task;
        }

        public TaskManager(string name, int lifetime, int memory)
        {
            task = new(name, memory, lifetime);
        }

        public void Tick(int time = 1)
        {
            if (task.Lifetime <= 0 && task.CurrentState != TaskState.Completed)
            {
                task.CurrentState = TaskState.Completed;
            }

            if (task.CurrentState != TaskState.Running) return;
            
            task.Lifetime -= time;
        }
        
        public void Freeze()
        {
            if (task.CurrentState == TaskState.Completed) return;

            task.CurrentState = TaskState.Idle;
        }

        public void Start()
        {
            if (task.CurrentState == TaskState.Completed)
            {
                throw new System.Exception("Can't start completed task");
            }

            task.CurrentState = TaskState.Running;
        }

        public void Stop()
        {
            if (task.CurrentState == TaskState.Completed) return;

            task.CurrentState = TaskState.Completed;
        }
    }
}
