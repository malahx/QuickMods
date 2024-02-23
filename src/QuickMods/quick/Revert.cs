using KSP.Messages;
using QuickMods.configuration;
using static KSP.Sim.impl.VesselSituations;

namespace QuickMods.quick;

public class Revert(string name, RevertConfiguration configuration) : ModsBase(name, configuration)
{
    public override void Start()
    {
        MessageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    public override void OnDestroy()
    {
        MessageCenter.Unsubscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    private void OnVesselSituationChange(MessageCenterMessage msg)
    {
        if (!configuration.CanLoseRevert() || !CanRevert() ||
            msg is not (VesselSituationChangedMessage { OldSituation: Flying, NewSituation: SubOrbital }
                or VesselSituationChangedMessage { NewSituation: Escaping }))
            return;

        Game.stateRevTracker._saveVABGameData = null;
        Game.stateRevTracker._saveGameData = null;
        Game.stateRevTracker._lastLaunchedAssemblyName = null;
        Game.stateRevTracker._lastVesselCreatedName = null;

        Logger.LogDebug("Lost.");
    }

    private bool CanRevert()
    {
        var gameStateRevTracker = Game.stateRevTracker;
        return gameStateRevTracker.HasValidReversionState && gameStateRevTracker.IsLastLaunchedVesselTheActiveVessel() ||
               gameStateRevTracker.HasValidVABReversionState && gameStateRevTracker.IsLastLaunchedAssemblyTheActiveVessel();
    }
}