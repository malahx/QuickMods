using System;
using System.IO;
using System.Reflection;
using QuickLibrary;
using UnityEngine;

namespace QuickIronMan
{
    public class SimConfig : Simulation, Toolbar.IToolbarConfig
    {
        [KSPField(isPersistant = true)] public static readonly SimConfig INSTANCE = new SimConfig();

        public const string SimulationTexturePath = "QuickMods/QuickIronMan/Textures/sim";

        public string ModName() => "QuickIronMan";
        public string LargeToolbarIconActive() => "QuickMods/QuickIronMan/Textures/toolbar_insim";
        public string LargeToolbarIconInactive() => "QuickMods/QuickIronMan/Textures/toolbar_sim";
        public string SmallToolbarIconActive() => "QuickMods/QuickIronMan/Textures/toolbar_insim";
        public string SmallToolbarIconInactive()=> "QuickMods/QuickIronMan/Textures/toolbar_sim";

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
            
            Debug.Log($"[QuickIronMan](Simulation) Set simulation: {IsInSimulation()}");
        }

        public void ResetSimulation()
        {
            SetSimulation(DefaultIsSimulation);
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