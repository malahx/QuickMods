using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickVesselName
{
    public class VesselNameService
    {
        private readonly Config config;

        public VesselNameService(Config config)
        {
            this.config = config;
        }

        public string RetrieveVesselName(VesselDefinition vesselDefinition)
        {
            string[] names = null;

            // Vessel is a Rover
            if (vesselDefinition.VesselType == VesselType.Rover ||
                config.RoverParts.Contains(vesselDefinition.PartName))
            {
                names = config.RoverNames;
            }
            
            // Vessel is a Probe
            if (vesselDefinition.VesselType == VesselType.Probe)
            {
                names = config.ProbeNames;
            }
            
            // Vessel is a Station
            if (vesselDefinition.VesselType == VesselType.Station)
            {
                names = config.StationNames;
            }

            // Vessel is a Pod
            if (vesselDefinition.HasCrew)
            {
                names = config.CrewedNames;
            }
            
            // Vessel isn't defined
            if (names == null)
            {
                names = config.LauncherNames;
            }
            
            return RetrieveRandomName(names);
        }

        private static string RetrieveRandomName(IReadOnlyList<string> names)
        {
            return names[new Random().Next(0, names.Count - 1)];
        }

        public bool IsARetrievedVesselName(string vesselName)
        {
            return config.CrewedNames.Contains(vesselName) ||
                   config.LauncherNames.Contains(vesselName) ||
                   config.ProbeNames.Contains(vesselName) ||
                   config.RoverNames.Contains(vesselName) ||
                   config.StationNames.Contains(vesselName);
        }
    }
}