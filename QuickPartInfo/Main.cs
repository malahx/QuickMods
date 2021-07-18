using UnityEngine;

namespace QuickPartInfo
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class QuickPartInfo : MonoBehaviour
    {
        private PartInfoConfig cfg;

        private PartInfoService partInfoService;

        private void Awake()
        {
            cfg = new PartInfoConfig();
            partInfoService = new PartInfoService(cfg);
        }

        private void OnDestroy()
        {
            Debug.Log($"QuickPartInfo[{cfg.Version}] Update part...");

            foreach (var p in PartLoader.LoadedPartsList) partInfoService.UpdateDescription(p);

            Debug.Log($"QuickPartInfo[{cfg.Version}] Part updated.");
        }
    }
}