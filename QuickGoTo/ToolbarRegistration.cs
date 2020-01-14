using System.IO;
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
            QuickGoTo.relativePath = QuickGoTo.MOD;
            QuickGoTo.PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../" ;

            QStockToolbar.TexturePath = QuickGoTo.relativePath + "/Textures/StockToolBar";
        }
}
}