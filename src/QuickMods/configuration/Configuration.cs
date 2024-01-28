using BepInEx.Configuration;
using SpaceWarp.API.Mods;

namespace QuickMods.configuration;

public class Configuration
{
    
    public readonly QuickRevertConfiguration QuickRevert = new();
    
    public void Init(ConfigFile config)
    {
        QuickRevert.Init(config);
    }
}