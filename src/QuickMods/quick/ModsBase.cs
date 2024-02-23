using BepInEx.Logging;
using KSP.Game;
using KSP.Messages;
using QuickMods.configuration;

namespace QuickMods.quick;

public abstract class ModsBase(string name, ConfigurationBase configuration)
{
    protected GameInstance Game => GameManager.Instance.Game;
    protected MessageCenter MessageCenter => Game.Messages;

    public bool Initialized() => configuration.Initialized();

    public virtual void Start() {}

    public virtual void OnDestroy() {}
    
    public virtual void Update() {}

    protected readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(name + $"[{MyPluginInfo.PLUGIN_VERSION}]");
}