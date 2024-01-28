using QuickIronMan.simulation;
using UnityEngine;

namespace QuickIronMan.construction {
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class EditorConstruct : MonoBehaviour
    {
        private void Awake()
        {
            Simulation.INSTANCE.LockSimulation(true);

            Debug.Log($"[QuickIronMan]({name}) Awake");
            Destroy(this);
        }

        private void OnDestroy()
        {
            Debug.Log($"[QuickIronMan]({name}) Destroy");
        }
    }
}
