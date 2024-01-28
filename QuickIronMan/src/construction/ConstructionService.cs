using System.Collections.Generic;
using System.Linq;
using KSP.Localization;
using KSP.UI;
using QuickIronMan.construction.model;
using QuickIronMan.utils;
using UnityEngine;

namespace QuickIronMan.construction {
    public class ConstructionService
    {
        public static readonly ConstructionService Instance = new ConstructionService();

        private readonly List<VesselConstruction> constructions = new List<VesselConstruction>();

        public List<VesselConstruction> Constructions()
        {
            return constructions;
        }

        public void AddToConstruction(ShipTemplate shipTemplate)
        {
            var vessel = VesselUtils.GenerateId(shipTemplate);
            var constructionTime = VesselUtils.ConstructionTime(shipTemplate);
            var universalTime = Planetarium.GetUniversalTime();
            var shipName = shipTemplate.shipName.StartsWith("#")
                ? Localizer.Format(shipTemplate.shipName)
                : shipTemplate.shipName;

            var alarmToSet = new AlarmTypeRaw
            {
                title = $"{shipName}: Construction...",
                description = "The vessel is in construction, each bolt must be tightened.",
                actions =
                {
                    warp = AlarmActions.WarpEnum.KillWarp,
                    message = AlarmActions.MessageEnum.Yes,
                    deleteWhenDone = true
                },
                ut = universalTime + constructionTime
            };
            AlarmClockScenario.AddAlarm(alarmToSet);

            Add(new VesselConstruction
            {
                AlarmId = alarmToSet.Id,
                Id = vessel,
                Path = shipTemplate.filename,
                Name = shipName,
                StartedAt = universalTime,
                Time = constructionTime
            });
        }

        public void Add(VesselConstruction vesselConstruction)
        {
            constructions.Add(vesselConstruction);
        }

        public void Remove(VesselConstruction vesselConstruction)
        {
            constructions.Remove(vesselConstruction);
        }

        public void Reset()
        {
            constructions.Clear();
        }

        public int ConstructionNumber()
        {
            return constructions.Count;
        }

        public void RemoveShipConstructionFromAlarm(uint alarmId, bool actioned)
        {
            var vessels = constructions.Where(v => alarmId == v.AlarmId && !actioned);
            foreach (var vessel in vessels)
            {
                Remove(vessel);
                var message = $"{vessel.Name} ship construction cancelled";
                ScreenMessages.PostScreenMessage(message, 5);
                Debug.Log($"[QuickIronMan](ConstructionService): {message} (from alarm removed)");
                break;
            }
        }

        public void LaunchShip(ShipTemplate shipTemplate)
        {
            FindVessel(shipTemplate).Status = VesselStatus.Launch;
        }

        public void CleanedConstruction(Dictionary<ShipTemplate, VesselListItem>.KeyCollection keys)
        {
            var vesselNames = Enumerable.ToList(Enumerable.Select(keys, VesselUtils.GenerateId));
            var vessels = Enumerable.Where(constructions, v => !vesselNames.Contains(v.Id));
            foreach (var vessel in vessels)
            {
                AlarmClockScenario.DeleteAlarm(vessel.AlarmId);
                constructions.Remove(vessel);
                Debug.Log(
                    $"[QuickIronMan](ConstructionService): Deleted construction of {vessel.Name} in {vesselNames}");
            }
        }

        public bool CanLaunch(ShipTemplate shipTemplate)
        {
            var vessel = VesselUtils.GenerateId(shipTemplate);
            return constructions.Any(v => v.Id == vessel && Planetarium.GetUniversalTime() > v.StartedAt + v.Time);
        }

        public int InConstruction(ShipTemplate shipTemplate)
        {
            var vessel = VesselUtils.GenerateId(shipTemplate);
            var vessels =
                constructions.FindAll(v => v.Id == vessel && Planetarium.GetUniversalTime() < v.StartedAt + v.Time);
            return vessels.Capacity;
        }

        public bool CanConstruct()
        {
            return constructions.Count < 5;
        }

        public bool ConstructionStarted(ShipTemplate shipTemplate)
        {
            return FindVessel(shipTemplate) != null;
        }

        public double ConstructionFinishAt(ShipTemplate shipTemplate)
        {
            var vessel = FindVessel(shipTemplate);
            if (vessel == null)
                return VesselUtils.ConstructionTime(shipTemplate);
            return vessel.StartedAt + vessel.Time - Planetarium.GetUniversalTime();
        }

        private VesselConstruction FindVessel(ShipTemplate shipTemplate)
        {
            return constructions.Find(c => c.Id.Equals(VesselUtils.GenerateId(shipTemplate)));
        }
    }
}