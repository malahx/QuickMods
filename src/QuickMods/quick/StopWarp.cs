using KSP.Game;
using KSP.Messages;
using QuickMods.configuration;

namespace QuickMods.quick;

public class StopWarp(StopWarpConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();
        MessageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MessageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    private void OnVesselSituationChange(MessageCenterMessage msg)
    {
        if (!config.VesselSituationChange() || msg is not VesselSituationChangedMessage message || !Game.ViewController.TimeWarp.IsWarping) return;

        Game.ViewController.TimeWarp.StopTimeWarp(true);

        var notificationData = new NotificationData
        {
            Tier = NotificationTier.Passive,
            Primary = new NotificationLineItemData { LocKey = "QuickMods/StopWarp/Notifications/VesselSituationChange/Primary" },
            FirstLine = new NotificationLineItemData { LocKey = "QuickMods/StopWarp/Notifications/VesselSituationChange/FirstLine", ObjectParams = [message.OldSituation, message.NewSituation] },
            Importance = NotificationImportance.Low
        };
        Game.Notifications.ProcessNotification(notificationData);

        Logger.LogDebug($"Stop wrap, VesselName: {message.Vessel.Name}, isActiveVessel: {Game.ViewController.IsActiveVessel(message.Vessel)}, Old Situation: {message.OldSituation}, New Situation; {message.NewSituation}");
    }
}