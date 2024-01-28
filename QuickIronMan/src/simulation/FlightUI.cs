using KSP.Localization;
using KSP.UI.Screens;
using QuickIronMan.toolbar;
using QuickIronMan.utils;
using QuickMods.utils.Toolbar;
using UnityEngine;

namespace QuickIronMan.simulation {
    
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FlightUI : MonoBehaviour
    {
        
        private readonly Simulation sim = Simulation.INSTANCE;
        private AltimeterSliderButtons altimeterSliderButtons = null;
        
        private GUIStyle textStyle;

        private Toolbar toolbar;

        private void Awake() {
            if (!sim.IsLockedSimulation()) {
                InitToolbar();
            }

            Simulation.OnLockSimulation.Add(DestroyToolbar);
            Simulation.OnUnlockSimulation.Add(InitToolbar);

            Debug.Log($"[QuickIronMan]({name}) Awake");
        }

        private void InitToolbar() {
            if (toolbar == null) {

                toolbar = new Toolbar.Builder()
                        .Config(new ToolbarConfig())
                        .Component(this)
                        .Create(sim.StartSimulation, sim.StopSimulationAndRevert);
            }

            if (sim.IsInSimulation())
                toolbar.SetTrue();

            Debug.Log($"[QuickIronMan]({name}) Init toolbar");
        }

        private void DestroyToolbar() {
            toolbar?.Destroy();
            toolbar = null;
            Debug.Log($"[QuickIronMan]({name}) Destroy toolbar!");
        }

        private void Start()
        {

            textStyle = GuiUtils.PrepareBigText(Color.grey);

            altimeterSliderButtons = (AltimeterSliderButtons)FindObjectOfType(typeof(AltimeterSliderButtons));

            GameEvents.onFlightReady.Add(OnFlightReady);

            Debug.Log($"[QuickIronMan]({name}) Start");
        }

        void OnFlightReady()
        {
            if (sim.IsInSimulation() && altimeterSliderButtons.hoverArea.enabled)
            {
                // Lock recover & return to space center button
                altimeterSliderButtons.hoverArea.enabled = false;
                altimeterSliderButtons.slidingTab.enabled = false;
                altimeterSliderButtons.spaceCenterButton.enabled = false;
                altimeterSliderButtons.vesselRecoveryButton.enabled = false;
            } 
            if (!sim.IsInSimulation() && !altimeterSliderButtons.hoverArea.enabled)
            {
                // Lock recover & return to space center button
                altimeterSliderButtons.hoverArea.enabled = true;
                altimeterSliderButtons.slidingTab.enabled = true;
                altimeterSliderButtons.spaceCenterButton.enabled = true;
                altimeterSliderButtons.vesselRecoveryButton.enabled = true;
            } 
            Debug.Log($"[QuickIronMan]({name}) Flight ready");
        }

        private void OnGUI()
        {
            if (!sim.IsInSimulation()) 
                return;
            
            GUILayout.BeginArea (new Rect (0, Screen.height / 10f, Screen.width - 0, 160), textStyle);
            GUILayout.Label (Localizer.Format("quickironman_simulation_message"), textStyle);
            GUILayout.EndArea ();
        }

        private void OnDestroy()
        {
            
            GameEvents.onFlightReady.Remove(OnFlightReady);
            Simulation.OnLockSimulation.Remove(DestroyToolbar);
            Simulation.OnUnlockSimulation.Remove(InitToolbar);

            DestroyToolbar();

            Debug.Log($"[QuickIronMan]({name}) Destroy");
        }
    }
}