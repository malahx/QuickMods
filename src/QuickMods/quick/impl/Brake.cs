using System.Collections;
using KSP.Game;
using KSP.Input;
using KSP.Messages;
using KSP.OAB;
using KSP.Sim;
using KSP.Sim.impl;
using QuickMods.configuration.impl;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void OnFlightViewEnteredMessage(MessageCenterMessage msg)
    {
        // Override default brakes
        if (Game.InputManager.TryGetInputDefinition<FlightInputDefinition>(out var definition))
        {
            Game.Input.Flight.WheelBrakes.started -= definition.OnWheelBrakes;
            Game.Input.Flight.WheelBrakes.performed -= definition.OnWheelBrakes;
            Game.Input.Flight.WheelBrakes.canceled -= definition.OnWheelBrakes;
            Game.Input.Flight.WheelBrakes.started += OnBrakes;
            Game.Input.Flight.WheelBrakes.performed += OnBrakes;
            Game.Input.Flight.WheelBrakes.canceled += OnBrakes;
        }
        else
            Logger.LogWarning("Can't override wheel brakes button.");

        // Activate brake at load
        if (HasBrakeEnabled())
            CoroutineUtil.Instance.StartCoroutine(ActivateBrakeWhenVesselIsLoaded());
    }

    private void OnBrakes(InputAction.CallbackContext context)
    {
        if (Game.GlobalGameState.GetState() != GameState.FlightView || !Game.ViewController.TryGetActiveSimVessel(out var vessel)) return;

        switch (config.ToggleBrake())
        {
            // Toggle brake with modifier
            case BrakeConfiguration.ToggleBrakeEnum.Modifier:
                if (context.performed && Input.GetKey(config.ModifierBrakeKey())) vessel.SetActionGroup(KSPActionGroup.Brakes, vessel.GetActionGroupState(KSPActionGroup.Brakes) != KSPActionGroupState.True);
                if (!Game.Input.Flight.TrimModifier.IsPressed()) vessel.SetActionGroup(KSPActionGroup.Brakes, context.performed);
                break;

            // Toggle brake without modifier
            case BrakeConfiguration.ToggleBrakeEnum.Toggle:
                if (context.performed)
                    vessel.SetActionGroup(KSPActionGroup.Brakes, vessel.GetActionGroupState(KSPActionGroup.Brakes) != KSPActionGroupState.True);
                break;

            // Stock brake
            case BrakeConfiguration.ToggleBrakeEnum.StockBrake:
                vessel.SetActionGroup(KSPActionGroup.Brakes, context.performed);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Logger.LogDebug($"OnBrakes: {vessel.GetActionGroupState(KSPActionGroup.Brakes)}");
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