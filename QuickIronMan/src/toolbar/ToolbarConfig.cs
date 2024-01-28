using KSP.UI.Screens;
using QuickMods.utils.Toolbar;

namespace QuickIronMan.toolbar {
    public class ToolbarConfig : IToolbarConfig {
        public string ModName() => "QuickIronMan";
        public string LargeToolbarIconActive() => "QuickMods/QuickIronMan/Textures/toolbar_insim";
        public string LargeToolbarIconInactive() => "QuickMods/QuickIronMan/Textures/toolbar_sim";
        public string SmallToolbarIconActive() => "QuickMods/QuickIronMan/Textures/toolbar_insim";
        public string SmallToolbarIconInactive() => "QuickMods/QuickIronMan/Textures/toolbar_sim";
        public ApplicationLauncher.AppScenes AppScenes() => ApplicationLauncher.AppScenes.FLIGHT;
    }
}
