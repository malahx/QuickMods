using QuickMods.utils.Toolbar;

namespace QuickIronMan.toolbar
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : Register
    {
        protected override string ModName() => "QuickIronMan";
        protected override string ModId() => "QuickIronMan_NS";
    }
}