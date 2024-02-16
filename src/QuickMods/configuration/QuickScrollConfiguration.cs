using BepInEx.Configuration;

namespace QuickMods.configuration;

public class QuickScrollConfiguration
{
    private ConfigEntry<bool> _enable;
    private ConfigEntry<bool> _inverseRnD;
    
    public bool Enabled()
    {
        return _enable.Value;
    }
    public int InverseRnD()
    {
        return _inverseRnD.Value ? -1 : 1;
    }
    
    public void Init(ConfigFile config)
    {
        _enable = config.Bind("QuickMods/Scroll", "Enable", false, "Enable or disable Scroll");
        _inverseRnD = config.Bind("QuickMods/Scroll", "InverseRnD", false, "Inverse Research & Development Scroll");
    }
}