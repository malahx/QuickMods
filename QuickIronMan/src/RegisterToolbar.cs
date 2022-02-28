using QuickMods.utils.Toolbar;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : Register
    {
        protected override string ModName() => "QuickIronMan";
    }
}