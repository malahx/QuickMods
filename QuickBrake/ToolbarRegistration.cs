using UnityEngine;
using System.IO;
using ToolbarControl_NS;
using System.Reflection;

namespace QuickBrake
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);


            QuickBrake.VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            QuickBrake.MOD = Assembly.GetExecutingAssembly().GetName().Name;
            QuickBrake.relativePath =  QuickBrake.MOD;
            QuickBrake.PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../";
            QStockToolbar.TexturePath = QuickBrake.relativePath + "/Textures/StockToolBar";

        }
    }
}