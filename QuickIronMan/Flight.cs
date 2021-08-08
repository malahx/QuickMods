using KSP.Localization;
using UnityEngine;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Flight : MonoBehaviour
    {
        
        private readonly SimConfig cfg = SimConfig.INSTANCE;
        
        private GUIStyle textStyle;

        private void Awake()
        {
            if (!cfg.InSimulation) 
                Destroy(this);
            
            Debug.Log($"QuickIronMan[{cfg.Version}] Awake");
        }

        private void Start()
        {
            
            Debug.Log($"QuickIronMan[{cfg.Version}] Start...");
            textStyle = new GUIStyle
            {
                stretchWidth = true,
                stretchHeight = true,
                alignment = TextAnchor.UpperCenter,
                fontSize = Screen.height / 20,
                fontStyle = FontStyle.Bold,
                normal = {textColor = Color.grey}
            };

            HighLogic.CurrentGame.Parameters.Flight.CanRestart = cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor = cfg.InSimulation;
            
            HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanQuickSave = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsNear = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsFar = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanEVA = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanBoard = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanAutoSave = !cfg.InSimulation;
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter = !cfg.InSimulation;

            FlightDriver.CanRevertToPostInit = cfg.InSimulation;
            FlightDriver.CanRevertToPrelaunch = cfg.InSimulation;
            
            Debug.Log($"QuickIronMan[{cfg.Version}] Started, simulation: {cfg.InSimulation}");
        }

        private void OnGUI()
        {
            if (!cfg.InSimulation) 
                return;
            
            GUILayout.BeginArea (new Rect (0, Screen.height / 10f, Screen.width - 0, 160), textStyle);
            GUILayout.Label (Localizer.Format("quickironman_simulation_message"), textStyle);
            GUILayout.EndArea ();
        }

        private void OnDestroy()
        {
            Debug.Log($"QuickIronMan[{cfg.Version}] Destroyed.");
        }
    }
}