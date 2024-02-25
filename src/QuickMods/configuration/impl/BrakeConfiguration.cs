using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class BrakeConfiguration(ConfigFile config) : ConfigurationBase("QuickBrake")
{
    private ConfigEntry<bool> _enableBrakeAtLaunchPad;
    private ConfigEntry<bool> _enableBrakeAtRunway;
    private ConfigEntry<bool> _enableBrakeWhenControlLost;
    private ConfigEntry<bool> _enableUnBrakeAtLaunch;
    private ConfigEntry<bool> _alwaysBrakeLandedRover;
    private ConfigEntry<bool> _alwaysBrakeLandedPlane;
    private ConfigEntry<bool> _alwaysBrakeLandedVessel;

    public bool EnableBrakeAtLaunchPad() => _enableBrakeAtLaunchPad.Value;
    public bool EnableBrakeAtRunway() => _enableBrakeAtRunway.Value;
    public bool EnableBrakeWhenControlLost() => _enableBrakeWhenControlLost.Value;
    public bool EnableUnBrakeAtLaunch() => _enableUnBrakeAtLaunch.Value;
    public bool AlwaysBrakeLandedRover() => _alwaysBrakeLandedRover.Value;
    public bool AlwaysBrakeLandedPlane() => _alwaysBrakeLandedPlane.Value;
    public bool AlwaysBrakeLandedVessel() => _alwaysBrakeLandedVessel.Value;

    public override void Init()
    {
        base.Init();

        _enableBrakeAtLaunchPad = config.Bind("QuickMods/Brake", "EnableBrakeAtLaunchPad", false, "Enable or disable brake at launchpad at load");
        _enableBrakeAtRunway = config.Bind("QuickMods/Brake", "EnableBrakeAtRunway", false, "Enable or disable brake at runway at load");
        _enableBrakeWhenControlLost = config.Bind("QuickMods/Brake", "EnableBrakeWhenControlLost", false, "Enable or disable brake when control is lost");
        _enableUnBrakeAtLaunch = config.Bind("QuickMods/Brake", "EnableUnBrakeAtLaunch", false, "Enable or disable un brake at launch");
        _alwaysBrakeLandedRover = config.Bind("QuickMods/Brake", "AlwaysBrakeLandedRover", false, "Enable or disable brake landed rover at load");
        _alwaysBrakeLandedPlane = config.Bind("QuickMods/Brake", "AlwaysBrakeLandedPlane", false, "Enable or disable brake landed plane at load");
        _alwaysBrakeLandedVessel = config.Bind("QuickMods/Brake", "AlwaysBrakeLandedVessel", false, "Enable or disable brake landed vessel at load");
    }
}