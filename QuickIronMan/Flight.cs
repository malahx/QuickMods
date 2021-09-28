using KSP.Localization;
using KSP.UI.Screens;
using UnityEngine;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Flight : MonoBehaviour
    {
        
        private readonly SimConfig cfg = SimConfig.INSTANCE;
        private AltimeterSliderButtons altimeterSliderButtons = null;
        
        private GUIStyle textStyle;
        private void Start()
        {
            
            Debug.Log($"QuickIronMan[{cfg.Version}] Start...");
            
            textStyle = CreateSimulationText();
            RefreshSimulationVariables();

            altimeterSliderButtons = (AltimeterSliderButtons)FindObjectOfType(typeof(AltimeterSliderButtons));
            
            Debug.Log($"QuickIronMan[{cfg.Version}] Started, simulation: {cfg.InSimulation}");
            
            if (!cfg.InSimulation) 
                Destroy(this);
        }

        private static GUIStyle CreateSimulationText()
        {
            return new GUIStyle
            {
                stretchWidth = true,
                stretchHeight = true,
                alignment = TextAnchor.UpperCenter,
                fontSize = Screen.height / 20,
                fontStyle = FontStyle.Bold,
                normal = {textColor = Color.grey}
            };
        }

        private void RefreshSimulationVariables()
        {
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

            FlightGlobals.ActiveVessel.isPersistent = !cfg.InSimulation;
            FlightDriver.fetch.bypassPersistence = cfg.InSimulation;

            FlightDriver.CanRevertToPostInit = cfg.InSimulation;
            FlightDriver.CanRevertToPrelaunch = cfg.InSimulation;
        }

        private void Update()
        {
            if (!cfg.InSimulation || !altimeterSliderButtons.hoverArea.enabled)
                return;

            // Lock recover & return to space center button
            altimeterSliderButtons.hoverArea.enabled = false;
            altimeterSliderButtons.slidingTab.enabled = false;
            altimeterSliderButtons.spaceCenterButton.enabled = false;
            altimeterSliderButtons.vesselRecoveryButton.enabled = false;
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