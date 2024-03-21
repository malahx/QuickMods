using KSP.Messages;
using KSP.Sim.impl;
using QuickMods.configuration.impl;

namespace QuickMods.quick.impl;

public class Warp(WarpConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();

        MessageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        MessageCenter.Unsubscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    private void OnVesselSituationChange(MessageCenterMessage msg)
    {
        if (!config.VesselSituationChange() || msg is not VesselSituationChangedMessage message || message.NewSituation == VesselSituations.Landed || message.OldSituation == VesselSituations.Landed || !Game.ViewController.TimeWarp.IsWarping) return;

        Game.ViewController.TimeWarp.StopTimeWarp(true);

        SendNotification("QuickMods/Warp/Notifications/VesselSituationChange/Primary", true);

        Logger.LogDebug($"Stop wrap, VesselName: {message.Vessel.Name}, isActiveVessel: {Game.ViewController.IsActiveVessel(message.Vessel)}, Old Situation: {message.OldSituation}, New Situation; {message.NewSituation}");
    }
}