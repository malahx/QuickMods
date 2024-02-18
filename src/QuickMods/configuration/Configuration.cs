using BepInEx.Configuration;

namespace QuickMods.configuration;

public class Configuration
{
    public readonly RevertConfiguration Revert = new();
    public readonly VesselNamesConfiguration VesselNames = new();
    public readonly ScrollConfiguration Scroll = new();
    public readonly StopWarpConfiguration StopWarp = new();

    public void Init(ConfigFile config)
    {
        Revert.Init(config);
        VesselNames.Init(config);
        Scroll.Init(config);
        StopWarp.Init(config);
    }
}