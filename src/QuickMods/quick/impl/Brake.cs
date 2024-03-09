using System.Collections;
using KSP.Messages;
using KSP.OAB;
using KSP.Sim;
using KSP.Sim.impl;
using QuickMods.configuration.impl;
using UnityEngine;

namespace QuickMods.quick.impl;

public class Brake(BrakeConfiguration config) : ModsBase(config)
{
    private bool HasBrakeEnabled(VesselComponent vessel) =>
        vessel.Situation is VesselSituations.PreLaunch or VesselSituations.Landed && !vessel.HasLaunched && config.BrakePreLaunchAtLoad() ||
        vessel.Situation == VesselSituations.Landed && vessel.HasLaunched &&
        (
            config.BrakeLandedVesselAtLoad() ||
            (config.BrakeLandedPlaneAtLoad() && vessel.GetControlOwner().PartData.partType == AssemblyPartTypeFilter.Spaceplane) ||
            (config.BrakeLandedRoverAtLoad() && vessel.GetControlOwner().PartData.partType == AssemblyPartTypeFilter.Rover) ||
            (config.BrakeWhenControlLost() && !vessel.IsControllable)
        );

    private bool HasBrakeEnabled() =>
        config.BrakePreLaunchAtLoad() || config.BrakeLandedVesselAtLoad() || config.BrakeLandedPlaneAtLoad() || config.BrakeLandedRoverAtLoad() || config.BrakeWhenControlLost();

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

    public override void Update()
    {
        if (config.ToggleBrakeKey().IsPressed() && Game.ViewController.TryGetActiveSimVessel(out var vessel))
            vessel.SetActionGroup(KSPActionGroup.Brakes, vessel.GetActionGroupState(KSPActionGroup.Brakes) != KSPActionGroupState.True);
    }

    private void OnFlightViewEnteredMessage(MessageCenterMessage msg)
    {
        // Activate brake at load
        if (HasBrakeEnabled())
            CoroutineUtil.Instance.StartCoroutine(ActivateBrakeWhenVesselIsLoaded());
    }

    private IEnumerator ActivateBrakeWhenVesselIsLoaded()
    {
        VesselComponent vessel;
        var i = 0;

        while (!Game.ViewController.TryGetActiveSimVessel(out vessel))
        {
            i++;
            if (i == 10) yield break;
            yield return new WaitForSecondsRealtime(1);
        }

        if (!HasBrakeEnabled(vessel)) yield break;

        SetBrake(vessel, true);
    }

    private void OnVesselLaunchedMessage(MessageCenterMessage msg)
    {
        if (!config.UnBrakeAtLaunch() || !Game.ViewController.TryGetActiveSimVessel(out var vessel)) return;

        SetBrake(vessel, false);
    }

    private void OnVesselLostControlMessage(MessageCenterMessage msg)
    {
        if (!config.BrakeWhenControlLost() || !Game.ViewController.TryGetActiveSimVessel(out var vessel) || vessel.IsDestroyedOrBeingDestroyed) return;

        SetBrake(vessel, true);
    }

    private void SetBrake(VesselComponent vessel, bool state)
    {
        vessel.SetActionGroup(KSPActionGroup.Brakes, state);
        SendNotification("QuickMods/Brake/Notifications/Primary", state);

        Logger.LogDebug(state ? "Brake" : "UnBrake");
    }
}