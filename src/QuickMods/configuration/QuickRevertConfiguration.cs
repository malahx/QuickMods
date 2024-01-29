using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class QuickRevertConfiguration : IConfigurationBase
{
    private ConfigEntry<bool> _canLoseRevert;

    public bool CanLoseRevert()
    {
        return _canLoseRevert.Value;
    }

    public void Init(ConfigFile config)
    {
        _canLoseRevert = config.Bind("QuickRevert", "canLoseRevert", false, "Enable or disable the revert lost when reach space");

        Debug.Log($"QuickRevert[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
    }
}