using ToolbarControl_NS;
using UnityEngine;

namespace QuickMods.utils.Toolbar
{
    public abstract class Register : MonoBehaviour
    {
            
        protected abstract string ModName();
            
        private void Start()
        {
            ToolbarControl.RegisterMod(ModName());
        }
    }
}