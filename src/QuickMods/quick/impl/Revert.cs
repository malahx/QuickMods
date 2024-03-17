using KSP.Messages;
using QuickMods.configuration.impl;
using static KSP.Sim.impl.VesselSituations;

namespace QuickMods.quick.impl;

public class Revert(RevertConfiguration config) : ModsBase(config)
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
        if (!config.CanLoseRevert() || !CanRevert() ||
            msg is not (VesselSituationChangedMessage { OldSituation: Flying, NewSituation: SubOrbital }
                or VesselSituationChangedMessage { NewSituation: Escaping }))
            return;

        Game.stateRevTracker._saveVABGameData = null;
        Game.stateRevTracker._saveGameData = null;
        Game.stateRevTracker._lastLaunchedAssemblyName = null;
        Game.stateRevTracker._lastVesselCreatedName = null;

        SendNotification("QuickMods/Revert/Notifications/LostRevert/Primary", true);

        Logger.LogDebug("Lost.");
    }

    private static bool CanRevert()
    {
        var gameStateRevTracker = Game.stateRevTracker;
        return gameStateRevTracker.HasValidReversionState && gameStateRevTracker.IsLastLaunchedVesselTheActiveVessel() ||
               gameStateRevTracker.HasValidVABReversionState && gameStateRevTracker.IsLastLaunchedAssemblyTheActiveVessel();
    }
}