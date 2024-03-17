using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class PrecisionControlConfiguration(ConfigFile config) : ConfigurationBase("QuickPrecisionControl", config)
{
    private ConfigEntry<bool> _enablePrecisionControl;
    private ConfigEntry<bool> _keepLastPrecisionControlValue;
    private ConfigEntry<bool> _displayMessageWhenTogglePrecisionControl;

    public bool PrecisionControlEnabled() => _enablePrecisionControl.Value;
    public bool KeepLastPrecisionControlValue() => _keepLastPrecisionControlValue.Value;
    public bool DisplayMessageWhenTogglePrecisionControl() => _displayMessageWhenTogglePrecisionControl.Value;

    public override void Init()
    {
        base.Init();
        
        _enablePrecisionControl = Config.Bind("QuickMods/PrecisionControl", "Enable", false, "Enable or disable by default precision control");
        _keepLastPrecisionControlValue = Config.Bind("QuickMods/PrecisionControl", "KeepLastPrecisionControlValue", false, "Always use the last value of precision control when loading a new scene");
        _displayMessageWhenTogglePrecisionControl = Config.Bind("QuickMods/PrecisionControl", "DisplayMessageWhenTogglePrecisionControl", false, "Display a message when toggle Precision Control");
    }
}