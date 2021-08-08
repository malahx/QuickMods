using System;
using KSP.Localization;
using KSP.UI.TooltipTypes;
using UnityEngine;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class Editor : MonoBehaviour
    {
        private TooltipController_Text tooltipControllerText;
        private string intialTooltip = "";

        private void Start()
        {
            
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Start...");
            EditorLogic.fetch.launchBtn.onClick.AddListener(PrepareLaunch);
            tooltipControllerText = EditorLogic.fetch.launchBtn.GetComponent<TooltipController_Text>();
            intialTooltip = tooltipControllerText.textString;
            tooltipControllerText.SetText($"{Localizer.Format("quickironman_simulate_tooltip")}\n{Localizer.Format("quickironman_launch_info_tooltip")}");
            SimConfig.INSTANCE.InSimulation = false;
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Started.");
        }

        private static void PrepareLaunch()
        {
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Prepare launch action.");
            
            var isHardLaunch = Input.GetKey(KeyCode.LeftShift);
            SimConfig.INSTANCE.InSimulation = !isHardLaunch;
            
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Launch action: OK");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                var textString = $"{Localizer.Format(intialTooltip)}\n{Localizer.Format("quickironman_launch_info_tooltip")}";
                tooltipControllerText.SetText(textString);
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                var textString = $"{Localizer.Format("quickironman_simulate_tooltip")}\n{Localizer.Format("quickironman_launch_info_tooltip")}";
                tooltipControllerText.SetText(textString);
            }
        }

        private void OnDestroy()
        {
            EditorLogic.fetch.launchBtn.onClick.RemoveListener(PrepareLaunch);
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Destroyed.");
        }
    }
}