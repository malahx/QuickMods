using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace QuickPartInfo
{
    public class PartInfoConfig
    {
        public PartInfoConfig()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                      Assembly.GetExecutingAssembly().GetName().Version.Minor +
                      Assembly.GetExecutingAssembly().GetName().Version.Build;
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            File = $"{path?.Replace(@"\", "/")}/../Config.txt";
            Load();
        }

        public string File { get; }
        public string Version { get; }
        public string StockMessage { get; private set; }
        public string ExpansionMessage { get; private set; }
        public string ModMessage { get; private set; }
        public string SubModMessage { get; private set; }
        public List<ModDefinition> Mods { get; private set; }

        private void Load()
        {
            Debug.Log($"QuickPartInfo[{Version}] Load configuration: {File}");
            try
            {
                InitConfigs(ConfigNode.Load(File).GetNode("QPI"));

                Debug.Log($"QuickPartInfo[{Version}] Configuration loaded.");
            }
            catch (Exception e)
            {
                Debug.LogError($"QuickPartInfo[{Version}] Configuration could not be load: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void InitConfigs(ConfigNode cfg)
        {
            StockMessage = cfg.GetNode("Message").GetValue("stock");
            SubModMessage = cfg.GetNode("Message").GetValue("submod");
            ModMessage = cfg.GetNode("Message").GetValue("mod");
            ExpansionMessage = cfg.GetNode("Message").GetValue("expansion");

            Mods = new List<ModDefinition>();
            foreach (var c in cfg.GetNodes("Mod"))
            {
                var mod = new ModDefinition
                {
                    Name = c.GetValue("name"),
                    DisplayName = c.GetValue("displayName"),
                    UseAcronym = bool.Parse(c.GetValue("useAcronym") ?? "false"),
                    HasSubMods = bool.Parse(c.GetValue("hasSubMods") ?? "false"),
                    SubModsUseThisName = bool.Parse(c.GetValue("subModsUseThisName") ?? "false"),
                    Expansion = bool.Parse(c.GetValue("expansion") ?? "false"),
                    Stock = bool.Parse(c.GetValue("stock") ?? "false")
                };

                Mods.Add(mod);
            }
        }
    }
}