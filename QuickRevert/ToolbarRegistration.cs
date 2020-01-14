using System.IO;
using UnityEngine;
using ToolbarControl_NS;

using System.Reflection;

namespace QuickRevert
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);

            QuickRevert.VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            QuickRevert.MOD = Assembly.GetExecutingAssembly().GetName().Name;
            QuickRevert.relativePath =  QuickRevert.MOD;
            QuickRevert.PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../" ;

            QStockToolbar.TexturePath = QuickRevert.relativePath + "/Textures/StockToolBar";
        }
    }
}