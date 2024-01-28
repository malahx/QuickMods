using ToolbarControl_NS;
using UnityEngine;

namespace QuickMods.utils.Toolbar
{
    public abstract class Register : MonoBehaviour
    {
            
        protected abstract string ModName();
        protected abstract string ModId();

        private void Start()
        {
            ToolbarControl.RegisterMod(ModId(), ModName());
        }
    }
}