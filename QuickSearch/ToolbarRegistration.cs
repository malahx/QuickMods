using UnityEngine;
using ToolbarControl_NS;

namespace QuickSearch.Toolbar
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(QStock.MODID, QStock.MODNAME);
        }
    }
}