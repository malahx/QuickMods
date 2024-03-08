using KSP.Game;
using KSP.Input;
using KSP.Messages;
using KSP.Sim.impl;
using QuickMods.configuration.impl;
using UnityEngine.InputSystem;

namespace QuickMods.quick.impl;

public class StopWarp(StopWarpConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();
        MessageCenter.Subscribe<VesselSituationChangedMessage>(OnVesselSituationChange);

        // Reset default TimeWarp key
        if (Game.InputManager.TryGetInputDefinition<GlobalInputDefinition>(out var definition))
        {
            Game.Input.Global.TimeWarpDecrease.started -= definition.OnTimeWarpDecrease;
            Game.Input.Global.TimeWarpDecrease.performed -= definition.OnTimeWarpDecrease;
            Game.Input.Global.TimeWarpDecrease.canceled -= definition.OnTimeWarpDecrease;
            Game.Input.Global.TimeWarpDecrease.performed += OnTimeWarpDecrease;
        }
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

        var notificationData = new NotificationData
        {
            Tier = NotificationTier.Passive,
            Primary = new NotificationLineItemData { LocKey = "QuickMods/StopWarp/Notifications/VesselSituationChange/Primary" },
            Importance = NotificationImportance.Low
        };
        Game.Notifications.ProcessNotification(notificationData);

        Logger.LogDebug($"Stop wrap, VesselName: {message.Vessel.Name}, isActiveVessel: {Game.ViewController.IsActiveVessel(message.Vessel)}, Old Situation: {message.OldSituation}, New Situation; {message.NewSituation}");
    }

    private void OnTimeWarpDecrease(InputAction.CallbackContext context)
    {
        if (!config.DontPauseWhenDecreaseWarp() && Game.InputManager.TryGetInputDefinition<GlobalInputDefinition>(out var definition))
            definition.OnTimeWarpDecrease(context);

        var num = Game.ViewController.TimeWarp.SetRateIndex(Game.ViewController.TimeWarp.CurrentRateIndex - 1, false) ? 1 : 0;
        if (num != 0)
            Game.ViewController.TimeWarp.CancelAutoWarp();
        Logger.LogDebug("OnTimeWarpDecrease");
    }
}