using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class StopWarpConfiguration : ConfigurationBase
{
    private ConfigEntry<bool> _vesselSituationChange;

    public bool VesselSituationChange() => _vesselSituationChange.Value;

    public new void Init(ConfigFile config)
    {
        base.Init(config);

        _vesselSituationChange = config.Bind("QuickMods/StopWarp", "VesselSituationChange", false, "Enable or disable the warp stopping when vessel change situation");

        Debug.Log($"{GetType()}[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
    }
}