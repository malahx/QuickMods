using BepInEx.Logging;
using I2.Loc;
using KSP.Game;
using KSP.Messages;
using QuickMods.configuration;

namespace QuickMods.quick;

public abstract class ModsBase(ConfigurationBase configuration) : IModsBase
{
    protected static GameInstance Game => GameManager.Instance.Game;
    protected static MessageCenter MessageCenter => Game.Messages;

    public bool Initialized() => configuration.Initialized();
    public bool Enabled() => configuration.Enabled();
    public bool Active() => Enabled() && Initialized();
    public string Name() => configuration.Name();

    public virtual void Start()
    {
        if (!Enabled() || Initialized())
        {
            Logger.LogDebug("Start or restart is forbidden.");
            throw new Exception("Start or restart is forbidden.");
        }
        
        configuration.Init();
        Logger.LogDebug("Start");
    }

    public virtual void OnDestroy()
    {
        configuration.Destroy();
        Logger.LogDebug("Destroy");
    }

    public virtual void Update()
    {
        if (Active()) return;
        
        Logger.LogDebug("Is not active.");
        throw new Exception("Is not active.");
    }

    protected static void SendNotification(string text, bool activated)
    {
        var activatedText = LocalizationManager.GetTranslation(activated ? "QuickMods/Common/Activated" : "QuickMods/Common/Deactivated");
        var notificationData = new NotificationData
        {
            Tier = NotificationTier.Passive,
            Primary = new NotificationLineItemData { LocKey = text, ObjectParams = [activatedText] },
            Importance = NotificationImportance.Low
        };
        Game.Notifications.ProcessNotification(notificationData);
    }

    protected readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(configuration.Name() + $"[{MyPluginInfo.PLUGIN_VERSION}]");
}