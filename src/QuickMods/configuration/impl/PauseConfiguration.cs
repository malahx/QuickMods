using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration.impl;

public class PauseConfiguration(ConfigFile config) : ConfigurationBase("QuickPause")
{
    private ConfigEntry<bool> _enableEscapePause;
    private ConfigEntry<bool> _enableEditorPause;
    private ConfigEntry<KeyCode> _pauseKey;
    private ConfigEntry<bool> _dontPauseWhenDecreaseWarp;

    public bool EscapePauseEnabled() => _enableEscapePause.Value;
    public bool EditorPauseEnabled() => _enableEditorPause.Value;
    public bool DontPauseWhenDecreaseWarp() => _dontPauseWhenDecreaseWarp.Value;
    public KeyCode PauseKey() => _pauseKey.Value;

    public override void Init()
    {
        base.Init();

        _enableEscapePause = config.Bind("QuickMods/Pause", "EnableEscapePause", false, "Enable or disable pause when escape");
        _enableEditorPause = config.Bind("QuickMods/Pause", "EnableEditorPause", false, "Enable or disable pause in the VAB/OAB/Editor");
        _pauseKey = config.Bind("QuickMods/Pause", "PauseKey", KeyCode.P, "Add a new key to toggle pause without using timewarp keys"); // new KeyboardShortcut(KeyCode.P) it seems to doesn't work with KSP!
        _dontPauseWhenDecreaseWarp = config.Bind("QuickMods/StopWarp", "DontPauseWhenDecreaseWarp", false, "Enable or disable the default pause when decrease warp");
    }
}