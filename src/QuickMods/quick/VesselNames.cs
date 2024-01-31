using KSP.Messages;
using KSP.OAB;
using QuickMods.configuration;

namespace QuickMods.quick;

public class VesselNames(string name, QuickVesselNamesConfiguration configuration) : ModsBase(name)
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

        var part = Game.OAB.Current.Stats.mainAssembly.Parts.First();
        var names = partPlacedMessage.partType switch
        {
            AssemblyPartTypeFilter.Rover => ("Rover", configuration?.RoverNames),
            AssemblyPartTypeFilter.Airplane => ("AirPlane", configuration?.AirPlaneNames),
            AssemblyPartTypeFilter.Spaceplane => ("SpacePlane", configuration?.SpacePlaneNames),
            _ => part.Category == PartCategories.Pods ? part.AvailablePart.CrewCapacity > 0 ? ("Crewed", configuration?.CrewedNames) : ("Probe", configuration?.ProbeNames) : ("Launcher", configuration?.LauncherNames)
        };

        Rename(RetrieveRandomName(names.Item2));

        Logger.LogDebug($"Set vessel name from {names.Item1} list.");
    }

    private string RetrieveRandomName(IReadOnlyList<string> names)
    {
        return names[new Random().Next(0, names.Count - 1)];
    }

    private void Rename(string name)
    {
        Game.OAB.Current.Stats.CurrentWorkspaceDisplayName.SetValue(name);
        Game.OAB.Current.Stats.CurrentWorkspaceVehicleDisplayName.SetValue(name);
        Game.OAB.Current.Stats.CurrentWorkspaceDescription.SetValue("");
    }
}