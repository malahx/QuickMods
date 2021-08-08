using System.Text.RegularExpressions;

namespace QuickModsInfo
{
    public static class ModNameCalculation
    {
        internal static string CalculateModName(ModDefinition mod, ModDefinition submod)
        {
            if (submod == null || !mod.HasSubMods && !mod.Expansion) return CalculateModName(mod);

            var modName = "";

            if (mod.SubModsUseThisName) modName = $"{CalculateModName(mod)} ";

            return $"{modName}{CalculateModName(submod)}";
        }

        internal static string CalculateModName(ModDefinition mod)
        {
            var modName = mod.DisplayName ?? mod.Name;

            if (mod.UseAcronym)
                modName = Regex.Replace(modName, "[a-z]", "");

            return CalculateModName(modName);
        }

        private static string CalculateModName(string modName)
        {
            return Regex.Replace(modName, "([^A-Z \\(\\)\\[\\]\\{\\}\\^\\+/\\\\]+)([A-Z]{1})", "$1 $2");
        }
    }
}