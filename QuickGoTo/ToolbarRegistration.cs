using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;



namespace QuickGoTo
{
 
[KSPAddon(KSPAddon.Startup.MainMenu, true)]
public class RegisterToolbar : MonoBehaviour
{
    void Start()
    {
            // This is here to get the varsinitialized
            // This mod does not (yet) use the ToolbarController

        //ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);

            QuickGoTo.VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            QuickGoTo.MOD = Assembly.GetExecutingAssembly().GetName().Name;
            QuickGoTo.relativePath = "QuickMods/" + QuickGoTo.MOD;
            QuickGoTo.PATH = KSPUtil.ApplicationRootPath + "GameData/" + QuickGoTo.relativePath;

            QStockToolbar.TexturePath = QuickGoTo.relativePath + "/Textures/StockToolBar";
        }
}
}