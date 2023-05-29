using System;
using UniRx;

namespace DynamicMem.Config
{
    public class MemoryConfig
    {
        private Subject<MemoryConfig> onSizeChanged = new();

        private int size;
        private int osAllocated;

        public MemoryConfig() 
        {
            size = 1 << 15;
            osAllocated = 1 << 12;
        }

        public IObservable<MemoryConfig> OnSizeChanged => onSizeChanged;

        public int Size 
        {
            get => size;
            set
            {
                size = value;
                onSizeChanged.OnNext(this);
            }
        }

        public int OsAllocated
        {
            get => osAllocated;
            set
            {
                osAllocated = value;
                onSizeChanged.OnNext(this);
            }
        }
    }
}
