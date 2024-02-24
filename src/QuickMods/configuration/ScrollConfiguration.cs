using BepInEx.Configuration;

namespace QuickMods.configuration;

public class ScrollConfiguration(ConfigFile config) : ConfigurationBase("QuickScroll")
{
    private ConfigEntry<bool> _enable;
    private ConfigEntry<bool> _inverseRnD;
    private ConfigEntry<bool> _rightClickRnD;

    public bool Enabled() => _enable.Value;

    public int InverseRnD() => _inverseRnD.Value ? -1 : 1;

    public bool RightClickRnD() => _rightClickRnD.Value;

    public override void Init()
    {
        base.Init();

        _enable = config.Bind("QuickMods/Scroll", "Enable", false, "Enable or disable Scroll");
        _inverseRnD = config.Bind("QuickMods/Scroll", "InverseRnD", false, "Inverse Research & Development Scroll");
        _rightClickRnD = config.Bind("QuickMods/Scroll", "RightClickRnD", false, "Right click to Scroll");
    }
}