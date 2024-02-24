using BepInEx.Configuration;

namespace QuickMods.configuration;

public class StopWarpConfiguration(ConfigFile config) : ConfigurationBase("QuickStopWarp")
{
    private ConfigEntry<bool> _vesselSituationChange;

    public bool VesselSituationChange() => _vesselSituationChange.Value;

    public override void Init()
    {
        base.Init();

        _vesselSituationChange = config.Bind("QuickMods/StopWarp", "VesselSituationChange", false, "Enable or disable the warp stopping when vessel change situation");
    }
}