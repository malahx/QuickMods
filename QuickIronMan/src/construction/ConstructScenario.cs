using System;
using System.Linq;
using QuickIronMan.construction.model;
using QuickIronMan.simulation;
using UnityEngine;

namespace QuickIronMan.construction {
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT,
        GameScenes.TRACKSTATION)]
    public class ConstructScenario : ScenarioModule
    {

        public override void OnAwake() 
        {

            GameEvents.onAlarmRemoving.Add(OnAlarmRemoving);
            GameEvents.onFlightReady.Add(OnFlightReady);

            Debug.Log($"[QuickIronMan]({name}): OnAwake");
        }

        private void OnFlightReady()
        {
            var toLaunch = false;
            foreach (var vessel in Enumerable.Where(ConstructionService.Instance.Constructions(),
                         v => v.Status == VesselStatus.Launch))
            {
                ConstructionService.Instance.Remove(vessel);
                toLaunch = true;
                Debug.Log($"[QuickIronMan]({name}) Vessel constructed & ready to start: {vessel.Name}");
                break;
            }

            if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.PRELAUNCH && !Simulation.INSTANCE.IsLockedSimulation())
            {
                FlightDriver.PreLaunchState = null;
                FlightDriver.PostInitState = null;
                Simulation.INSTANCE.SetSimulation(false);
                Simulation.INSTANCE.LockSimulation(false);

                Debug.Log($"[QuickIronMan]({name}): Vessel not in prelaunch, lost simulation");
            }

            Debug.Log($"[QuickIronMan]({name}): OnFlightReady");
        }

        private void OnAlarmRemoving(AlarmTypeBase data)
        {
            ConstructionService.Instance.RemoveShipConstructionFromAlarm(data.Id, data.Actioned);
        }

        // Save the QuickIronMan data
        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);

            var vessels = node.AddNode("VESSELS");
            foreach (var vessel in ConstructionService.Instance.Constructions())
            {
                var v = vessels.AddNode("VESSEL");
                v.AddValue("id", vessel.Id);
                v.AddValue("name", vessel.Name);
                v.AddValue("alarmId", vessel.AlarmId);
                v.AddValue("path", vessel.Path);
                v.AddValue("startedAt", vessel.StartedAt);
                v.AddValue("time", vessel.Time);
                v.AddValue("status", vessel.Status);
            }

            Debug.Log(
                $"[QuickIronMan]({name}): Saved - {ConstructionService.Instance.ConstructionNumber()} vessels to construct");
        }

        // Retrieve QuickIronMan data
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);

            ConstructionService.Instance.Reset();

            if (node.HasNode("VESSELS"))
            {
                var vessels = node.GetNode("VESSELS");
                if (vessels.HasNode("VESSEL"))
                {
                    foreach (var vessel in vessels.GetNodes("VESSEL"))
                    {
                        var v = new VesselConstruction();

                        if (vessel.HasValue("name"))
                            v.Name = vessel.GetValue("name");

                        if (vessel.HasValue("alarmId"))
                            uint.TryParse(vessel.GetValue("alarmId"), out v.AlarmId);

                        if (vessel.HasValue("path"))
                            v.Path = vessel.GetValue("path");

                        if (vessel.HasValue("id"))
                            v.Id = vessel.GetValue("id");

                        if (vessel.HasValue("startedAt"))
                            double.TryParse(vessel.GetValue("startedAt"), out v.StartedAt);

                        if (vessel.HasValue("time"))
                            double.TryParse(vessel.GetValue("time"), out v.Time);

                        if (vessel.HasValue("status"))
                            Enum.TryParse(vessel.GetValue("status"), out v.Status);

                        ConstructionService.Instance.Add(v);
                    }
                }
            }

            Debug.Log(
                $"[QuickIronMan]({name}): Loaded - {ConstructionService.Instance.ConstructionNumber()} vessels to construct");
        }

        private void OnDestroy()
        {
            GameEvents.onAlarmRemoving.Remove(OnAlarmRemoving);
            GameEvents.onFlightReady.Remove(OnFlightReady);
            Debug.Log($"[QuickIronMan]({name}) OnDestroy");
        }
    }
}