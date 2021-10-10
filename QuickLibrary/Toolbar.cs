using KSP.UI.Screens;
using ToolbarControl_NS;
using UnityEngine;

namespace QuickLibrary
{
    public class Toolbar
    {

        private ToolbarControl toolbarControl;

        private readonly IToolbarConfig toolbarConfig;

        public Toolbar(IToolbarConfig toolbarConfig)
        {
            this.toolbarConfig = toolbarConfig;
        }

        public void SetComponent(Component component)
        {
            toolbarControl = component.gameObject.AddComponent<ToolbarControl>();
        }

        public void Create(ToolbarControl.TC_ClickHandler onTrue, ToolbarControl.TC_ClickHandler onFalse)
        {
            toolbarControl.AddToAllToolbars(
                onTrue, 
                onFalse,
                ApplicationLauncher.AppScenes.FLIGHT,
                $"{toolbarConfig.ModName()}_NS",
                $"{toolbarConfig.ModName()}_ID",
                toolbarConfig.LargeToolbarIconActive(),
                toolbarConfig.LargeToolbarIconInactive(),
                toolbarConfig.SmallToolbarIconActive(),
                toolbarConfig.SmallToolbarIconInactive(),
                toolbarConfig.ModName()
            );
            Debug.Log($"[QuickLibrary](Toolbar): Create toolbar for {toolbarConfig.ModName()}");
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
            Debug.Log($"[QuickLibrary](Toolbar): Destroy toolbar for {toolbarConfig.ModName()}");
        }
        
        public abstract class Register : MonoBehaviour
        {
            private void Start()
            {
                ToolbarControl.RegisterMod(toolbarConfig.ModName());
            }
        }
        
        public interface IToolbarConfig
        {
            string ModName();
            string LargeToolbarIconActive();
            string LargeToolbarIconInactive();
            string SmallToolbarIconActive();
            string SmallToolbarIconInactive();
        }
    }
}