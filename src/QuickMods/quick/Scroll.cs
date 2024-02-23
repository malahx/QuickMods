using KSP.Game;
using QuickMods.configuration;
using UnityEngine;

namespace QuickMods.quick;

public class Scroll(string name, ScrollConfiguration configuration) : ModsBase(name, configuration)
{
    private float? _lastMouseX;

    public override void Update()
    {
        if (!configuration.Enabled() || Game.GlobalGameState?.GetState() != GameState.ResearchAndDevelopment) return;

        _lastMouseX ??= -1;

        if (!configuration.RightClickRnD() && Input.GetMouseButtonUp(0) || (configuration.RightClickRnD() && Input.GetMouseButtonUp(1)))
        {
            _lastMouseX = 0;
            Logger.LogDebug("Release scroll");
        }

        if ((!configuration.RightClickRnD() && !Input.GetMouseButton(0)) || (configuration.RightClickRnD() && !Input.GetMouseButton(1))) return;

        if (_lastMouseX <= 0) _lastMouseX = Input.mousePosition.x;

        var newScrollValue = Game.UI._rdCenter._tierScroll.value + (_lastMouseX - Input.mousePosition.x) * configuration.InverseRnD() / Screen.width ?? 0;
        Game.UI._rdCenter._tierScroll.value = Math.Max(0, Math.Min(1, newScrollValue));
        _lastMouseX = Input.mousePosition.x;
    }
}