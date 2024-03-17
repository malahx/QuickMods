using KSP.Input;
using KSP.Messages;
using QuickMods.configuration.impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QuickMods.quick.impl;

public class Pause(PauseConfiguration config) : ModsBase(config)
{
    private bool? _lastPauseValue;

    public override void Start()
    {
        base.Start();

        MessageCenter.Subscribe<EscapeMenuOpenedMessage>(OnEscapeMenuOpenedMessage);
        MessageCenter.Subscribe<EscapeMenuClosedMessage>(OnEscapeMenuClosedMessage);
        MessageCenter.Subscribe<OABLoadedMessage>(OnOABLoadedMessage);
        MessageCenter.Subscribe<OABUnloadedMessage>(OnOABUnloadedMessage);

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

        MessageCenter.Unsubscribe<EscapeMenuOpenedMessage>(OnEscapeMenuOpenedMessage);
        MessageCenter.Unsubscribe<EscapeMenuClosedMessage>(OnEscapeMenuClosedMessage);
        MessageCenter.Unsubscribe<OABLoadedMessage>(OnOABLoadedMessage);
        MessageCenter.Unsubscribe<OABUnloadedMessage>(OnOABUnloadedMessage);

        // Reset default TimeWarp key
        if (Game.InputManager.TryGetInputDefinition<GlobalInputDefinition>(out var definition))
        {
            Game.Input.Global.TimeWarpDecrease.started += definition.OnTimeWarpDecrease;
            Game.Input.Global.TimeWarpDecrease.performed += definition.OnTimeWarpDecrease;
            Game.Input.Global.TimeWarpDecrease.canceled += definition.OnTimeWarpDecrease;
            Game.Input.Global.TimeWarpDecrease.performed -= OnTimeWarpDecrease;
        }
    }

    public override void Update()
    {
        if (Input.GetKeyDown(config.PauseKey()))
            ForcePause(!Game.ViewController.IsPaused);
    }

    private void OnEscapeMenuOpenedMessage(MessageCenterMessage msg)
    {
        if (!config.EscapePauseEnabled()) return;

        _lastPauseValue = Game.ViewController.IsPaused;
        ForcePause(true);
    }

    private void OnEscapeMenuClosedMessage(MessageCenterMessage msg)
    {
        if (!config.EscapePauseEnabled()) return;

        ForcePause(_lastPauseValue);
        _lastPauseValue = null;
    }

    private void OnOABLoadedMessage(MessageCenterMessage msg)
    {
        if (config.EditorPauseEnabled() && !Game.ViewController.IsPaused) ForcePause(true);
    }

    private void OnOABUnloadedMessage(MessageCenterMessage msg)
    {
        if (config.EditorPauseEnabled() && Game.ViewController.IsPaused) ForcePause(false);
    }

    private void ForcePause(bool? pause)
    {
        if (!pause.HasValue) return;

        var newPauseState = pause.GetValueOrDefault(false);
        Game.ViewController.SetPause(newPauseState);

        Logger.LogDebug($"Set pause to {newPauseState}");
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