using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class PrecisionControlConfiguration(ConfigFile config) : ConfigurationBase("QuickPrecisionControl")
{
    private ConfigEntry<bool> _enable;

    public bool Enabled() => _enable.Value;
    
    public override void Init()
    {
        base.Init();

        _enable = config.Bind("QuickMods/PrecisionControl", "Enable", false, "Enable or disable by default precision control");
    }
}