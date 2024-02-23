using System.Reflection;
using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class VesselNamesConfiguration : ConfigurationBase
{
    private ConfigEntry<bool> _automaticVesselName;
    private ConfigEntry<bool> _customVesselName;

    public bool AutomaticVesselName()
    {
        return _automaticVesselName.Value;
    }

    public bool CustomVesselName()
    {
        return _customVesselName.Value;
    }

    public string[] CrewedNames { get; private set; }
    public string[] LauncherNames { get; private set; }
    public string[] ProbeNames { get; private set; }
    public string[] RoverNames { get; private set; }
    public string[] AirPlaneNames { get; private set; }
    public string[] SpacePlaneNames { get; private set; }
    public string[] CustomNames { get; private set; }

    private const string CrewedNamesFile = "/VesselNames/CrewedNames.txt";
    private const string LauncherNamesFile = "/VesselNames/LauncherNames.txt";
    private const string ProbeNamesFile = "/VesselNames/ProbeNames.txt";
    private const string RoverNamesFile = "/VesselNames/RoverNames.txt";
    private const string AirPlaneFile = "/VesselNames/AirPlaneNames.txt";
    private const string SpacePlaneFile = "/VesselNames/SpacePlaneNames.txt";
    private const string CustomFile = "/VesselNames/CustomNames.txt";

    public new void Init(ConfigFile config)
    {
        base.Init(config);
        
        _automaticVesselName = config.Bind("QuickMods/VesselNames", "AutomaticVesselName", false, "Enable or disable the automatic vessel name");
        _customVesselName = config.Bind("QuickMods/VesselNames", "CustomVesselName", false, $"Enable or disable the custom vessel name, you need to create the file in {CustomFile}");

        try
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var completePath = $"{path?.Replace(@"\", "/")}";

            CrewedNames = File.Exists($"{completePath}{CrewedNamesFile}") ? File.ReadAllLines($"{completePath}{CrewedNamesFile}") : [];
            LauncherNames = File.Exists($"{completePath}{LauncherNamesFile}") ? File.ReadAllLines($"{completePath}{LauncherNamesFile}") : [];
            ProbeNames = File.Exists($"{completePath}{ProbeNamesFile}") ? File.ReadAllLines($"{completePath}{ProbeNamesFile}") : [];
            RoverNames = File.Exists($"{completePath}{RoverNamesFile}") ? File.ReadAllLines($"{completePath}{RoverNamesFile}") : [];
            AirPlaneNames = File.Exists($"{completePath}{AirPlaneFile}") ? File.ReadAllLines($"{completePath}{AirPlaneFile}") : [];
            SpacePlaneNames = File.Exists($"{completePath}{SpacePlaneFile}") ? File.ReadAllLines($"{completePath}{SpacePlaneFile}") : [];
            CustomNames = File.Exists($"{completePath}{CustomFile}") ? File.ReadAllLines($"{completePath}{CustomFile}") : [];

            Debug.Log($"{GetType()}[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
        }
        catch (Exception e)
        {
            Debug.LogError($"{GetType()}[{MyPluginInfo.PLUGIN_VERSION}] Vessel names could not be load: {e.Message}");
            Debug.LogException(e);
        }
    }
}