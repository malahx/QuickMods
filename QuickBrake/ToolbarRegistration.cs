using UnityEngine;
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
            QuickBrake.relativePath = "QuickMods/" + QuickBrake.MOD;
            QuickBrake.PATH = KSPUtil.ApplicationRootPath + "GameData/" + QuickBrake.relativePath;
            QStockToolbar.TexturePath = QuickBrake.relativePath + "/Textures/StockToolBar";

        }
    }
}