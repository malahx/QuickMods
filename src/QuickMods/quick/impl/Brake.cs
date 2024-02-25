using KSP.Messages;
using KSP.OAB;
using KSP.Sim;
using KSP.Sim.impl;
using QuickMods.configuration.impl;

namespace QuickMods.quick.impl;

public class Brake(BrakeConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();
        MessageCenter.Subscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
        MessageCenter.Subscribe<VesselLaunchedMessage>(OnVesselLaunchedMessage);
        MessageCenter.Subscribe<VesselLostControlMessage>(OnVesselLostControlMessage);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MessageCenter.Unsubscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
        MessageCenter.Unsubscribe<VesselLaunchedMessage>(OnVesselLaunchedMessage);
        MessageCenter.Unsubscribe<VesselLostControlMessage>(OnVesselLostControlMessage);
    }

    private void OnFlightViewEnteredMessage(MessageCenterMessage msg)
    {
        if (Game.ViewController.TryGetActiveSimVessel(out var activeSimVessel) || !IsEnableBrakeAtLoad(activeSimVessel)) return;

        activeSimVessel.SetActionGroup(KSPActionGroup.Brakes, true);
        Logger.LogDebug("Brake");
    }

    private void OnVesselLaunchedMessage(MessageCenterMessage msg)
    {
        if (msg is not VesselLaunchedMessage message || message.Vessel.GetActionGroupState(KSPActionGroup.Brakes) != KSPActionGroupState.False || !config.EnableUnBrakeAtLaunch()) return;

        message.Vessel.SetActionGroup(KSPActionGroup.Brakes, true);
        Logger.LogDebug("UnBrake");
    }

    private void OnVesselLostControlMessage(MessageCenterMessage msg)
    {
        if (msg is not VesselLostControlMessage message || message.Vessel.GetActionGroupState(KSPActionGroup.Brakes) != KSPActionGroupState.False || !config.EnableBrakeWhenControlLost()) return;
        
        message.Vessel.SetActionGroup(KSPActionGroup.Brakes, true);
        Logger.LogDebug("Brake");
    }

    private bool IsEnableBrakeAtLoad(VesselComponent activeSimVessel)
    {
        return (activeSimVessel.Situation == VesselSituations.PreLaunch &&
                (
                    config.EnableBrakeAtLaunchPad() && activeSimVessel.landedAt == "LaunchPad" ||
                    config.EnableBrakeAtRunway() && activeSimVessel.landedAt == "Runway")
               ) ||
               activeSimVessel.Situation == VesselSituations.Landed &&
               (
                   config.AlwaysBrakeLandedVessel() ||
                   (config.AlwaysBrakeLandedPlane() && activeSimVessel.GetControlOwner().PartData.partType == AssemblyPartTypeFilter.Spaceplane) ||
                   (config.AlwaysBrakeLandedRover() && activeSimVessel.GetControlOwner().PartData.partType == AssemblyPartTypeFilter.Rover)
               ) ||
               (config.EnableBrakeWhenControlLost() && !activeSimVessel.IsControllable);
    }
}