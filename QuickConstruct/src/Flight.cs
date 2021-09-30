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
        private bool constructionInitialized;

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

            // Wait the vessel is loaded
            GameEvents.onFlightReady.Add(OnFlightReady);

            Debug.Log($"[QuickConstruct]({name}) Start");
        }

        private void OnFlightReady()
        {
            if (FlightGlobals.ActiveVessel == null)
            {
                return;
            }

            // No construction needs without the scenario, without time to pass, this is not a new vessel or the game is a simulation
            if (ConstructScenario.Instance == null ||
                !ConstructScenario.Instance.HasTimeToPass ||
                FlightGlobals.ActiveVessel.distanceTraveled != 0 ||
                (SimulationModsCompatibility.Instance != null && SimulationModsCompatibility.Instance.IsInSimulation()))
            {
                Destroy(this);
                return;
            }

            // perhaps a better way exists ?
            StartCoroutine(nameof(WaitLoadingFinished));

            Debug.Log($"[QuickConstruct]({name}) Construct...");
        }

        private IEnumerator WaitLoadingFinished()
        {
            yield return new WaitForEndOfFrame();
            
            // Lock vessel, lock electricity, warp and wait the warp is finished
            InputLockManager.SetControlLock(ControlTypes.ALL_SHIP_CONTROLS, "vessel_noControl_quickconstruct");
            
            // Avoid empty electricity on the launchpad :/
            LockElectricity();
            
            // It seems to be needed for high warp, I've try to work with Unload() or MakeInactive() but no luck
            FlightGlobals.ActiveVessel.DetachPatchedConicsSolver();
            
            // Wait detach has finished before warp, perhaps useless ?
            yield return new WaitForEndOfFrame();
            
            TimeWarp.fetch.WarpTo(Planetarium.GetUniversalTime() + ConstructScenario.Instance.EditorTimePassed);
            
            ConstructScenario.OnEndsConstruction.Add(ActivateVessel);
            constructionInitialized = true;

            Debug.Log($"[QuickConstruct]({name}) Construct...");
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

        private void ActivateVessel()
        {
            StartCoroutine(nameof(WaitWarpFinished));
        }

        private IEnumerator WaitWarpFinished()
        {
            // again a better way exists if you know it, send me in issues ;)
            yield return new WaitForSeconds(1);
            
            UnlockElectricity();

            // Unlock vessel
            FlightGlobals.ActiveVessel.AttachPatchedConicsSolver();
            InputLockManager.RemoveControlLock("vessel_noControl_quickconstruct");
            
            Debug.Log($"[QuickConstruct]({name}) Construction finished");
            Destroy(this);
        }

        // Display construction message
        private void OnGUI()
        {
            if (!constructionInitialized || !ConstructScenario.Instance.HasTimeToPass) 
                return;
            
            var printTime = KSPUtil.PrintTime(ConstructScenario.Instance.EditorTimePassed, 2, false);
            
            GUILayout.BeginArea (new Rect (0, Screen.height / 10f, Screen.width - 0, 160), textStyle);
            GUILayout.Label (Localizer.Format("quickconstruct_flight_construct_message", printTime), textStyle);
            GUILayout.EndArea ();
        }

        private void OnDestroy()
        {
            // Remove all listeners & locks
            UnlockElectricity();

            GameEvents.onFlightReady.Remove(OnFlightReady);
            ConstructScenario.OnEndsConstruction.Remove(ActivateVessel);
            
            InputLockManager.RemoveControlLock("vessel_noControl_quickconstruct");
            
            Debug.Log($"[QuickConstruct]({name}) Destroy");
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