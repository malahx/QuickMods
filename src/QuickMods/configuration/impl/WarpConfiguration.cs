using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class WarpConfiguration(ConfigFile config) : ConfigurationBase("QuickStopWarp")
{
    private ConfigEntry<bool> _vesselSituationChange;

    public bool VesselSituationChange() => _vesselSituationChange.Value;

    public override void Init()
    {
        base.Init();

        _vesselSituationChange = config.Bind("QuickMods/Warp", "StopWarpWhenVesselSituationChange", false, "Enable or disable the warp stopping when vessel change situation");
    }
}