using KSP.UI.Screens;
using ToolbarControl_NS;
using UnityEngine;

namespace QuickLibrary
{
    public class Toolbar
    {

        private ToolbarControl toolbarControl;

        private readonly MonoBehaviour linkedToolbar;
        private static string _modName;
        private readonly string largeToolbarIconActive;
        private readonly string largeToolbarIconInactive;
        private readonly string smallToolbarIconActive;
        private readonly string smallToolbarIconInactive;

        public static void Register(string modName)
        {
            _modName = modName;
        }

        public Toolbar(MonoBehaviour linkedToolbar,
            string largeToolbarIconActive,
            string largeToolbarIconInactive,
            string smallToolbarIconActive,
            string smallToolbarIconInactive)
        {
            this.largeToolbarIconActive = largeToolbarIconActive;
            this.largeToolbarIconInactive = largeToolbarIconInactive;
            this.smallToolbarIconActive = smallToolbarIconActive;
            this.smallToolbarIconInactive = smallToolbarIconInactive;
            this.linkedToolbar = linkedToolbar;
        }

        public void Create(ToolbarControl.TC_ClickHandler onTrue, ToolbarControl.TC_ClickHandler onFalse)
        {
            toolbarControl = linkedToolbar.gameObject.AddComponent<ToolbarControl>();
            toolbarControl.AddToAllToolbars(
                onTrue, 
                onFalse,
                ApplicationLauncher.AppScenes.FLIGHT,
                $"{_modName}_NS",
                $"{_modName}_ID",
                largeToolbarIconActive,
                largeToolbarIconInactive,
                smallToolbarIconActive,
                smallToolbarIconInactive,
                _modName
            );
        }
        
        public void SetTrue()
        {
            toolbarControl.SetTrue();
        }

        public void Destroy()
        {
            toolbarControl.OnDestroy();
            Object.Destroy(toolbarControl);
            toolbarControl = null;
        }
    }
}