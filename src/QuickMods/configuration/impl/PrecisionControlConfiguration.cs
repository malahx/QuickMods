using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class PrecisionControlConfiguration(ConfigFile config) : ConfigurationBase("QuickPrecisionControl")
{
    private ConfigEntry<bool> _enable;
    private ConfigEntry<bool> _keepLastPrecisionControlValue;
    private ConfigEntry<bool> _displayMessageWhenTogglePrecisionControl;

    public bool Enabled() => _enable.Value;
    public bool KeepLastPrecisionControlValue() => _keepLastPrecisionControlValue.Value;
    public bool DisplayMessageWhenTogglePrecisionControl() => _displayMessageWhenTogglePrecisionControl.Value;

    public override void Init()
    {
        base.Init();

        _enable = config.Bind("QuickMods/PrecisionControl", "Enable", false, "Enable or disable by default precision control");

        _keepLastPrecisionControlValue = config.Bind("QuickMods/PrecisionControl", "KeepLastPrecisionControlValue", false, "Always use the last value of precision control when loading a new scene");

        _displayMessageWhenTogglePrecisionControl = config.Bind("QuickMods/PrecisionControl", "DisplayMessageWhenTogglePrecisionControl", false, "Display a message when toggle Precision Control");
    }
}