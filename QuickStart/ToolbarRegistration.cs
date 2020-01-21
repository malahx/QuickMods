
using System.IO;
using UnityEngine;
using System.Reflection;

namespace QuickStart
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        public static string VERSION;
        public static string MOD = "";
        public static string relativePath;
        public static string PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace(@"\", "/") + "/../";

        void Start()
        {
            VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;
            MOD = Assembly.GetExecutingAssembly().GetName().Name;
            relativePath = MOD;
            PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../";
        }
    }
}