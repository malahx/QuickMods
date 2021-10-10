using QuickLibrary;

namespace QuickIronMan
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : Toolbar.Register
    {
        protected override string ModName() => "QuickIronMan";
    }
}