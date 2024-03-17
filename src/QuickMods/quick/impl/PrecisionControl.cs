using KSP.Game;
using KSP.Messages;
using QuickMods.configuration.impl;
using UnityEngine.InputSystem;

namespace QuickMods.quick.impl;

public class PrecisionControl(PrecisionControlConfiguration config) : ModsBase(config)
{
    private bool _lastValue = true;

    private static bool CurrentPrecisionMode
    {
        get => Game != null && Game.ViewController != null && Game.ViewController.flightInputHandler.IsPrecisionMode;
        set
        {
            if (Game != null && Game.ViewController != null && Game.ViewController.flightInputHandler.IsPrecisionMode != value)
                Game.ViewController.flightInputHandler.TogglePrecisionMode();
        }
    }

    private bool PrecisionControlHasChanged => (!config.KeepLastPrecisionControlValue() || _lastValue) != CurrentPrecisionMode;


    public override void Start()
    {
        base.Start();

        MessageCenter.Subscribe<GameStateChangedMessage>(OnGameStateChangedMessage);
        Game.Input.Flight.TogglePrecisionMode.performed += OnActivatePrecisionMode;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        MessageCenter.Unsubscribe<GameStateChangedMessage>(OnGameStateChangedMessage);
        Game.Input.Flight.TogglePrecisionMode.performed -= OnActivatePrecisionMode;
    }

    private void OnGameStateChangedMessage(MessageCenterMessage msg)
    {
        if (!config.PrecisionControlEnabled() || !PrecisionControlHasChanged || msg is not GameStateChangedMessage { CurrentState: GameState.FlightView }) return;

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