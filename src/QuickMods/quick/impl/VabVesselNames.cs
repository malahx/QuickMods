using HoudiniEngineUnity;
using KSP.IO;
using KSP.Messages;
using KSP.OAB;
using QuickMods.configuration.impl;

namespace QuickMods.quick.impl;

public class VabVesselNames(VabVesselNamesConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();

        MessageCenter.Subscribe<FirstPartPlacedMessage>(OnFirstPartPlacedMessage);
        MessageCenter.Subscribe<OABLoadedMessage>(OnOABLoadedMessage);
        MessageCenter.Subscribe<OABUnloadedMessage>(OnOABUnloadedMessage);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        MessageCenter.Unsubscribe<FirstPartPlacedMessage>(OnFirstPartPlacedMessage);
        MessageCenter.Unsubscribe<OABLoadedMessage>(OnOABLoadedMessage);
        MessageCenter.Unsubscribe<OABUnloadedMessage>(OnOABUnloadedMessage);
    }

    private void OnFirstPartPlacedMessage(MessageCenterMessage msg)
    {
        if (!config.AutomaticVesselName() || msg is not FirstPartPlacedMessage partPlacedMessage)
            return;

        var names = FindNames(partPlacedMessage.partType);

        Rename(names.Item1, RetrieveRandomName(names.Item2));
    }

    private (string, List<string>) FindNames(AssemblyPartTypeFilter partType)
    {
        if (config.CustomVesselName())
            return ("CustomName", config.CustomNames);

        return FindNamesFromType(partType) ?? FindNamesFromParts(Game.OAB.Current.Stats.mainAssembly.Parts);
    }

    private (string, List<string>)? FindNamesFromType(AssemblyPartTypeFilter partType)
    {
        return partType switch
        {
            AssemblyPartTypeFilter.Rover => ("Rover", config.RoverNames),
            AssemblyPartTypeFilter.Airplane => ("AirPlane", config.AirPlaneNames),
            AssemblyPartTypeFilter.Spaceplane => ("SpacePlane", config.SpacePlaneNames),
            _ => null
        };
    }

    private (string, List<string>) FindNamesFromParts(IReadOnlyCollection<IObjectAssemblyPart> parts)
    {
        var part = RetrieverParent(parts);

        return FindNamesFromType(part.AvailablePart.PartType) ?? FindNamesFromPart(part);
    }

    private static IObjectAssemblyPart RetrieverParent(IReadOnlyCollection<IObjectAssemblyPart> parts)
    {
        return parts.Filter(p => !p.HasParent()).FirstOrDefault() ??
               parts.Filter(p => p.Category == PartCategories.Pods).FirstOrDefault() ??
               parts.Filter(p => p.Category == PartCategories.Control).FirstOrDefault() ??
               parts.First();
    }

    private (string, List<string>) FindNamesFromPart(IObjectAssemblyPart part)
    {
        return part.Category == PartCategories.Pods ? part.AvailablePart.CrewCapacity > 0 ? ("Crewed", config.CrewedNames) : ("Probe", config.ProbeNames) : ("Launcher", config.LauncherNames);
    }

    private string RetrieveRandomName(IReadOnlyList<string> names)
    {
        if (names.Count == 0) return null;
        if (config.SortNamePicker() == VabVesselNamesConfiguration.EnumSortNamePicker.Random) return names[new Random().Next(0, names.Count - 1)];

        if (config.SortNamePickerCurrentLine() > names.Count)
        {
            config.SortNamePickerCurrentLineReset();
            Logger.LogDebug("Reset SortNamePickerCurrentLine");
        }

        return names[config.SortNamePickerCurrentLine() - 1];
    }

    private void Rename(string vesselType, string name)
    {
        if (name == null) return;

        Game.OAB.Current.Stats.CurrentWorkspaceDisplayName.SetValue(name);
        Game.OAB.Current.Stats.CurrentWorkspaceVehicleDisplayName.SetValue(name);
        Game.OAB.Current.Stats.CurrentWorkspaceDescription.SetValue("");

        Logger.LogDebug($"Set vessel name from {vesselType} list.");
    }

    private void OnOABLoadedMessage(MessageCenterMessage msg) =>
        Game.OAB.Current.Stats.eventsUI.OnSaveWorkspace = PrepareNextVesselName() + Game.OAB.Current.Stats.eventsUI.OnSaveWorkspace;

    private void OnOABUnloadedMessage(MessageCenterMessage msg) =>
        Game.OAB.Current.Stats.eventsUI.OnSaveWorkspace -= PrepareNextVesselName();

    private Action<string, string, string, string, bool, IOProvider.DataLocation> PrepareNextVesselName() => (filename, s1, arg3, arg4, arg5, dataLocation) =>
    {
        var filePath = IOProvider.JoinPath(IOProvider.PathOfDataType(dataLocation), IOProvider.CleanFilename(filename)) + ".json";
        if (config.SortNamePicker() != VabVesselNamesConfiguration.EnumSortNamePicker.Line || IOProvider.FileExists(filePath)) return;

        config.SortNamePickerCurrentLineNext();

        Logger.LogDebug("Increase SortNamePickerCurrentLine");
    };
}