using I2.Loc;
using KSP.Game;
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
        SendNotification();

        Logger.LogDebug("Set PrecisionControl to true");
    }

    private void OnActivatePrecisionMode(InputAction.CallbackContext context)
    {
        SendNotification();
    }

    private void SendNotification()
    {
        var isPrecisionMode = Game.ViewController.flightInputHandler.IsPrecisionMode;
        var isPrecisionModeText = LocalizationManager.GetTranslation(isPrecisionMode ? "QuickMods/PrecisionControl/Notifications/Primary/Activated" : "QuickMods/PrecisionControl/Notifications/Primary/Deactivated");
        var notificationData = new NotificationData
        {
            Tier = NotificationTier.Passive,
            Primary = new NotificationLineItemData { LocKey = "QuickMods/PrecisionControl/Notifications/Primary", ObjectParams = [isPrecisionModeText] },
            Importance = NotificationImportance.Low
        };
        Game.Notifications.ProcessNotification(notificationData);
    }
}