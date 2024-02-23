using BepInEx.Configuration;

namespace QuickMods.configuration;

public abstract class ConfigurationBase
{
    private bool _initialized;

    public bool Initialized() => _initialized;

    protected void Init(ConfigFile config)
    {
        _initialized = true;
    }
}