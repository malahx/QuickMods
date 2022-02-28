using System;
using KSP.Localization;
using KSP.UI.Screens;
using QuickMods.utils;
using QuickMods.utils.Toolbar;
using UnityEngine;

namespace QuickIronMan
{
    internal enum SimVesselStatus
    {
        Initial,
        Loaded,
        Launched,
        Waiting
    }
    
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Flight : MonoBehaviour
    {
        [KSPField(isPersistant = true)]
        private static SimVesselStatus _status = SimVesselStatus.Initial;
        
        private readonly SimConfig sim = SimConfig.INSTANCE;
        private AltimeterSliderButtons altimeterSliderButtons = null;
        
        private GUIStyle textStyle;
        private bool flightReady = false;
        private Toolbar toolbar;
        private void Awake()
        {
            if (!sim.IsLockSimulation())
            {
                toolbar = new Toolbar.Builder()
                    .Config(sim)
                    .Component(this)
                    .Create(StartSimulation, StopSimulationAndRevert);

                if (sim.IsInSimulation())
                    toolbar.SetTrue();
            }
        }

        private void StartSimulation()
        {

            if (FlightDriver.PreLaunchState == null) {
                GameEvents.onGameAboutToQuicksave.Fire();
                Game game = HighLogic.CurrentGame.Updated();
                game.startScene = GameScenes.FLIGHT;
                GamePersistence.SaveGame(game, "simulation", HighLogic.SaveFolder, SaveMode.BACKUP);
                if (FlightGlobals.ClearToSave() == ClearToSaveStatus.CLEAR)
                {
                    GamePersistence.SaveGame(game, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
                    Debug.Log($"[QuickIronMan]({name}) It's clear, I save your game");
                }
                Debug.Log($"[QuickIronMan]({name}) Backup simulation");
            }
            
            sim.SetSimulation(true);
        }

        private void StopSimulationAndRevert()
        {
            sim.SetSimulation(false);
            if (_status != SimVesselStatus.Launched)
            {
                return;
            }
            if (FlightDriver.PreLaunchState == null)
            {
                
                var configNode = GamePersistence.LoadSFSFile("simulation", HighLogic.SaveFolder);
                if (configNode != null && HighLogic.CurrentGame != null)
                {
                    var game = GamePersistence.LoadGameCfg(configNode, "simulation", true, false);
                    FlightDriver.StartAndFocusVessel(game, game.flightState.activeVesselIdx);
                    _status = SimVesselStatus.Waiting;
                    Debug.Log($"[QuickIronMan]({name}) Revert to save");
                    return;
                }
                _status = SimVesselStatus.Launched;
                Debug.Log($"[QuickIronMan]({name}) Something seems wrong, no simulation save, can't return before simulation start");
                return;
            } 
            FlightDriver.RevertToLaunch();
            Debug.Log($"[QuickIronMan]({name}) Revert to launch");
        }

        private void Start()
        {
            textStyle = GuiUtils.PrepareBigText(Color.grey);
            sim.RefreshSimulationVariables();

            altimeterSliderButtons = (AltimeterSliderButtons)FindObjectOfType(typeof(AltimeterSliderButtons));

            GameEvents.onLaunch.Add(OnLaunch);
            GameEvents.onFlightReady.Add(OnFlightReady);

            if (_status != SimVesselStatus.Waiting)
            {
                if (FlightGlobals.fetch.activeVessel.situation != Vessel.Situations.PRELAUNCH)
                {
                    _status = SimVesselStatus.Launched;
                    Debug.Log($"[QuickIronMan]({name}) This vessel is already launched");
                }
                else
                {
                    _status = SimVesselStatus.Loaded;
                }
            }

            Debug.Log($"[QuickIronMan]({name}) Start, simulation: {sim.IsInSimulation()}");
        }

        private void OnLaunch(EventReport data)
        {
            _status = SimVesselStatus.Launched;

            if (!sim.IsInSimulation()) 
            {
                FlightDriver.PreLaunchState = null;
                FlightDriver.PostInitState = null;
                Debug.Log($"[QuickIronMan]({name}) lost revert possibility");
            }

            Debug.Log($"[QuickIronMan]({name}) Launch, Simulation: {sim.IsInSimulation()}");
        }
        
        void OnFlightReady()
        {
            flightReady = true;
            Debug.Log($"[QuickIronMan]({name}) Flight ready");
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

        private void FixedUpdate()
        {
            if (_status != SimVesselStatus.Waiting || !flightReady) return;
            
            PauseMenu.Display();
            _status = SimVesselStatus.Launched;
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
            GameEvents.onFlightReady.Remove(OnFlightReady);

            toolbar?.Destroy();

            Debug.Log($"[QuickIronMan]({name}) Destroy");
        }
    }
}