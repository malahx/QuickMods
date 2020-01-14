using System.IO;
using UnityEngine;
using ToolbarControl_NS;
using System.Reflection;

namespace QuickSearch.Toolbar
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(QStock.MODID, QStock.MODNAME);

            QuickSearch.VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            QuickSearch.MOD = Assembly.GetExecutingAssembly().GetName().Name;
            QuickSearch.relativePath =  QuickSearch.MOD;
            QuickSearch.PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../" ;
        }
    }
}