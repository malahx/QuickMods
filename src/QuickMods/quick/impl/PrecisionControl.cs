using KSP.Game;
using KSP.Messages;
using QuickMods.configuration.impl;
using UnityEngine.InputSystem;

namespace QuickMods.quick.impl;

public class PrecisionControl(PrecisionControlConfiguration config) : ModsBase(config)
{
    private bool _lastValue = true;

    private bool CurrentPrecisionMode
    {
        get => Game.ViewController.flightInputHandler.IsPrecisionMode;
        set
        {
            if (Game.ViewController.flightInputHandler.IsPrecisionMode != value)
                Game.ViewController.flightInputHandler.TogglePrecisionMode();
        }
    }

    private bool PrecisionControlHasChanged => (!config.KeepLastPrecisionControlValue() || _lastValue) != CurrentPrecisionMode;


    public override void Start()
    {
        base.Start();

        MessageCenter.Subscribe<GameStateChangedMessage>(OnGameStateLeftMessage);
        Game.Input.Flight.TogglePrecisionMode.performed += OnActivatePrecisionMode;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        MessageCenter.Unsubscribe<GameStateChangedMessage>(OnGameStateLeftMessage);
        Game.Input.Flight.TogglePrecisionMode.performed -= OnActivatePrecisionMode;
    }

    private void OnGameStateLeftMessage(MessageCenterMessage msg)
    {
        if (!config.Enabled() || !PrecisionControlHasChanged || msg is not GameStateChangedMessage { CurrentState: GameState.FlightView }) return;

        CurrentPrecisionMode = !config.KeepLastPrecisionControlValue() || _lastValue;
        _lastValue = CurrentPrecisionMode;

        if (CurrentPrecisionMode && config.DisplayMessageWhenTogglePrecisionControl())
            SendNotification("QuickMods/PrecisionControl/Notifications/Primary", CurrentPrecisionMode);

        Logger.LogDebug($"Set PrecisionControl to {CurrentPrecisionMode}");
    }

    private void OnActivatePrecisionMode(InputAction.CallbackContext context)
    {
        _lastValue = !config.KeepLastPrecisionControlValue() || CurrentPrecisionMode;

        if (config.DisplayMessageWhenTogglePrecisionControl())
            SendNotification("QuickMods/PrecisionControl/Notifications/Primary", CurrentPrecisionMode);
    }
}