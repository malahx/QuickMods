using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration.impl;

public class BrakeConfiguration(ConfigFile config) : ConfigurationBase("QuickBrake", config)
{
    private ConfigEntry<ToggleBrakeEnum> _toggleBrake;
    private ConfigEntry<KeyCode> _modifierBrakeKey;
    private ConfigEntry<bool> _unBrakeAtLaunch;
    private ConfigEntry<bool> _brakeWhenControlLost;
    private ConfigEntry<bool> _brakePreLaunchAtLoad;
    private ConfigEntry<bool> _brakeLandedRoverAtLoad;
    private ConfigEntry<bool> _brakeLandedPlaneAtLoad;
    private ConfigEntry<bool> _brakeLandedVesselAtLoad;

    public ToggleBrakeEnum ToggleBrake() => _toggleBrake.Value;
    public KeyCode ModifierBrakeKey() => _modifierBrakeKey.Value;
    public bool UnBrakeAtLaunch() => _unBrakeAtLaunch.Value;
    public bool BrakeWhenControlLost() => _brakeWhenControlLost.Value;
    public bool BrakePreLaunchAtLoad() => _brakePreLaunchAtLoad.Value;
    public bool BrakeLandedRoverAtLoad() => _brakeLandedRoverAtLoad.Value;
    public bool BrakeLandedPlaneAtLoad() => _brakeLandedPlaneAtLoad.Value;
    public bool BrakeLandedVesselAtLoad() => _brakeLandedVesselAtLoad.Value;

    public override void Init()
    {
        base.Init();

        _toggleBrake = Config.Bind("QuickMods/Brake", "ToggleBrake", ToggleBrakeEnum.StockBrake, "StockBrake: the default brake key is like in Stock KSP.\n\nModifier: the default brake key is like in Stock KSP, but with a modifier you can toggle the brake.\n\nToggle: the default brake key is toggle brake.");
        _modifierBrakeKey = Config.Bind("QuickMods/Brake", "ModifierBrakeKey", KeyCode.LeftAlt, "The modifier key for brake Modifier option.");
        _unBrakeAtLaunch = Config.Bind("QuickMods/Brake", "UnBrakeAtLaunch", false, "Enable or disable un brake after launch");
        _brakeWhenControlLost = Config.Bind("QuickMods/Brake", "BrakeWhenControlLost", false, "Enable or disable brake when control is lost");
        _brakePreLaunchAtLoad = Config.Bind("QuickMods/Brake", "BrakePreLaunchAtLoad", false, "Enable or disable brake before launch");
        _brakeLandedRoverAtLoad = Config.Bind("QuickMods/Brake", "BrakeLandedRoverAtLoad", false, "Enable or disable brake landed rover at load");
        _brakeLandedPlaneAtLoad = Config.Bind("QuickMods/Brake", "BrakeLandedPlaneAtLoad", false, "Enable or disable brake landed plane at load");
        _brakeLandedVesselAtLoad = Config.Bind("QuickMods/Brake", "BrakeLandedVesselAtLoad", false, "Enable or disable brake landed vessel at load");
    }

    public override void Destroy()
    {
        base.Destroy();
        Config.Remove(_toggleBrake.Definition);
    }

    public enum ToggleBrakeEnum
    {
        Modifier,
        Toggle,
        StockBrake
    }
}