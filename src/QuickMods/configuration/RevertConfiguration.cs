using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class RevertConfiguration : ConfigurationBase
{
    private ConfigEntry<bool> _canLoseRevert;

    public bool CanLoseRevert() => _canLoseRevert.Value;

    public new void Init(ConfigFile config)
    {
        base.Init(config);
        
        _canLoseRevert = config.Bind("QuickMods/Revert", "CanLoseRevert", false, "Enable or disable the revert lost when reach space");

        Debug.Log($"{GetType()}[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
    }
}