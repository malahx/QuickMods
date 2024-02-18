using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class RevertConfiguration : IConfigurationBase
{
    private ConfigEntry<bool> _canLoseRevert;

    public bool CanLoseRevert()
    {
        return _canLoseRevert.Value;
    }

    public void Init(ConfigFile config)
    {
        _canLoseRevert = config.Bind("QuickMods/Revert", "CanLoseRevert", false, "Enable or disable the revert lost when reach space");

        Debug.Log($"{GetType()}[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
    }
}