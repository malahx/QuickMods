/* 
QuickStart
Copyright 2017 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using UnityEngine;

namespace QuickStart.QUtils {

    public static class QStyle {

        static GUIStyle label;
        public static GUIStyle Label {
            get {
                if (label == null) {
                    label = new GUIStyle();
                    label.stretchWidth = true;
                    label.stretchHeight = true;
                    label.alignment = TextAnchor.MiddleCenter;
                    label.fontSize = (Screen.height / 20);
                    label.fontStyle = FontStyle.Bold;
                    label.normal.textColor = Color.green;
                    if (QSettings.Instance.enableBlackScreen) {
                        label.normal.background = QTexture.ColorToTex(new Vector2(Screen.width, Screen.height), Color.black);
                    }
                }
                return label;
            }
            internal set {
                label = value;
            }
        }

        static GUIStyle button;
        public static GUIStyle Button {
            get {
                if (button == null) {
                    button = new GUIStyle(HighLogic.Skin.button);
                    button.contentOffset = new Vector2(2, 2);
                    button.alignment = TextAnchor.MiddleCenter;
                }
                return button;
            }
        }

    }
}