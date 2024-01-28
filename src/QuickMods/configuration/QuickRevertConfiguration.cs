using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class QuickRevertConfiguration : IConfigurationBase
{
    private ConfigEntry<bool> _keepRevert;
    private ConfigEntry<bool> _canLoseRevert;

    public bool KeepRevert()
    {
        return _keepRevert.Value;
    }

    public bool CanLoseRevert()
    {
        return _canLoseRevert.Value;
    }

    public void Init(ConfigFile config)
    {
        _keepRevert = config.Bind("QuickRevert", "keepRevert", true, "Enable or disable revert keeping");
        _canLoseRevert = config.Bind("QuickRevert", "canLoseRevert", false, "Enable or disable the revert lost when reach space");

        Debug.Log($"QuickRevert[{MyPluginInfo.PLUGIN_VERSION}] QuickRevert configuration initialized.");
    }
}