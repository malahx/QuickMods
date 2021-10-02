using System.Linq;
using QuickLibrary;
using UnityEngine;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class SimulationModsCompatibility : MonoBehaviour
    {
        
        internal static SimulationModsCompatibility Instance;
        
        internal static EventData<bool> OnSimulation = new EventData<bool>(nameof(OnSimulation));

        private void Start()
        {
            
            // Check if QuickIronMan is loaded
            var loadedQuickIronMan = AssemblyLoader.loadedAssemblies.FirstOrDefault(p => p.name == "QuickIronMan");

            if (loadedQuickIronMan == null)
            {
                Debug.Log($"[QuickConstruct]({name}): QuickIronMan not loaded");
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(this);

            Instance = this;

            // Prepare simulation listeners
            Simulation.OnEnterSimulation.Add(OnEnterSimulation);
            Simulation.OnExitSimulation.Add(OnExitSimulation);
            
            Debug.Log($"[QuickConstruct]({name}): Start");
        }

        public bool IsInSimulation()
        {
            var simulation = Simulation.Instance; 
            var inSimulation = simulation != null && simulation.IsInSimulation();

            Debug.Log($"[QuickConstruct]({name}): Simulation detected ? {inSimulation}");
            return inSimulation;
        }

        private void OnEnterSimulation()
        {
            OnSimulation.Fire(true);
            Debug.Log($"[QuickConstruct]({name}): OnEnterSimulation");
        }
        
        private void OnExitSimulation()
        {
            OnSimulation.Fire(false);
            Debug.Log($"[QuickConstruct]({name}): OnExitSimulation");
        }

        private void OnDestroy()
        {
            Simulation.OnEnterSimulation.Remove(OnEnterSimulation);
            Simulation.OnExitSimulation.Remove(OnExitSimulation);
            Debug.Log($"[QuickConstruct]({name}): Destroy");
        }
    }
}