using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class PauseConfiguration(ConfigFile config) : ConfigurationBase("QuickPauseControl")
{
    private ConfigEntry<bool> _enable;

    public bool Enabled() => _enable.Value;
    
    public override void Init()
    {
        base.Init();

        _enable = config.Bind("QuickMods/Pause", "Enable", false, "Enable or disable pause when escape");
    }
}