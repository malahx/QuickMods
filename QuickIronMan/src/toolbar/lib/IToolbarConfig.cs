using KSP.UI.Screens;

namespace QuickMods.utils.Toolbar
{
    public interface IToolbarConfig
    {
        string ModName();
        string LargeToolbarIconActive();
        string LargeToolbarIconInactive();
        string SmallToolbarIconActive();
        string SmallToolbarIconInactive();
        ApplicationLauncher.AppScenes AppScenes();
    }
}