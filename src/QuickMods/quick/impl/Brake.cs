using System.Collections;
using I2.Loc;
using KSP.Game;
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
        Game.Input.m_Flight_WheelBrakes.started -= Game.ViewController.flightInputHandler._inputDefinition.OnWheelBrakes;
        Game.Input.m_Flight_WheelBrakes.performed -= Game.ViewController.flightInputHandler._inputDefinition.OnWheelBrakes;
        Game.Input.m_Flight_WheelBrakes.canceled -= Game.ViewController.flightInputHandler._inputDefinition.OnWheelBrakes;
        Game.Input.m_Flight_WheelBrakes.performed += OnBrakes;

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
                if (context.performed && Game.Input.m_Flight_TrimModifier.IsPressed()) vessel.SetActionGroup(KSPActionGroup.Brakes, vessel.GetActionGroupState(KSPActionGroup.Brakes) != KSPActionGroupState.True);
                if (!Game.Input.m_Flight_TrimModifier.IsPressed()) vessel.SetActionGroup(KSPActionGroup.Brakes, context.performed);
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
            Logger.LogDebug(i);
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
        SendNotification(state);
        Logger.LogDebug(state ? "Brake" : "UnBrake");
    }

    private void SendNotification(bool brake)
    {
        var isBrakeText = LocalizationManager.GetTranslation(brake ? "QuickMods/Common/Activated" : "QuickMods/Common/Deactivated");
        var notificationData = new NotificationData
        {
            Tier = NotificationTier.Passive,
            Primary = new NotificationLineItemData { LocKey = "QuickMods/Brake/Notifications/Primary", ObjectParams = [isBrakeText] },
            Importance = NotificationImportance.Low
        };
        Game.Notifications.ProcessNotification(notificationData);
    }
}