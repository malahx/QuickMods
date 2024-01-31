using System.Reflection;
using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class QuickVesselNamesConfiguration : IConfigurationBase
{
    private ConfigEntry<bool> _automaticVesselName;

    public bool AutomaticVesselName()
    {
        return _automaticVesselName.Value;
    }

    public string[] CrewedNames { get; private set; }
    public string[] LauncherNames { get; private set; }
    public string[] ProbeNames { get; private set; }
    public string[] RoverNames { get; private set; }
    public string[] AirPlaneNames { get; private set; }
    public string[] SpacePlaneNames { get; private set; }

    private const string CrewedNamesFile = "/QuickVesselNames/CrewedNames.txt";
    private const string LauncherNamesFile = "/QuickVesselNames/LauncherNames.txt";
    private const string ProbeNamesFile = "/QuickVesselNames/ProbeNames.txt";
    private const string RoverNamesFile = "/QuickVesselNames/RoverNames.txt";
    private const string AirPlaneFile = "/QuickVesselNames/AirPlaneNames.txt";
    private const string SpacePlaneFile = "/QuickVesselNames/SpacePlaneNames.txt";

    public void Init(ConfigFile config)
    {
        _automaticVesselName = config.Bind("QuickMods/VesselNames", "AutomaticVesselName", false, "Enable or disable the automatic vessel name");

        try
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var completePath = $"{path?.Replace(@"\", "/")}";

            CrewedNames = File.ReadAllLines($"{completePath}{CrewedNamesFile}");
            LauncherNames = File.ReadAllLines($"{completePath}{LauncherNamesFile}");
            ProbeNames = File.ReadAllLines($"{completePath}{ProbeNamesFile}");
            RoverNames = File.ReadAllLines($"{completePath}{RoverNamesFile}");
            AirPlaneNames = File.ReadAllLines($"{completePath}{AirPlaneFile}");
            SpacePlaneNames = File.ReadAllLines($"{completePath}{SpacePlaneFile}");

            Debug.Log($"QuickVesselNames[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
        }
        catch (Exception e)
        {
            Debug.LogError($"QuickVesselNames[{MyPluginInfo.PLUGIN_VERSION}] Vessel names could not be load: {e.Message}");
            Debug.LogException(e);
        }
    }
}