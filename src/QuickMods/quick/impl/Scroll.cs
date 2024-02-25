using KSP.Game;
using QuickMods.configuration.impl;
using UnityEngine;

namespace QuickMods.quick.impl;

public class Scroll(ScrollConfiguration config) : ModsBase(config)
{
    private float? _lastMouseX;

    public override void Update()
    {
        if (!config.Enabled() || Game.GlobalGameState?.GetState() != GameState.ResearchAndDevelopment) return;

        _lastMouseX ??= -1;

        if (!config.RightClickRnD() && Input.GetMouseButtonUp(0) || (config.RightClickRnD() && Input.GetMouseButtonUp(1)))
        {
            _lastMouseX = 0;
            Logger.LogDebug("Release scroll");
        }

        if ((!config.RightClickRnD() && !Input.GetMouseButton(0)) || (config.RightClickRnD() && !Input.GetMouseButton(1))) return;

        if (_lastMouseX <= 0) _lastMouseX = Input.mousePosition.x;

        var newScrollValue = Game.UI._rdCenter._tierScroll.value + (_lastMouseX - Input.mousePosition.x) * config.InverseRnD() / Screen.width ?? 0;
        Game.UI._rdCenter._tierScroll.value = Math.Max(0, Math.Min(1, newScrollValue));
        _lastMouseX = Input.mousePosition.x;
    }
}