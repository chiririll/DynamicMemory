using DynamicMem.Config;
using DynamicMem.Model;

namespace DynamicMem
{
    public class AppManager
    {
        private AppConfig config;

        private Memory memory;

        public AppManager() 
        {
            config = new();

            memory = new(config.memory);
        }

        public Memory Memory => memory;
    }
}