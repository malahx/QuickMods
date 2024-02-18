using KSP.Messages;
using QuickMods.configuration;

namespace QuickMods.quick;

public class StopWarp(string name, StopWarpConfiguration configuration) : ModsBase(name)
{
    public override void Start()
    {
        MessageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    public override void OnDestroy()
    {
        MessageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    private void OnVesselSituationChange(MessageCenterMessage msg)
    {
        if (!configuration.VesselSituationChange() || msg is not VesselSituationChangedMessage message) return;

        Game.ViewController.TimeWarp.StopTimeWarp(true);
        Logger.LogDebug($"Stop wrap, VesselName: {message.Vessel.Name}, isActiveVessel: {Game.ViewController.IsActiveVessel(message.Vessel)}, Old Situation: {message.OldSituation}, New Situation; {message.NewSituation}");
    }
}