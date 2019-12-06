using UnityEngine;
using ToolbarControl_NS;
using System.Reflection;

namespace QuickSAS
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);

            QuickSAS.VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            QuickSAS.MOD = Assembly.GetExecutingAssembly().GetName().Name;
            QuickSAS.relativePath = "QuickMods/" + QuickSAS.MOD;
            QuickSAS.PATH = KSPUtil.ApplicationRootPath + "GameData/" + QuickSAS.relativePath;

            QStockToolbar.TexturePath = QuickSAS.relativePath + "/Textures/StockToolBar";
        }
    }
}