using UnityEngine;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpaceCenter : MonoBehaviour
    {
        private void Start()
        {
            SimConfig.INSTANCE.ResetSimulation();
            
            Debug.Log($"[QuickIronMan]({name}) Ironman save enabled.");
            Destroy(this);
        }

        private void OnDestroy()
        {
            Debug.Log($"[QuickIronMan]({name}) Destroy");
        }
    }
}