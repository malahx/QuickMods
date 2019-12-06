using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;



namespace QuickHide
{


[KSPAddon(KSPAddon.Startup.MainMenu, true)]
public class RegisterToolbar : MonoBehaviour
{
    void Start()
    {
            // This is here to get the varsinitialized
            // This mod does not (yet) use the ToolbarController

            //ToolbarControl.RegisterMod(QStockToolbar.MODID, QStockToolbar.MODNAME);

            QuickHide.VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            QuickHide.MOD = Assembly.GetExecutingAssembly().GetName().Name;
            QuickHide.relativePath = "QuickMods/" + QuickHide.MOD;
            QuickHide.PATH = KSPUtil.ApplicationRootPath + "GameData/" + QuickHide.relativePath;
        }
    }
}