using ToolbarControl_NS;
using UnityEngine;

namespace QuickMods.utils.Toolbar
{
    public class Toolbar
    {
        private readonly string modName;
        private ToolbarControl toolbarControl;

        public Toolbar(string modName, ToolbarControl toolbarControl)
        {
            this.toolbarControl = toolbarControl;
            this.modName = modName;
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
            Debug.Log($"[QuickMods](Toolbar): Destroy toolbar for {modName}");
        }
        
        public class Builder
        {
            
            private IToolbarConfig toolbarConfig;

            private Component containerComponent;
            
            public Builder Config(IToolbarConfig config)
            {
                toolbarConfig = config;
                return this;
            }

            public Builder Component(Component component)
            {
                containerComponent = component;
                return this;
            }

            public Toolbar Create(ToolbarControl.TC_ClickHandler onTrue, ToolbarControl.TC_ClickHandler onFalse)
            {
                var toolbarControl = containerComponent.gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(
                    onTrue, 
                    onFalse,
                    toolbarConfig.AppScenes(),
                    $"{toolbarConfig.ModName()}_NS",
                    $"{toolbarConfig.ModName()}_ID",
                    toolbarConfig.LargeToolbarIconActive(),
                    toolbarConfig.LargeToolbarIconInactive(),
                    toolbarConfig.SmallToolbarIconActive(),
                    toolbarConfig.SmallToolbarIconInactive(),
                    toolbarConfig.ModName()
                );
                Debug.Log($"[QuickMods](Toolbar): Create toolbar for {toolbarConfig.ModName()}");
                return new Toolbar(toolbarConfig.ModName(), toolbarControl);
            }    
        }
    }
}