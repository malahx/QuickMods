using System.Linq;
using System.Reflection;
using UnityEngine;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class SimulationModsCompatibility : MonoBehaviour
    {
        
        internal static SimulationModsCompatibility Instance;
        private FieldInfo inSimulation;
        private object simConfig;
        private Assembly quickIronMan;

        private void Start()
        {
            var loadedQuickIronMan = AssemblyLoader.loadedAssemblies.FirstOrDefault(p => p.name == "QuickIronMan");

            if (loadedQuickIronMan == null)
            {
                Debug.Log($"[QuickConstruct]({name}): QuickIronMan not loaded");
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(this);

            Instance = this;

            quickIronMan = loadedQuickIronMan.assembly;

            Debug.Log($"[QuickConstruct]({name}): Start");
        }

        public bool IsInSimulation()
        {

            if (simConfig == null || inSimulation == null)
            {
                InitializeSimulationData();
            }

            if (simConfig == null || inSimulation == null)
            {
                Debug.Log($"[QuickConstruct]({name}): no simulation found ?");
                return false;   
            }

            var value = (bool) inSimulation.GetValue(simConfig);
            
            Debug.Log($"[QuickConstruct]({name}): Simulation detected ? {value}");
            return value;
        }

        private void InitializeSimulationData()
        {
            var type = quickIronMan.GetType("QuickIronMan.SimConfig");
            var instance = type.GetField("INSTANCE", BindingFlags.Public | BindingFlags.Static);
            
            if (instance == null)
            {
                Debug.Log($"[QuickConstruct]({name}): no QuickIronMan configuration");
                return;
            }

            simConfig = instance.GetValue(null);
            inSimulation = type.GetField("InSimulation");
        }

        private void OnDestroy()
        {
            Debug.Log($"[QuickConstruct]({name}): Destroy");
        }
    }
}