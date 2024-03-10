using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class BrakeConfiguration(ConfigFile config) : ConfigurationBase("QuickBrake")
{
    private ConfigEntry<ToggleBrakeEnum> _toggleBrake;
    private ConfigEntry<bool> _unBrakeAtLaunch;
    private ConfigEntry<bool> _brakeWhenControlLost;
    private ConfigEntry<bool> _brakePreLaunchAtLoad;
    private ConfigEntry<bool> _brakeLandedRoverAtLoad;
    private ConfigEntry<bool> _brakeLandedPlaneAtLoad;
    private ConfigEntry<bool> _brakeLandedVesselAtLoad;

    public ToggleBrakeEnum ToggleBrake() => _toggleBrake.Value;
    public bool UnBrakeAtLaunch() => _unBrakeAtLaunch.Value;
    public bool BrakeWhenControlLost() => _brakeWhenControlLost.Value;
    public bool BrakePreLaunchAtLoad() => _brakePreLaunchAtLoad.Value;
    public bool BrakeLandedRoverAtLoad() => _brakeLandedRoverAtLoad.Value;
    public bool BrakeLandedPlaneAtLoad() => _brakeLandedPlaneAtLoad.Value;
    public bool BrakeLandedVesselAtLoad() => _brakeLandedVesselAtLoad.Value;

    public override void Init()
    {
        base.Init();

        _toggleBrake = config.Bind("QuickMods/Brake", "ToggleBrake", ToggleBrakeEnum.StockBrake, "StockBrake is no toggle feature like in Stock KSP, Modifier enable toggle only when press modifier+brake key");
        _unBrakeAtLaunch = config.Bind("QuickMods/Brake", "UnBrakeAtLaunch", false, "Enable or disable un brake after launch");
        _brakeWhenControlLost = config.Bind("QuickMods/Brake", "BrakeWhenControlLost", false, "Enable or disable brake when control is lost");
        _brakePreLaunchAtLoad = config.Bind("QuickMods/Brake", "BrakePreLaunchAtLoad", false, "Enable or disable brake before launch");
        _brakeLandedRoverAtLoad = config.Bind("QuickMods/Brake", "BrakeLandedRoverAtLoad", false, "Enable or disable brake landed rover at load");
        _brakeLandedPlaneAtLoad = config.Bind("QuickMods/Brake", "BrakeLandedPlaneAtLoad", false, "Enable or disable brake landed plane at load");
        _brakeLandedVesselAtLoad = config.Bind("QuickMods/Brake", "BrakeLandedVesselAtLoad", false, "Enable or disable brake landed vessel at load");
    }

    public enum ToggleBrakeEnum
    {
        Modifier,
        Toggle,
        StockBrake
    }
}