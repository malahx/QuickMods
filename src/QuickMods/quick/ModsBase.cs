using BepInEx.Logging;
using I2.Loc;
using KSP.Game;
using KSP.Messages;
using QuickMods.configuration;

namespace QuickMods.quick;

public abstract class ModsBase(ConfigurationBase configuration) : IModsBase
{
    protected GameInstance Game => GameManager.Instance.Game;
    protected MessageCenter MessageCenter => Game.Messages;

    public bool Initialized() => configuration.Initialized();

    public virtual void Start()
    {
        configuration.Init();
        Logger.LogDebug("Start");
    }

    public virtual void OnDestroy()
    {
        Logger.LogDebug("Destroy");
    }

    public virtual void Update()
    {
    }

    protected void SendNotification(string text, bool activated)
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