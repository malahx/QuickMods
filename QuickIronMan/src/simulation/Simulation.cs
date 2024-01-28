using QuickIronMan.simulation.model;
using UnityEngine;

namespace QuickIronMan.simulation {
    public class Simulation
    {
        [KSPField(isPersistant = true)] public static readonly Simulation INSTANCE = new Simulation();

        public static EventVoid OnEnterSimulation = new EventVoid(nameof(OnEnterSimulation));
        public static EventVoid OnExitSimulation = new EventVoid(nameof(OnExitSimulation));
        public static EventVoid OnLockSimulation = new EventVoid(nameof(OnLockSimulation));
        public static EventVoid OnUnlockSimulation = new EventVoid(nameof(OnUnlockSimulation));

        private bool inSimulation = false;
        private bool locked = false;
        private VesselStatus _vesselStatus = VesselStatus.Initial;
        internal VesselStatus VesselStatus {
            get {
                return _vesselStatus;
            }
            private set {
                _vesselStatus = value;
            }
        }

        public bool IsInSimulation()
        {
            return inSimulation || locked;
        }

        public bool IsLockedSimulation()
        {
            return locked;
        }

        public void InitSimulation() {

            RefreshSimulationVariables();

            if (VesselStatus != VesselStatus.Waiting) {
                if (FlightGlobals.fetch.activeVessel.situation != Vessel.Situations.PRELAUNCH) {
                    VesselStatus = VesselStatus.Launched;
                    Debug.Log($"[QuickIronMan](Simulation) This vessel is already launched");
                } else {
                    VesselStatus = VesselStatus.Loaded;
                    Debug.Log($"[QuickIronMan](Simulation) This vessel is loaded");
                }
            }
        }

        public void VesselLaunched() {
            VesselStatus = VesselStatus.Launched;
            Debug.Log($"[QuickIronMan](Simulation) Vessel launched");
        }

        public void SaveSimulation() {
            GameEvents.onGameAboutToQuicksave.Fire();
            Game game = HighLogic.CurrentGame.Updated();
            game.startScene = GameScenes.FLIGHT;
            GamePersistence.SaveGame(game, "simulation", HighLogic.SaveFolder, SaveMode.BACKUP);
            if (FlightGlobals.ClearToSave() == ClearToSaveStatus.CLEAR) {
                GamePersistence.SaveGame(game, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
                Debug.Log($"[QuickIronMan](Simulation) It's clear, I save your game");
            }
        }

        public void StartSimulation() {

            if (FlightDriver.PreLaunchState == null) {
                SaveSimulation();
                Debug.Log($"[QuickIronMan](Simulation) Backup simulation");
            }

            SetSimulation(true);
        }

        public void StopSimulationAndRevert() {
            SetSimulation(false);
            if (VesselStatus != VesselStatus.Launched) {
                return;
            }

            if (FlightDriver.PreLaunchState != null) {
                VesselStatus = VesselStatus.Loaded;
                FlightDriver.RevertToLaunch();
                Debug.Log($"[QuickIronMan](Simulation) Revert to launch");
            } else {
                var configNode = GamePersistence.LoadSFSFile("simulation", HighLogic.SaveFolder);
                if (configNode != null && HighLogic.CurrentGame != null) {
                    VesselStatus = VesselStatus.Waiting;
                    var game = GamePersistence.LoadGameCfg(configNode, "simulation", true, false);
                    FlightDriver.StartAndFocusVessel(game, game.flightState.activeVesselIdx);
                    Debug.Log($"[QuickIronMan](Simulation) Revert to save");
                    return;
                }

                VesselStatus = VesselStatus.Launched;

                Debug.Log($"[QuickIronMan](Simulation) Something seems wrong, no simulation save, can't return before simulation start");
            }
        }

        public void SetSimulation(bool simulation)
        {
            var initialState = IsInSimulation();
            
            inSimulation = simulation || locked;

            RefreshSimulationVariables();

            if (IsInSimulation() && !initialState)
                OnEnterSimulation.Fire();
            else if (!IsInSimulation() && initialState)
                OnExitSimulation.Fire();

            Debug.Log($"[QuickIronMan](Simulation) Set simulation: {IsInSimulation()}, locked: {locked}, scene: {HighLogic.LoadedScene}");
        }

        public void LockSimulation(bool lockSimulation)
        {
            var initialState = IsInSimulation();
            
            locked = lockSimulation;

            RefreshSimulationVariables();

            if (IsInSimulation() && !initialState)
                OnEnterSimulation.Fire();
            else if (!IsInSimulation() && initialState)
                OnExitSimulation.Fire();

            if (IsLockedSimulation())
                OnLockSimulation.Fire();
            else
                OnUnlockSimulation.Fire();

            Debug.Log($"[QuickIronMan](Simulation) Lock Simulation: {locked}");
        }

        public void ResetSimulation()
        {
            SetSimulation(false);
            Debug.Log($"[QuickIronMan](Simulation) Reset simulation");
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
            Debug.Log($"[QuickIronMan](Simulation) Refresh Simulation Variables");
        }

        public void LostRevert() {
            FlightDriver.CanRevertToPostInit = false;
            FlightDriver.CanRevertToPrelaunch = false;
            FlightDriver.PreLaunchState = null;
            FlightDriver.PostInitState = null;

            Debug.Log($"[QuickIronMan](Simulation) Lost Revert (it is now disabled)");
        }
    }
}