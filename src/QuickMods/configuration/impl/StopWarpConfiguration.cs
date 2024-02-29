using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class StopWarpConfiguration(ConfigFile config) : ConfigurationBase("QuickStopWarp")
{
    private ConfigEntry<bool> _vesselSituationChange;
    private ConfigEntry<bool> _dontPauseWhenDecreaseWarp;

    public bool VesselSituationChange() => _vesselSituationChange.Value;
    public bool DontPauseWhenDecreaseWarp() => _dontPauseWhenDecreaseWarp.Value;

    public override void Init()
    {
        base.Init();

        _vesselSituationChange = config.Bind("QuickMods/StopWarp", "VesselSituationChange", false, "Enable or disable the warp stopping when vessel change situation");
        _dontPauseWhenDecreaseWarp = config.Bind("QuickMods/StopWarp", "DontPauseWhenDecreaseWarp", false, "Enable or disable the default pause when decrease warp");
    }
}