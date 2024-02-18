using BepInEx;
using HarmonyLib;
using QuickMods.configuration;
using QuickMods.quick;
using SpaceWarp;
using SpaceWarp.API.Mods;

namespace QuickMods;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class QuickModsPlugin : BaseSpaceWarpPlugin
{
    private static readonly Configuration Configuration = new();

    private readonly List<ModsBase> _mods =
    [
        new StopWarp("QuickStopWarp", Configuration.StopWarp),
        new Revert("QuickRevert", Configuration.Revert),
        new VesselNames("QuickVesselNames", Configuration.VesselNames),
        new Scroll("QuickScroll", Configuration.Scroll)
    ];

    public override void OnInitialized()
    {
        base.OnInitialized();
        Harmony.CreateAndPatchAll(typeof(QuickModsPlugin).Assembly);

        Configuration.Init(Config);
        foreach (var m in _mods)
            m.Start();
    }

    private void OnDestroy()
    {
        foreach (var m in _mods)
            m.OnDestroy();
    }

    private void Update()
    {
        foreach (var m in _mods)
            m.Update();
    }
}