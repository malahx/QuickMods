using BepInEx.Configuration;

namespace QuickMods.configuration;

public class Configuration
{
    public readonly QuickRevertConfiguration QuickRevert = new();
    public readonly QuickVesselNamesConfiguration QuickVesselNames = new();

    public void Init(ConfigFile config)
    {
        QuickRevert.Init(config);
        QuickVesselNames.Init(config);
    }
}