using BepInEx.Logging;

namespace QuickMods.quick;

public abstract class ModsBase(string name)
{
    public abstract void Start();

    public abstract void OnDestroy();

    protected readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(name + $"[{MyPluginInfo.PLUGIN_VERSION}]");
}