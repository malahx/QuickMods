using KSP.Messages;
using QuickMods.configuration.impl;

namespace QuickMods.quick.impl;

public class Pause(PauseConfiguration config) : ModsBase(config)
{
    private bool? _lastPauseValue;
    
    public override void Start()
    {
        base.Start();
        MessageCenter.Subscribe<EscapeMenuOpenedMessage>(OnEscapeMenuOpenedMessage);
        MessageCenter.Subscribe<EscapeMenuClosedMessage>(OnEscapeMenuClosedMessage);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MessageCenter.Unsubscribe<EscapeMenuOpenedMessage>(OnEscapeMenuOpenedMessage);
        MessageCenter.Unsubscribe<EscapeMenuClosedMessage>(OnEscapeMenuClosedMessage);
    }

    private void OnEscapeMenuOpenedMessage(MessageCenterMessage msg)
    {
        _lastPauseValue = Game.ViewController.IsPaused;
        ForcePause(true);
    }

    private void OnEscapeMenuClosedMessage(MessageCenterMessage msg)
    {
        ForcePause(_lastPauseValue);
        _lastPauseValue = null;
    }

    private void ForcePause(bool? pause)
    {
        if (!config.Enabled() || !pause.HasValue) return;

        var newPauseState = pause.GetValueOrDefault(false);
        Game.ViewController.SetPause(newPauseState);

        Logger.LogDebug($"Set pause to {newPauseState}");
    }
}