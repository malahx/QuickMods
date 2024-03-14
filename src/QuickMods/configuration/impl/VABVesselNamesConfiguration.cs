using System.Reflection;
using BepInEx.Configuration;
using UnityEngine;

namespace QuickMods.configuration.impl;

public class VABVesselNamesConfiguration(ConfigFile config) : ConfigurationBase("QuickVABVesselNames")
{
    private ConfigEntry<bool> _automaticVesselName;
    private ConfigEntry<bool> _customVesselName;
    private ConfigEntry<EnumSortNamePicker> _sortNamePicker;
    private ConfigEntry<int> _sortNamePickerCurrentLine;

    public bool AutomaticVesselName()
    {
        return _automaticVesselName.Value;
    }

    public bool CustomVesselName()
    {
        return _customVesselName.Value;
    }

    public EnumSortNamePicker SortNamePicker()
    {
        return _sortNamePicker.Value;
    }

    public int SortNamePickerCurrentLine()
    {
        return _sortNamePickerCurrentLine.Value;
    }

    public void SortNamePickerCurrentLineNext()
    {
        _sortNamePickerCurrentLine.Value++;
    }

    public void SortNamePickerCurrentLineReset()
    {
        _sortNamePickerCurrentLine.Value = 1;
    }

    public readonly List<string> CrewedNames = [];
    public readonly List<string> LauncherNames = [];
    public readonly List<string> ProbeNames = [];
    public readonly List<string> RoverNames = [];
    public readonly List<string> AirPlaneNames = [];
    public readonly List<string> SpacePlaneNames = [];
    public readonly List<string> CustomNames = [];

    private const string FolderVesselNames = "VesselNames";
    private const string DefaultFileName = "default_";

    private const string CrewedNamesFile = "CrewedNames.txt";
    private const string LauncherNamesFile = "LauncherNames.txt";
    private const string ProbeNamesFile = "ProbeNames.txt";
    private const string RoverNamesFile = "RoverNames.txt";
    private const string AirPlaneFile = "AirPlaneNames.txt";
    private const string SpacePlaneFile = "SpacePlaneNames.txt";
    private const string CustomFile = "CustomNames.txt";

    public override void Init()
    {
        base.Init();

        _automaticVesselName = config.Bind("QuickMods/VesselNames", "AutomaticVesselName", false, "Enable or disable the automatic vessel name");
        _sortNamePicker = config.Bind("QuickMods/VesselNames", "SortNamePicker", EnumSortNamePicker.Random, "Chose the method to pick the name");
        _sortNamePickerCurrentLine = config.Bind("QuickMods/VesselNames", "SortNamePickerCurrentLine", 1, "The current line of the file to pick the next vessel name (if you selected SortNamePicker by Line)\n\nThe value will increase each time you save a new vessel.");
        _customVesselName = config.Bind("QuickMods/VesselNames", "CustomVesselName", false, $"Enable or disable the custom vessel name, you need to create the file in {CustomFile}");

        var files = new Dictionary<string, List<string>> { { CustomFile, CustomNames }, { CrewedNamesFile, CrewedNames }, { LauncherNamesFile, LauncherNames }, { ProbeNamesFile, ProbeNames }, { RoverNamesFile, RoverNames }, { AirPlaneFile, AirPlaneNames }, { SpacePlaneFile, SpacePlaneNames } };

        try
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var completePath = $"{path?.Replace(@"\", "/")}";
            foreach (var file in files)
            {
                var filePath = $"{completePath}/{FolderVesselNames}/{file.Key}";
                var defaultFilePath = $"{completePath}/{FolderVesselNames}/{DefaultFileName}{file.Key}";

                if (!File.Exists(filePath))
                    if (File.Exists(defaultFilePath))
                        File.Copy(defaultFilePath, filePath);
                    else
                        File.Create(filePath).Close();

                if (File.Exists(filePath))
                    file.Value.AddRange(File.ReadAllLines(filePath));
            }
        }
        catch (Exception e)
        {
            Logger.LogError($"Vessel names could not be load: {e.Message}");
            Debug.LogException(e);
        }
    }

    public enum EnumSortNamePicker
    {
        Line,
        Random
    }
}