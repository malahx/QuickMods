using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration.impl;

public class PauseConfiguration(ConfigFile config) : ConfigurationBase("QuickPauseControl")
{
    private ConfigEntry<bool> _enable;
    private ConfigEntry<KeyboardShortcut> _pauseKey;
    private ConfigEntry<bool> _dontPauseWhenDecreaseWarp;

    public bool Enabled() => _enable.Value;
    public bool DontPauseWhenDecreaseWarp() => _dontPauseWhenDecreaseWarp.Value;
    public KeyboardShortcut PauseKey() => _pauseKey.Value;

    public override void Init()
    {
        base.Init();

        _enable = config.Bind("QuickMods/Pause", "Enable", false, "Enable or disable pause when escape");

        _pauseKey = config.Bind("QuickMods/Pause", "PauseKey",  new KeyboardShortcut(KeyCode.P), "Add a new key to toggle pause without using timewarp keys");
        _dontPauseWhenDecreaseWarp = config.Bind("QuickMods/StopWarp", "DontPauseWhenDecreaseWarp", false, "Enable or disable the default pause when decrease warp");
    }
}