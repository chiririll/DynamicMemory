namespace DynamicMem.NewModel
{
    public class Task
    {
        public State Status { get; private set; }
        public int Size { get; private set; }
        public int Address { get; private set; }


        public void Tick()
        {

        }

        public void SetAddress(int address)
        {
            Address = address;
        }

        public void SetStatus(State status)
        {
            Status = status;
        }

        public enum State
        {
            Running,
            Idle,
            Completed,
            Killed
        }
    }
}
