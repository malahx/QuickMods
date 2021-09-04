using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace QuickIronMan
{
    public class SimConfig
    {
        [KSPField(isPersistant = true)] internal static readonly SimConfig INSTANCE = new SimConfig();

        public const string SimulationTexturePath = "QuickMods/QuickIronMan/Textures/sim";

        private SimConfig()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.Major + "." +
                      Assembly.GetExecutingAssembly().GetName().Version.Minor +
                      Assembly.GetExecutingAssembly().GetName().Version.Build;
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            File = $"{path?.Replace(@"\", "/")}/../Config.txt";
            Load();
        }

        public string Version { get; }
        public bool DefaultIsSimulation { get; private set; }
        public KeyCode Key { get; private set; }
        private string File { get; }
        public bool InSimulation = false;

        private void Load()
        {
            Debug.Log($"QuickIronMan[{Version}] Load configuration: {File}");
            try
            {
                InitConfigs(ConfigNode.Load(File).GetNode("QIM"));

                Debug.Log($"QuickIronMan[{Version}] Configuration loaded.");
            }
            catch (Exception e)
            {
                Debug.LogError($"QuickIronMan[{Version}] Configuration could not be load: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void InitConfigs(ConfigNode cfg)
        {
            DefaultIsSimulation = bool.Parse(cfg.GetValue("defaultIsSimulation"));
            Key = Enum.TryParse(cfg.GetValue("key"), out KeyCode value)
                ? value
                : KeyCode.Space;
        }
    }
}