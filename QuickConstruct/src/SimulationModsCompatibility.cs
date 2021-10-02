using System.Linq;
using System.Reflection;
using QuickLibrary;
using UnityEngine;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class SimulationModsCompatibility : MonoBehaviour
    {
        
        internal static SimulationModsCompatibility Instance;

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
            Debug.Log($"[QuickConstruct]({name}): Start");
        }

        public bool IsInSimulation()
        {
            var simulation = Simulation.Instance; 
            var inSimulation = simulation != null && simulation.IsInSimulation();

            Debug.Log($"[QuickConstruct]({name}): Simulation detected ? {inSimulation}");
            return inSimulation;
        }

        private void OnDestroy()
        {
            Debug.Log($"[QuickConstruct]({name}): Destroy");
        }
    }
}