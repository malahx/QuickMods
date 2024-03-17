using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class StopWarpConfiguration(ConfigFile config) : ConfigurationBase("QuickStopWarp", config)
{
    private ConfigEntry<bool> _vesselSituationChange;

    public bool VesselSituationChange() => _vesselSituationChange.Value;

    public override void Init()
    {
        base.Init();

        _vesselSituationChange = Config.Bind("QuickMods/StopWarp", "VesselSituationChange", false, "Enable or disable the warp stopping when vessel change situation");
    }
}