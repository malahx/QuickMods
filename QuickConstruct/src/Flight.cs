using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KSP.Localization;
using UnityEngine;

namespace QuickConstruct
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Flight : MonoBehaviour
    {
        private GUIStyle textStyle;
        private bool constructionStarted;

        private readonly List<PartResource> resources = new List<PartResource>();

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

            // Prepare event listeners
            GameEvents.onFlightReady.Add(OnFlightReady);
            GameEvents.onLaunch.Add(OnLaunch);
            SimulationModsCompatibility.OnSimulation.Add(OnSimulation);

            Debug.Log($"[QuickConstruct]({name}) Start");
        }

        private void OnSimulation(bool simulation)
        {
            // Toggle construction
            if (simulation)
                InputLockManager.RemoveControlLock("vessel_noControl_quickconstruct");
            else
                InputLockManager.SetControlLock(ControlTypes.ALL_SHIP_CONTROLS, "vessel_noControl_quickconstruct");
        }

        private void OnFlightReady()
        {
            // Don't know if it is useful but I've this check since many time ...
            if (FlightGlobals.ActiveVessel == null)
            {
                return;
            }

            // Delete construct if there is no time to pass or the vessel isn't a new vessel
            if (ConstructScenario.Instance == null ||
                !ConstructScenario.Instance.HasTimeToPass ||
                FlightGlobals.ActiveVessel.distanceTraveled != 0)
            {
                Destroy(this);
                return;
            }

            // No construction in simulation, but it can be toggle
            if (SimulationModsCompatibility.Instance != null && SimulationModsCompatibility.Instance.IsInSimulation()) 
                return;
            
            InputLockManager.SetControlLock(ControlTypes.ALL_SHIP_CONTROLS, "vessel_noControl_quickconstruct");

            Debug.Log($"[QuickConstruct]({name}) Prepare construct...");
        }

        private void OnLaunch(EventReport data)
        {
            // When the vessel is hard launch the construction is useless ;) 
            if (SimulationModsCompatibility.Instance == null || !SimulationModsCompatibility.Instance.IsInSimulation())
                Destroy(this);                
        }

        private void ActivateVessel()
        {
            StartCoroutine(nameof(WaitWarpFinished));
        }

        private IEnumerator WaitWarpFinished()
        {
            // again a better way exists if you know it, send me in issues ;)
            yield return new WaitForSeconds(1);
            
            // Unlock vessel
            UnlockElectricity();
            // FlightGlobals.ActiveVessel.AttachPatchedConicsSolver();
            InputLockManager.RemoveControlLock("vessel_noControl_quickconstruct");
            
            Debug.Log($"[QuickConstruct]({name}) Construction finished");
            Destroy(this);
        }

        private void Update()
        {
            if (!GameSettings.LAUNCH_STAGES.GetKeyDown() ||
                InputLockManager.GetControlLock("vessel_noControl_quickconstruct") == ControlTypes.None ||
                constructionStarted) 
                return;
            
            // Avoid empty electricity on the launchpad :/
            LockElectricity();
            
            // It seems to be needed for high warp, I've try to work with Unload() or MakeInactive() but no luck
            // FlightGlobals.ActiveVessel.DetachPatchedConicsSolver();
            
            TimeWarp.fetch.WarpTo(Planetarium.GetUniversalTime() + ConstructScenario.Instance.EditorTimePassed);
            
            ConstructScenario.OnEndsConstruction.Add(ActivateVessel);
            constructionStarted = true;
                
            Debug.Log($"[QuickConstruct]({name}) Start construct...");
        }

        // Display construction message
        private void OnGUI()
        {
            if (!ConstructScenario.Instance.HasTimeToPass || InputLockManager.GetControlLock("vessel_noControl_quickconstruct") == ControlTypes.None) 
                return;
            
            var printTime = KSPUtil.PrintTime(ConstructScenario.Instance.EditorTimePassed, 2, false);
            var message = constructionStarted ? Localizer.Format("quickconstruct_flight_construct_message", printTime) : 
                Localizer.Format("quickconstruct_flight_prepare_construct_message", printTime, GameSettings.LAUNCH_STAGES.primary.name);

            GUILayout.BeginArea(new Rect (0, Screen.height / 10f, Screen.width - 0, 200), textStyle);
            GUILayout.Label(message, textStyle);
            GUILayout.EndArea();
        }

        private void OnDestroy()
        {
            // Remove all listeners & locks
            UnlockElectricity();

            GameEvents.onFlightReady.Remove(OnFlightReady);
            GameEvents.onLaunch.Remove(OnLaunch);
            ConstructScenario.OnEndsConstruction.Remove(ActivateVessel);
            SimulationModsCompatibility.OnSimulation.Remove(OnSimulation);
            
            InputLockManager.RemoveControlLock("vessel_noControl_quickconstruct");
            
            Debug.Log($"[QuickConstruct]({name}) Destroy");
        }

        private void LockElectricity()
        {
            FlightGlobals.ActiveVessel.Parts.ForEach(p =>
            {
                var elect = p.Resources.FirstOrDefault(r => r.resourceName == "ElectricCharge");
                if (elect != null && elect.flowState)
                {
                    elect.flowState = false;
                    resources.Add(elect);
                }
            });
        }
        
        private void UnlockElectricity()
        {
            // Enable electricity
            foreach (var r in new List<PartResource>(resources))
            {
                r.flowState = true;
                resources.Remove(r);
            }
        }
    }
}