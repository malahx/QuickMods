using KSP.Localization;
using KSP.UI.Screens;
using QuickLibrary;
using UnityEngine;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Flight : MonoBehaviour
    {
        private readonly SimConfig sim = SimConfig.INSTANCE;
        private AltimeterSliderButtons altimeterSliderButtons = null;
        
        private GUIStyle textStyle;
        private bool launched;
        private Toolbar toolbar;

        private void Start()
        {
            textStyle = PrepareSimulationText();
            sim.RefreshSimulationVariables();

            altimeterSliderButtons = (AltimeterSliderButtons)FindObjectOfType(typeof(AltimeterSliderButtons));
            
            toolbar = new Toolbar(this, SimConfig.ToolbarInSimulationTexturePath, SimConfig.ToolbarSimulationTexturePath, SimConfig.ToolbarInSimulationTexturePath, SimConfig.ToolbarSimulationTexturePath);
            
            toolbar.Create(
                () => sim.SetSimulation(true),
                () =>
                {
                    sim.SetSimulation(false);
                    if (launched)
                    {
                        FlightDriver.RevertToLaunch();
                    }
                });
            
            if (sim.IsInSimulation())
                toolbar.SetTrue();
            
            GameEvents.onLaunch.Add(OnLaunch);
            
            Debug.Log($"[QuickIronMan]({name}) Start, simulation: {sim.IsInSimulation()}");
        }

        private static GUIStyle PrepareSimulationText()
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

        private void OnLaunch(EventReport data)
        {
            if (!sim.IsInSimulation())
            {
                Debug.Log($"[QuickIronMan]({name}) Launch, not in simulation");
                Destroy(this);
                return;
            }

            launched = true;
            
            Debug.Log($"[QuickIronMan]({name}) Launch, in simulation");
        }

        private void Update()
        {
            if (!sim.IsInSimulation() || !altimeterSliderButtons.hoverArea.enabled)
                return;

            // Lock recover & return to space center button
            altimeterSliderButtons.hoverArea.enabled = false;
            altimeterSliderButtons.slidingTab.enabled = false;
            altimeterSliderButtons.spaceCenterButton.enabled = false;
            altimeterSliderButtons.vesselRecoveryButton.enabled = false;
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
            
            GameEvents.onLaunch.Remove(OnLaunch);

            toolbar?.Destroy();

            Debug.Log($"[QuickIronMan]({name}) Destroy");
        }
    }
}