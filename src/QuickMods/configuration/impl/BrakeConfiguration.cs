using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration.impl;

public class BrakeConfiguration(ConfigFile config) : ConfigurationBase("QuickBrake")
{
    private ConfigEntry<KeyboardShortcut> _toggleBrakeKey;
    private ConfigEntry<bool> _unBrakeAtLaunch;
    private ConfigEntry<bool> _brakeWhenControlLost;
    private ConfigEntry<bool> _brakePreLaunchAtLoad;
    private ConfigEntry<bool> _brakeLandedRoverAtLoad;
    private ConfigEntry<bool> _brakeLandedPlaneAtLoad;
    private ConfigEntry<bool> _brakeLandedVesselAtLoad;

    public KeyboardShortcut ToggleBrakeKey() => _toggleBrakeKey.Value;
    public bool UnBrakeAtLaunch() => _unBrakeAtLaunch.Value;
    public bool BrakeWhenControlLost() => _brakeWhenControlLost.Value;
    public bool BrakePreLaunchAtLoad() => _brakePreLaunchAtLoad.Value;
    public bool BrakeLandedRoverAtLoad() => _brakeLandedRoverAtLoad.Value;
    public bool BrakeLandedPlaneAtLoad() => _brakeLandedPlaneAtLoad.Value;
    public bool BrakeLandedVesselAtLoad() => _brakeLandedVesselAtLoad.Value;

    public override void Init()
    {
        base.Init();

        _toggleBrakeKey = config.Bind("QuickMods/Brake", "ToggleBrakeKey", new KeyboardShortcut(KeyCode.B, KeyCode.LeftAlt), "The key to toggle brake");
        _unBrakeAtLaunch = config.Bind("QuickMods/Brake", "UnBrakeAtLaunch", false, "Enable or disable un brake after launch");
        _brakeWhenControlLost = config.Bind("QuickMods/Brake", "BrakeWhenControlLost", false, "Enable or disable brake when control is lost");
        _brakePreLaunchAtLoad = config.Bind("QuickMods/Brake", "BrakePreLaunchAtLoad", false, "Enable or disable brake before launch");
        _brakeLandedRoverAtLoad = config.Bind("QuickMods/Brake", "BrakeLandedRoverAtLoad", false, "Enable or disable brake landed rover at load");
        _brakeLandedPlaneAtLoad = config.Bind("QuickMods/Brake", "BrakeLandedPlaneAtLoad", false, "Enable or disable brake landed plane at load");
        _brakeLandedVesselAtLoad = config.Bind("QuickMods/Brake", "BrakeLandedVesselAtLoad", false, "Enable or disable brake landed vessel at load");
    }
}