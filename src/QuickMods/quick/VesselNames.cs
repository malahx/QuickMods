using HoudiniEngineUnity;
using KSP.Messages;
using KSP.OAB;
using QuickMods.configuration;

namespace QuickMods.quick;

public class VesselNames(string name, VesselNamesConfiguration configuration) : ModsBase(name)
{
    public override void Start()
    {
        MessageCenter.Subscribe<FirstPartPlacedMessage>(OnFirstPartPlacedMessage);
    }

    public override void OnDestroy()
    {
        MessageCenter.Unsubscribe<FirstPartPlacedMessage>(OnFirstPartPlacedMessage);
    }

    private void OnFirstPartPlacedMessage(MessageCenterMessage msg)
    {
        if (!configuration.AutomaticVesselName() || msg is not FirstPartPlacedMessage partPlacedMessage)
            return;

        var names = FindNames(partPlacedMessage.partType);

        Rename(names.Item1, RetrieveRandomName(names.Item2));
    }

    private (string, string[]) FindNames(AssemblyPartTypeFilter partType)
    {
        if (configuration.CustomVesselName())
            return ("CustomName", configuration.CustomNames);

        return FindNamesFromType(partType) ?? FindNamesFromParts(Game.OAB.Current.Stats.mainAssembly.Parts);
    }

    private (string, string[])? FindNamesFromType(AssemblyPartTypeFilter partType)
    {
        return partType switch
        {
            AssemblyPartTypeFilter.Rover => ("Rover", configuration.RoverNames),
            AssemblyPartTypeFilter.Airplane => ("AirPlane", configuration.AirPlaneNames),
            AssemblyPartTypeFilter.Spaceplane => ("SpacePlane", configuration.SpacePlaneNames),
            _ => null
        };
    }

    private (string, string[]) FindNamesFromParts(IReadOnlyCollection<IObjectAssemblyPart> parts)
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

    private (string, string[]) FindNamesFromPart(IObjectAssemblyPart part)
    {
        return part.Category == PartCategories.Pods ? part.AvailablePart.CrewCapacity > 0 ? ("Crewed", configuration.CrewedNames) : ("Probe", configuration.ProbeNames) : ("Launcher", configuration.LauncherNames);
    }

    private static string RetrieveRandomName(IReadOnlyList<string> names)
    {
        return names.Count == 0 ? null : names[new Random().Next(0, names.Count - 1)];
    }

    private void Rename(string vesselType, string name)
    {
        if (name == null) return;

        Game.OAB.Current.Stats.CurrentWorkspaceDisplayName.SetValue(name);
        Game.OAB.Current.Stats.CurrentWorkspaceVehicleDisplayName.SetValue(name);
        Game.OAB.Current.Stats.CurrentWorkspaceDescription.SetValue("");

        Logger.LogDebug($"Set vessel name from {vesselType} list.");
    }
}