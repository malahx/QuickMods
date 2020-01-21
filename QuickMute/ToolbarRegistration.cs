using UnityEngine;
using ToolbarControl_NS;
using System.IO;
using System.Reflection;

namespace QuickMute
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        public static string VERSION;
        public static string MOD = "";
        public static string relativePath;
        public static string PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\", "/") + "/../";

        void Start()
        {
            ToolbarControl.RegisterMod(QStock.MODID, QStock.MODNAME);

            VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            MOD = Assembly.GetExecutingAssembly().GetName().Name;
            relativePath = MOD;
            PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\", "/") + "/../";            
        }
    }
}