using KSP.Localization;
using UnityEngine;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Flight : MonoBehaviour
    {
        private GUIStyle textStyle;
        private bool constructionInitialized;

        private void Start()
        {
            
            // Prepare UI
            textStyle = new GUIStyle
            {
                stretchWidth = true,
                stretchHeight = true,
                alignment = TextAnchor.UpperCenter,
                fontSize = Screen.height / 20,
                fontStyle = FontStyle.Bold,
                normal = {textColor = Color.red}
            };
            
            // Wait the vessel is loaded
            GameEvents.onFlightReady.Add(OnFlightReady);
            
            Debug.Log($"[QuickConstruct]({name}) Start");
        }

        
        private void OnFlightReady()
        {
            if (FlightGlobals.ActiveVessel == null) {
                return;
            }

            // No construction needs with the scenario, without time to pass, this not a new vessel or the game is a simulation
            if (ConstructScenario.Instance == null || 
                !ConstructScenario.Instance.HasTimeToPass ||
                FlightGlobals.ActiveVessel.distanceTraveled != 0 || 
                (SimulationModsCompatibility.Instance != null && SimulationModsCompatibility.Instance.IsInSimulation()))
            {
                Destroy(this);
                return;
            }
            
            // Lock vessel, warp and wait the warp is finished
            InputLockManager.SetControlLock(ControlTypes.ALL_SHIP_CONTROLS, "vessel_noControl_quickconstruct");
            FlightGlobals.ActiveVessel.MakeInactive();
            TimeWarp.fetch.WarpTo(Planetarium.GetUniversalTime() + ConstructScenario.Instance.EditorTimePassed);
            ConstructScenario.OnEndsConstruction.Add(ActivateVessel);
            constructionInitialized = true;

            Debug.Log($"[QuickConstruct]({name}) Construct...");
        }

        private void ActivateVessel()
        {
            // Unlock vessel
            InputLockManager.RemoveControlLock("vessel_noControl_quickconstruct");
            FlightGlobals.ActiveVessel.MakeActive();
            Debug.Log($"[QuickConstruct]({name}) Construction finished");
            Destroy(this);
        }

        // Display construction message
        private void OnGUI()
        {
            if (!constructionInitialized || !ConstructScenario.Instance.HasTimeToPass) 
                return;
            
            var printTime = KSPUtil.PrintTime(ConstructScenario.Instance.EditorTimePassed, 1, false);
            
            GUILayout.BeginArea (new Rect (0, Screen.height / 10f, Screen.width - 0, 160), textStyle);
            GUILayout.Label (Localizer.Format("quickconstruct_flight_construct_message", printTime), textStyle);
            GUILayout.EndArea ();
        }

        private void OnDestroy()
        {
            // Remove all listeners & locks
            GameEvents.onFlightReady.Remove(OnFlightReady);
            ConstructScenario.OnEndsConstruction.Remove(ActivateVessel);
            InputLockManager.RemoveControlLock("vessel_noControl_quickconstruct");
            
            Debug.Log($"[QuickConstruct]({name}) Destroy");
        }
    }
}