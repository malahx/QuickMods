using HoudiniEngineUnity;
using KSP.Messages;
using KSP.OAB;
using QuickMods.configuration.impl;

namespace QuickMods.quick.impl;

public class VesselNames(VesselNamesConfiguration config) : ModsBase(config)
{
    public override void Start()
    {
        base.Start();
        MessageCenter.Subscribe<FirstPartPlacedMessage>(OnFirstPartPlacedMessage);
        MessageCenter.Subscribe<VesselSavedMessage>(OnVesselSavedMessage);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MessageCenter.Unsubscribe<FirstPartPlacedMessage>(OnFirstPartPlacedMessage);
        MessageCenter.Unsubscribe<VesselSavedMessage>(OnVesselSavedMessage);
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
        if (config.SortNamePicker() == VesselNamesConfiguration.EnumSortNamePicker.Random) return names[new Random().Next(0, names.Count - 1)];

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

    private void OnVesselSavedMessage(MessageCenterMessage msg)
    {
        if (config.SortNamePicker() == VesselNamesConfiguration.EnumSortNamePicker.Random || !Game.OAB.IsLoaded) return;

        //TODO detect if vessel is already saved 
        
        config.SortNamePickerCurrentLineNext();

        Logger.LogDebug("Increase SortNamePickerCurrentLine");
    }
}