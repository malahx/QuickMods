using KSP.Messages;
using QuickMods.configuration.impl;
using UnityEngine.InputSystem;

namespace QuickMods.quick.impl;

public class PrecisionControl(PrecisionControlConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();
        MessageCenter.Subscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
        Game.Input.Flight.TogglePrecisionMode.performed += OnActivatePrecisionMode;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MessageCenter.Unsubscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
        Game.Input.Flight.TogglePrecisionMode.performed -= OnActivatePrecisionMode;
    }

    private void OnFlightViewEnteredMessage(MessageCenterMessage msg)
    {
        if (!config.Enabled()) return;

        Game.ViewController.flightInputHandler._isPrecisionMode = true;
        SendNotification("QuickMods/PrecisionControl/Notifications/Primary", Game.ViewController.flightInputHandler.IsPrecisionMode);

        Logger.LogDebug("Set PrecisionControl to true");
    }

    private void OnActivatePrecisionMode(InputAction.CallbackContext context)
    {
        SendNotification("QuickMods/PrecisionControl/Notifications/Primary", Game.ViewController.flightInputHandler.IsPrecisionMode);
    }
}