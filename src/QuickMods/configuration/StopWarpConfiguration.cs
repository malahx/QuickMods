using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class StopWarpConfiguration : IConfigurationBase
{
    private ConfigEntry<bool> _vesselSituationChange;

    public bool VesselSituationChange()
    {
        return _vesselSituationChange.Value;
    }

    public void Init(ConfigFile config)
    {
        _vesselSituationChange = config.Bind("QuickMods/StopWarp", "VesselSituationChange", false, "Enable or disable the warp stopping when vessel change situation");

        Debug.Log($"{GetType()}[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
    }
}