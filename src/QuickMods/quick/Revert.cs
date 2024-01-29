using KSP.Game;
using KSP.Messages;
using QuickMods.configuration;
using static KSP.Sim.impl.VesselSituations;

namespace QuickMods.quick;

public class Revert(string name, Configuration configuration) : ModsBase(name)
{
    private readonly QuickRevertConfiguration _configuration = configuration.QuickRevert;

    public override void Start()
    {
        var messageCenter = GameManager.Instance.Game.Messages;
        messageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    public override void OnDestroy()
    {
        var messageCenter = GameManager.Instance.Game.Messages;
        messageCenter.Unsubscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
    }

    private void OnVesselSituationChange(MessageCenterMessage msg)
    {
        if (!_configuration.CanLoseRevert() || !CanRevert() ||
            msg is not (VesselSituationChangedMessage { OldSituation: Flying, NewSituation: SubOrbital }
                or VesselSituationChangedMessage { NewSituation: Escaping }))
            return;

        var gameStateRevTracker = GameManager.Instance.Game.stateRevTracker;
        gameStateRevTracker._saveVABGameData = null;
        gameStateRevTracker._saveGameData = null;
        gameStateRevTracker._lastLaunchedAssemblyName = null;
        gameStateRevTracker._lastVesselCreatedName = null;

        Logger.LogDebug("Lost.");
    }

    private bool CanRevert()
    {
        var gameStateRevTracker = GameManager.Instance.Game.stateRevTracker;
        return gameStateRevTracker.HasValidReversionState && gameStateRevTracker.IsLastLaunchedVesselTheActiveVessel() ||
               gameStateRevTracker.HasValidVABReversionState && gameStateRevTracker.IsLastLaunchedAssemblyTheActiveVessel();
    }
}