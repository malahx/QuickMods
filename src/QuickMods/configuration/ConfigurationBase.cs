using BepInEx.Logging;

namespace QuickMods.configuration;

public abstract class ConfigurationBase(string name)
{
    private bool _initialized;

    public bool Initialized() => _initialized;
    public string Name() => name;

    public virtual void Init()
    {
        _initialized = true;
        Logger.LogDebug("Configuration initialized");
    }

    protected readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(name + $"[{MyPluginInfo.PLUGIN_VERSION}]");
}