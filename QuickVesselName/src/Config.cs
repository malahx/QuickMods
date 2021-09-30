using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace QuickVesselName
{
    public class Config
    {
        public string[] CrewedNames;
        public string[] LauncherNames;
        public string[] ProbeNames;
        public string[] RoverNames;
        public string[] StationNames;
        public string[] RoverParts;

        private const string CrewedNamesFile = "/../CrewedNames.txt";
        private const string LauncherNamesFile = "/../LauncherNames.txt";
        private const string ProbeNamesFile = "/../ProbeNames.txt";
        private const string RoverNamesFile = "/../RoverNames.txt";
        private const string StationNamesFile = "/../StationNames.txt";
        private const string RoverPartsFile = "/../RoverPartList.txt";
        
        [KSPField(isPersistant = true)] internal static readonly Config INSTANCE = new Config();

        private Config()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FilePath = $"{path?.Replace(@"\", "/")}";
            Load();
        }

        private string FilePath { get; }

        private void Load()
        {
            Debug.Log("[QuickVesselName] Load vessel names...");
            try
            {
                CrewedNames = File.ReadAllLines($"{FilePath}{CrewedNamesFile}");
                LauncherNames = File.ReadAllLines($"{FilePath}{LauncherNamesFile}");
                ProbeNames = File.ReadAllLines($"{FilePath}{ProbeNamesFile}");
                RoverNames = File.ReadAllLines($"{FilePath}{RoverNamesFile}");
                StationNames = File.ReadAllLines($"{FilePath}{StationNamesFile}");
                RoverParts = File.ReadAllLines($"{FilePath}{RoverPartsFile}");
                
                Debug.Log("[QuickModsInfo] Vessel names loaded.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[QuickModsInfo] Vessel names could not be load: {e.Message}");
                Debug.LogException(e);
            }
        }
    }
}