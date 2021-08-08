using UnityEngine;

namespace QuickModsInfo
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class QuickModsInfo : MonoBehaviour
    {
        private ModsInfoConfig cfg;

        private ModsInfoService modsInfoService;

        private void Awake()
        {
            cfg = new ModsInfoConfig();
            modsInfoService = new ModsInfoService(cfg);
        }

        private void OnDestroy()
        {
            Debug.Log($"QuickModsInfo[{cfg.Version}] Update part...");

            foreach (var p in PartLoader.LoadedPartsList) modsInfoService.UpdateDescription(p);

            Debug.Log($"QuickModsInfo[{cfg.Version}] Part updated.");
        }
    }
}