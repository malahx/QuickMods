using System.IO;
using UnityEngine;
using ToolbarControl_NS;
using System.Reflection;

namespace QuickExit
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);

            QuickExit.VERSION = Assembly.GetAssembly(typeof(QuickExit)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickExit)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickExit)).GetName().Version.Build;
            QuickExit.MOD = Assembly.GetAssembly(typeof(QuickExit)).GetName().Name;
            QuickExit.relativePath =  QuickExit.MOD;
            QuickExit.PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../";
            QStockToolbar.TexturePath = QuickExit.relativePath + "/Textures/StockToolBar";

        }
    }
}