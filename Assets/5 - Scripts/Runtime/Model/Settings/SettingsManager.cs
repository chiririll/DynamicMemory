namespace DynamicMem.Model
{
    public class SettingsManager
    {
        public SettingsManager() 
        {

            DI.Add(this);
            this.LogMsg("Initialized");
        }
    }
}
