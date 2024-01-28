using BepInEx.Configuration;

namespace QuickMods.configuration;

public interface IConfigurationBase
{
    void Init(ConfigFile config);
}