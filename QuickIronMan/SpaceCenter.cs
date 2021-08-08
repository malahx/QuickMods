using System;
using UnityEngine;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpaceCenter : MonoBehaviour
    {
        private void Start()
        {
            SimConfig.INSTANCE.InSimulation = false;
            
            HighLogic.CurrentGame.Parameters.Flight.CanRestart = false;
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor = false;
            
            HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad = true;
            HighLogic.CurrentGame.Parameters.Flight.CanQuickSave = true;
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation = true;
            HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsNear = true;
            HighLogic.CurrentGame.Parameters.Flight.CanSwitchVesselsFar = true;
            HighLogic.CurrentGame.Parameters.Flight.CanEVA = true;
            HighLogic.CurrentGame.Parameters.Flight.CanBoard = true;
            HighLogic.CurrentGame.Parameters.Flight.CanAutoSave = true;
            HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter = true;

            FlightDriver.CanRevertToPostInit = false;
            FlightDriver.CanRevertToPrelaunch = false;
            
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Ironman save enabled.");
            Destroy(this);
        }

        private void OnDestroy()
        {
            Debug.Log($"QuickIronMan[{SimConfig.INSTANCE.Version}] Destroyed.");
        }
    }
}