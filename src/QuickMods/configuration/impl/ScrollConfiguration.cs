using BepInEx.Configuration;

namespace QuickMods.configuration.impl;

public class ScrollConfiguration(ConfigFile config) : ConfigurationBase("QuickScroll", config)
{
    private ConfigEntry<bool> _enableScroll;
    private ConfigEntry<bool> _inverseRnD;
    private ConfigEntry<bool> _rightClickRnD;

    public bool ScrollEnabled() => _enableScroll.Value;

    public int InverseRnD() => _inverseRnD.Value ? -1 : 1;

    public bool RightClickRnD() => _rightClickRnD.Value;

    public override void Init()
    {
        base.Init();

        _enableScroll = Config.Bind("QuickMods/Scroll", "Enable", false, "Enable or disable Scroll");
        _inverseRnD = Config.Bind("QuickMods/Scroll", "InverseRnD", false, "Inverse Research & Development Scroll");
        _rightClickRnD = Config.Bind("QuickMods/Scroll", "RightClickRnD", false, "Right click to Scroll");
    }
}