using KSP.Messages;
using QuickMods.configuration.impl;

namespace QuickMods.quick.impl;

public class PrecisionControl(PrecisionControlConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();
        MessageCenter.Subscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MessageCenter.Unsubscribe<FlightViewEnteredMessage>(OnFlightViewEnteredMessage);
    }

    private void OnFlightViewEnteredMessage(MessageCenterMessage msg)
    {
        if (!config.Enabled()) return;

        Game.ViewController.flightInputHandler._isPrecisionMode = true;

        Logger.LogDebug("Set PrecisionControl to true");
    }
}