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

    private readonly List<IModsBase> _mods = [];
    public QuickModsPlugin()
    {
        _mods.Add(new StopWarp(new StopWarpConfiguration(Config)));
        _mods.Add(new Revert(new RevertConfiguration(Config)));
        _mods.Add(new VesselNames(new VesselNamesConfiguration(Config)));
        _mods.Add(new Scroll(new ScrollConfiguration(Config)));
    }

    public override void OnInitialized()
    {
        Harmony.CreateAndPatchAll(typeof(QuickModsPlugin).Assembly);

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
        foreach (var m in _mods.Where(m => m.Initialized()))
            m.Update();
    }
}