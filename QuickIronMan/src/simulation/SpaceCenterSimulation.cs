using UnityEngine;

namespace QuickIronMan.simulation {
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpaceCenterSimulation : MonoBehaviour
    {
        private void Start()
        {
            Simulation.INSTANCE.ResetSimulation();
            
            Debug.Log($"[QuickIronMan]({name}) Ironman save enabled.");
            Destroy(this);
        }

        private void OnDestroy()
        {
            Debug.Log($"[QuickIronMan]({name}) Destroy");
        }
    }
}