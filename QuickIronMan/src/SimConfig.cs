using System;
using System.IO;
using System.Reflection;
using QuickLibrary;
using UnityEngine;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        private void Start()
        {
            Toolbar.Register("QuickIronMan");
        }
    }

    public class SimConfig : Simulation
    {
        [KSPField(isPersistant = true)] public static readonly SimConfig INSTANCE = new SimConfig();

        public const string SimulationTexturePath = "QuickMods/QuickIronMan/Textures/sim";
        public const string ToolbarInSimulationTexturePath = "QuickMods/QuickIronMan/Textures/toolbar_insim.png";
        public const string ToolbarSimulationTexturePath = "QuickMods/QuickIronMan/Textures/toolbar_sim.png";

        private SimConfig()
        {
            Instance = this;
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            File = $"{path?.Replace(@"\", "/")}/../Config.txt";
            Load();
        }

        public bool DefaultIsSimulation { get; private set; }
        public KeyCode Key { get; private set; }
        private string File { get; }
        
        private bool inSimulation = false;

        public override bool IsInSimulation()
        {
            return inSimulation;
        }

        public override void SetSimulation(bool simulation)
        {
            inSimulation = simulation;
            
            RefreshSimulationVariables();
                
            if (IsInSimulation())
                OnEnterSimulation.Fire();
            else
                OnExitSimulation.Fire();
        }

        public void RefreshSimulationVariables()
        {
            HighLogic.CurrentGame.Parameters.Flight.CanRestart = IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor = IsInSimulation();

            HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanQuickSave = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsNear = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsFar = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanEVA = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanBoard = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanAutoSave = !IsInSimulation();
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter = !IsInSimulation();

            if (!HighLogic.LoadedSceneIsFlight) return;
            
            if (FlightGlobals.ActiveVessel != null)
                FlightGlobals.ActiveVessel.isPersistent = !IsInSimulation();
                
            if (FlightDriver.fetch != null)
                FlightDriver.fetch.bypassPersistence = IsInSimulation();

            FlightDriver.CanRevertToPostInit = IsInSimulation();
            FlightDriver.CanRevertToPrelaunch = IsInSimulation();
        }

        private void Load()
        {
            Debug.Log($"[QuickIronMan](Simulation) Load configuration: {File}");
            try
            {
                InitConfigs(ConfigNode.Load(File).GetNode("QIM"));

                Debug.Log($"[QuickIronMan](Simulation) Configuration loaded.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[QuickIronMan](Simulation) Configuration could not be load: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void InitConfigs(ConfigNode cfg)
        {
            DefaultIsSimulation = bool.Parse(cfg.GetValue("defaultIsSimulation"));
            Key = Enum.TryParse(cfg.GetValue("key"), out KeyCode value)
                ? value
                : KeyCode.Space;
        }
    }
}