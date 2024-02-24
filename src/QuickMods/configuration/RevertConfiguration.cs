using BepInEx.Configuration;

namespace QuickMods.configuration;

public class RevertConfiguration(ConfigFile config) : ConfigurationBase("QuickRevert")
{
    private ConfigEntry<bool> _canLoseRevert;

    public bool CanLoseRevert() => _canLoseRevert.Value;

    public override void Init()
    {
        base.Init();

        _canLoseRevert = config.Bind("QuickMods/Revert", "CanLoseRevert", false, "Enable or disable the revert lost when reach space");
    }
}