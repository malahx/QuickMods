using System.Reflection;

namespace QuickIronMan
{
    public class SimConfig
    {

        [KSPField(isPersistant = true)] internal static readonly SimConfig INSTANCE = new SimConfig();
        
        private SimConfig()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                      Assembly.GetExecutingAssembly().GetName().Version.Minor +
                      Assembly.GetExecutingAssembly().GetName().Version.Build;
        }

        public string Version { get; }
        public bool InSimulation = false;
    }
}