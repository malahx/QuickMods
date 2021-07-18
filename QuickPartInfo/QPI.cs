using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace QuickPartInfo {

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class QuickPartInfo : MonoBehaviour {

        public static string VERSION = Assembly.GetExecutingAssembly().GetName().Version.Major + "." + Assembly.GetExecutingAssembly().GetName().Version.Minor + Assembly.GetExecutingAssembly().GetName().Version.Build;

        public static string[] MAIN_MODS = { "REPOSoftTech" };
        public static IDictionary<string, string> MODS_NAME = new Dictionary<string, string>() {
            { "Serenity", "Breaking Ground" }
        };

        void OnDestroy() {
            int i = 0;
            foreach (AvailablePart p in PartLoader.LoadedPartsList) {

                int firstIndex = p.partUrl.IndexOf('/');
                if (firstIndex > 0 && p.partUrl != null) {

                    string title = CalculateTitle(p.partUrl, firstIndex);

                    p.description += string.Format(" <b><color=green>From {0}.</color></b>", title);
                    i++;
                }
            }

            Debug.Log(string.Format("QuickPartInfo[{0}] Descriptions tweaks: {1}", VERSION, i));
        }

        private string CalculateTitle(string partUrl, int firstIndex) {
            string modName = partUrl.Substring(0, firstIndex);

            if (modName.Equals("Squad")) 
                return modName;

            if (modName.Equals("SquadExpansion")) 
                return SubMod(partUrl, firstIndex, "Expansion");

            if (MAIN_MODS.IndexOf(modName) != -1) 
                return SubMod(partUrl, firstIndex, "mod");
          
            return CleanModName(modName, "mod");
        }

        private string SubMod(string partUrl, int firstIndex, string type) {
            int secondIndex = partUrl.IndexOf('/', firstIndex + 1) - (firstIndex + 1);

            if (secondIndex > 0) {
                string modName = partUrl.Substring(firstIndex + 1, secondIndex);

                return CleanModName(modName, type);
            }
            return "";
        }

        private string CleanModName(string modName, string type) {
            modName = Regex.Replace(modName, "([^A-Z]+)([A-Z]{1})", "$1 $2");

            if (MODS_NAME.ContainsKey(modName))
                modName = MODS_NAME[modName];

            return string.Format("{0} {1}", modName, type);
        }
    }
}
