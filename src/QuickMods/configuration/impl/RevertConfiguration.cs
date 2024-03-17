using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class RevertConfiguration(ConfigFile config) : ConfigurationBase("QuickRevert", config)
{
    private ConfigEntry<bool> _canLoseRevert;

    public bool CanLoseRevert() => _canLoseRevert.Value;

    public override void Init()
    {
        base.Init();

        _canLoseRevert = Config.Bind("QuickMods/Revert", "CanLoseRevert", false, "Enable or disable the revert lost when reach space");
    }
}