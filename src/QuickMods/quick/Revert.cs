using KSP.Game;
using KSP.Messages;
using QuickMods.configuration;
using QuickMods.quick.model;
using static KSP.Sim.impl.VesselSituations;

namespace QuickMods.quick;

public class Revert(string name, Configuration configuration) : ModsBase(name)
{
    private readonly QuickRevertConfiguration _configuration = configuration.QuickRevert;
    private readonly RevertLogics _revertLogics = new();

    public override void Start()
    {
        var messageCenter = GameManager.Instance.Game.Messages;
        messageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
        messageCenter.Subscribe<LaunchFromVABMessage>(OnLaunchFromVABMessage);
        messageCenter.Subscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
    }

    public override void OnDestroy()
    {
        var messageCenter = GameManager.Instance.Game.Messages;
        messageCenter.Unsubscribe<VesselSituationChangedMessage>(OnVesselSituationChange);
        messageCenter.Unsubscribe<LaunchFromVABMessage>(OnLaunchFromVABMessage);
        messageCenter.Unsubscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
    }

    private void OnVesselSituationChange(MessageCenterMessage msg)
    {
        if (!_configuration.CanLoseRevert() || !_revertLogics.CanRevert() ||
            msg is not (VesselSituationChangedMessage { OldSituation: Flying, NewSituation: SubOrbital }
                or VesselSituationChangedMessage { NewSituation: Escaping }))
            return;

        _revertLogics.Reset();

        Logger.LogDebug("Lost.");
    }

    private void OnLaunchFromVABMessage(MessageCenterMessage msg)
    {
        if (!_configuration.KeepRevert())
            return;

        _revertLogics.Save();

        Logger.LogDebug("Saved.");
    }

    private void OnFlightViewEnteredMessage(MessageCenterMessage msg)
    {
        if (!_configuration.KeepRevert() || !_revertLogics.RevertIsEmpty() || !_revertLogics.RevertSaved())
            return;

        _revertLogics.Restore();

        Logger.LogDebug("Restored.");
    }
}