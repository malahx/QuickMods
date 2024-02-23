using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration;

public class ScrollConfiguration : ConfigurationBase
{
    private ConfigEntry<bool> _enable;
    private ConfigEntry<bool> _inverseRnD;
    private ConfigEntry<bool> _rightClickRnD;

    public bool Enabled() => _enable.Value;

    public int InverseRnD() => _inverseRnD.Value ? -1 : 1;
    
    public bool RightClickRnD() => _rightClickRnD.Value;

    public new void Init(ConfigFile config)
    {
        base.Init(config);

        _enable = config.Bind("QuickMods/Scroll", "Enable", false, "Enable or disable Scroll");
        _inverseRnD = config.Bind("QuickMods/Scroll", "InverseRnD", false, "Inverse Research & Development Scroll");
        _rightClickRnD = config.Bind("QuickMods/Scroll", "RightClickRnD", false, "Right click to Scroll");

        Debug.Log($"{GetType()}[{MyPluginInfo.PLUGIN_VERSION}] Configuration initialized.");
    }
}