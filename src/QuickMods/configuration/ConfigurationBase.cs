using BepInEx.Configuration;
using BepInEx.Logging;

namespace QuickMods.configuration;

public abstract class ConfigurationBase(string name, ConfigFile config)
{
    private bool _initialized;
    protected readonly ConfigFile Config = config;

    private readonly ConfigEntry<bool> _enabled = config.Bind(
        "QuickMods",
        name,
        true,
        $"Enable or disable {name}\n\nATTENTION: \n* Disabling this option and close the settings will unload the linked quickmods.\n* If you want to reenable it, you need to activate it and restart KSP.");

    public bool Initialized() => _initialized;
    public bool Enabled() => _enabled.Value;
    public string Name() => name;

    public virtual void Init()
    {
        if (_initialized)
        {
            Logger.LogDebug("Configuration already initialized.");
            throw new Exception("Configuration already initialized.");
        }

        _initialized = true;
        Logger.LogDebug("Configuration initialized.");
    }

    protected readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(name + $"Configuration[{MyPluginInfo.PLUGIN_VERSION}]");

    public virtual void Destroy()
    {
        _initialized = false;
        Logger.LogDebug("Configuration destroyed.");
    }
}