using UnityEngine;
using ToolbarControl_NS;
using System.Reflection;

namespace QuickIVA
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);

            QuickIVA.VERSION = Assembly.GetAssembly(typeof(QuickIVA)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickIVA)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickIVA)).GetName().Version.Build;
            QuickIVA.MOD = Assembly.GetAssembly(typeof(QuickIVA)).GetName().Name;
            QuickIVA.relativePath = "QuickMods/" + QuickIVA.MOD;
            QuickIVA.PATH = KSPUtil.ApplicationRootPath + "GameData/" + QuickIVA.relativePath;

            QStockToolbar.TexturePath = QuickIVA.relativePath + "/Textures/StockToolBar";

        }
    }
}