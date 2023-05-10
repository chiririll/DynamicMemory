using System;
using UniRx;

namespace DynamicMem.Config
{
    public class MemoryConfig
    {
        private Subject<MemoryConfig> onChanged = new();

        private int size;
        private int osAllocated;

        public MemoryConfig() 
        {
            size = 1 << 15;
            osAllocated = 1 << 12;
        }

        public IObservable<MemoryConfig> OnChanged => onChanged;

        public int Size 
        {
            get => size;
            set
            {
                size = value;
                onChanged.OnNext(this);
            }
        }

        public int OsAllocated
        {
            get => osAllocated;
            set
            {
                osAllocated = value;
                onChanged.OnNext(this);
            }
        }
    }
}
