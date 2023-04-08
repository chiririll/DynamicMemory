namespace DynamicMem.Model
{
    public class Task
    {
        public Task(int lifetime, int memory)
        {
            Lifetime = lifetime;
            Memory = memory;

            CurrentState = State.Idle;
        }

        public int Lifetime { get; private set; }
        public int Memory { get; private set; }

        public State CurrentState { get; private set; }

        public void Tick(int time = 1)
        {
            if (Lifetime <= 0 && CurrentState != State.Finished)
            {
                CurrentState = State.Finished;
            }

            if (CurrentState != State.Idle) return;
            
            Lifetime -= time;
        }
        
        public void Freeze()
        {
            CurrentState = State.Idle;
        }

        public void Start()
        {
            if (CurrentState == State.Finished)
            {
                throw new System.Exception("Can't start finished task");
            }

            CurrentState = State.Running;
        }

        public void Stop()
        {
            if (CurrentState == State.Finished) return;

            CurrentState = State.Finished;
        }

        public enum State
        {
            Idle,
            Running,
            Finished
        }
    }
}
