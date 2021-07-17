using UnityEngine;
using System.IO;
using ToolbarControl_NS;
using System.Reflection;

namespace QuickBrake
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        public static string VERSION;
        public static string MOD = "";
        public static string relativePath;
        public static string PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\", "/") + "/../";

        void Start()
        {
            ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);


            VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            MOD = Assembly.GetExecutingAssembly().GetName().Name;
            relativePath = RegisterToolbar.MOD;
            PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\", "/") + "/../";

            QStockToolbar.TexturePath = relativePath + "/Textures/StockToolBar";

        }
    }
}