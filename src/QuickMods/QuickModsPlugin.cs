using BepInEx;
using HarmonyLib;
using KSP.Messages;
using QuickMods.configuration.impl;
using QuickMods.quick;
using QuickMods.quick.impl;
using SpaceWarp;
using SpaceWarp.API.Mods;

namespace QuickMods;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class QuickModsPlugin : BaseSpaceWarpPlugin
{
    private List<IModsBase> _mods = [];

    public QuickModsPlugin()
    {
        _mods.Add(new StopWarp(new StopWarpConfiguration(Config)));
        _mods.Add(new Revert(new RevertConfiguration(Config)));
        _mods.Add(new VABVesselNames(new VABVesselNamesConfiguration(Config)));
        _mods.Add(new Scroll(new ScrollConfiguration(Config)));
        _mods.Add(new PrecisionControl(new PrecisionControlConfiguration(Config)));
        _mods.Add(new Pause(new PauseConfiguration(Config)));
        _mods.Add(new Brake(new BrakeConfiguration(Config)));
    }

    public override void OnInitialized()
    {
        Harmony.CreateAndPatchAll(typeof(QuickModsPlugin).Assembly);

        Messages.Subscribe<EscapeMenuClosedMessage>(OnEscapeMenuClosedMessage);

        InitializeMods();
    }

    private void OnEscapeMenuClosedMessage(MessageCenterMessage msg) => InitializeMods();

    private void InitializeMods()
    {
        List<IModsBase> mods = [];
        foreach (var m in _mods)
        {
            if (m.Enabled() && !m.Initialized())
                m.Start();

            if (!m.Enabled())
            {
                m.OnDestroy();
                continue;
            }
            
            mods.Add(m);
        }

        _mods = mods;
    }

    private void OnDestroy()
    {
        Messages.Unsubscribe<EscapeMenuClosedMessage>(OnEscapeMenuClosedMessage);
        
        foreach (var m in _mods.Where(m => m.Initialized()))
            m.OnDestroy();
    }

    private void Update()
    {
        foreach (var m in _mods.Where(m => m.Active()))
            m.Update();
    }
}