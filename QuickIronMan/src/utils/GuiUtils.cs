using UnityEngine;

namespace QuickMods.utils
{
    public static class GuiUtils
    {
        public static GUIStyle PrepareBigText(Color color)
        {
            return new GUIStyle
            {
                stretchWidth = true,
                stretchHeight = true,
                alignment = TextAnchor.UpperCenter,
                fontSize = Screen.height / 20,
                fontStyle = FontStyle.Bold,
                normal = {textColor = color}
            };
        }
    }
}