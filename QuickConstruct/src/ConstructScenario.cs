using System;
using System.Collections.Generic;
using QuickConstruct.model;
using QuickConstruct.utils;
using UniLinq;
using UnityEngine;

namespace QuickConstruct
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION)]
    public class ConstructScenario : ScenarioModule
    {
        
        internal static ConstructScenario Instance;

        private readonly List<VesselConstruction> construction = new List<VesselConstruction>();

        public string spaceCenterSelectedShipName;

        public void AddToConstruction(ShipTemplate shipTemplate)
        {
            
            var vessel = VesselUtils.ShipName(shipTemplate);
            var constructionTime = ConstructionTime(shipTemplate);
            var universalTime = Planetarium.GetUniversalTime();

            var alarmToSet = new AlarmTypeRaw
            {
                title = $"{shipTemplate.shipName}: Construction...",
                description = "The vessel is in construction, each bolt must be tightened.",
                actions =
                {
                    warp = AlarmActions.WarpEnum.KillWarp,
                    message = AlarmActions.MessageEnum.Yes,
                    deleteWhenDone = true
                },
                ut = universalTime+constructionTime
            };
            AlarmClockScenario.AddAlarm(alarmToSet);

            construction.Add(new VesselConstruction
            {
                AlarmId = alarmToSet.Id,
                Vessel = vessel,
                Name = shipTemplate.shipName,
                StartedAt = universalTime,
                Time = constructionTime
            });
        }


        public bool CanLaunch(ShipTemplate shipTemplate)
        {
            var vessel = VesselUtils.ShipName(shipTemplate);
            return construction.Any(v => v.Vessel == vessel && Planetarium.GetUniversalTime() > v.StartedAt + v.Time);
        }

        public bool CanConstruct()
        {
            return construction.Count < 5;
        }

        public bool ConstructionStarted(ShipTemplate shipTemplate)
        {
            return construction.Find(c => c.Vessel.Equals(VesselUtils.ShipName(shipTemplate))) != null;
        }

        public double ConstructionFinishAt(ShipTemplate shipTemplate)
        {
            var vessel = construction.Find(c => c.Vessel.Equals(VesselUtils.ShipName(shipTemplate)));
            if (vessel == null)
                return ConstructionTime(shipTemplate);
            return vessel.StartedAt + vessel.Time - Planetarium.GetUniversalTime();
        }

        public double ConstructionTime(ShipTemplate shipTemplate)
        {
            // Minimal construction time 30 Kerbin days
            // Maximal construction time 1 Earth year
            return Math.Min(shipTemplate.partCount * shipTemplate.totalMass * 100 + 3600 * 6 * 30, 3600 * 6 * 30 * 365);
        }
        
        public override void OnAwake()
        {
            if (Instance != null)
            {
                Debug.Log($"[QuickConstruct]({name}): Scenario already loaded ?!? Auto destroy.");
                Destroy(this);
            }
            Instance = this;
            Debug.Log($"[QuickConstruct]({name}): OnAwake");
        }

        private void Start()
        {
            if (HighLogic.LoadedSceneIsEditor)
                SimulationModsCompatibility.Instance.LockSimulation(true);
            else
            {
                SimulationModsCompatibility.Instance.LockSimulation(false);
            }
            
            GameEvents.onAlarmRemoved.Add(OnAlarmRemoved);
            GameEvents.onLaunch.Add(OnLaunch);
            
            if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                spaceCenterSelectedShipName = null;
            
            Debug.Log($"[QuickConstruct]({name}): Start");
        }

        private void OnLaunch(EventReport data)
        {
            for (var i = 0; i < construction.Count; i++)
            {
                var vessel = construction[i];
                
                if (vessel.Vessel != spaceCenterSelectedShipName) continue;
                
                construction.RemoveAt(i);
                break;
            }
            
            Debug.Log($"[QuickConstruct]({name}): OnLaunch");
        }

        private void OnAlarmRemoved(uint data)
        {
            for (var index = 0; index < construction.Count; index++)
            {
                var vessel = construction[index];
                if (data != vessel.AlarmId) continue;
                construction.Remove(vessel);
                Debug.Log($"[QuickConstruct]({name}): Remove vessel construction: {vessel.Name}");
                break;
            }
        }

        // Save the QuickConstruct data
        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);

            var vessels = node.AddNode("VESSELS");
            foreach (var vessel in construction)
            {
                var v = vessels.AddNode("VESSEL");
                v.AddValue("name", vessel.Vessel);
                v.AddValue("startedAt", vessel.StartedAt);
                v.AddValue("time", vessel.Time);
            }

            Debug.Log($"[QuickConstruct]({name}): Saved - {construction.Count}");
        }

        // Retrieve QuickConstruct data
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);

            construction.Clear();
            
            if (node.HasNode("VESSELS"))
            {
                var vessels = node.GetNode("VESSELS");
                if (vessels.HasNode("VESSEL"))
                {
                    foreach (var vessel in vessels.GetNodes("VESSEL"))
                    {
                        var v = new VesselConstruction();
                        
                        if (vessel.HasValue("name"))
                            v.Vessel = vessel.GetValue("name");

                        if (vessel.HasValue("startedAt"))
                            double.TryParse(vessel.GetValue("startedAt"), out v.StartedAt);

                        if (vessel.HasValue("time"))
                            double.TryParse(vessel.GetValue("time"), out v.Time);
                        
                        construction.Add(v);
                    }
                }
            }

            Debug.Log($"[QuickConstruct]({name}): Loaded - {construction.Count}");
        }

        private void OnDestroy()
        {
            GameEvents.onAlarmRemoved.Remove(OnAlarmRemoved);
            GameEvents.onLaunch.Remove(OnLaunch);
            Debug.Log($"[QuickConstruct]({name}) OnDestroy");
        }
    }
}