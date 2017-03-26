/* 
QuickMute
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

using System;
using KSP.UI.Screens;
using KSP.UI.Util;
using QuickMute.Toolbar;
using UnityEngine;

namespace QuickMute.QUtils {
    public class QLevel {

        internal bool window = false;
        bool keep = false;
        DateTime keepDate = DateTime.Now;
        Color color = new Color();
        GUIStyle styleWindow;
        GUIStyle styleSlider;
        GUIStyle styleThumb;

        Rect dim = new Rect();
        internal Rect Dim {
            get {
                if (dim.IsEmpty()) {
                    dim.x = (Screen.width - dim.width) / 2;
                    dim.y = (Screen.height - dim.height) / 2;
                    if (QSettings.Instance.StockToolBar) {
                        Rect activeButtonPos;
                        if (QStock.Instance.isActive) {
                            activeButtonPos = QStock.Instance.Position;
                            if (ApplicationLauncher.Instance.IsPositionedAtTop) {
                                dim.x = activeButtonPos.x - dim.width;
                                dim.y = activeButtonPos.y;
                            } else {
                                dim.x = activeButtonPos.x + activeButtonPos.width - dim.width;
                                dim.y = activeButtonPos.y - dim.height;
                            }
                        }
                    }
                    QDebug.Log("Dim init", "QLevel");
                }
                return dim;
            }
            set {
                dim = value;
            }
        }

        public QLevel() {
            styleWindow = new GUIStyle(HighLogic.Skin.window);
            styleWindow.padding = new RectOffset();
            styleSlider = new GUIStyle(HighLogic.Skin.verticalSlider);
            //styleSlider.padding = new RectOffset();
            styleThumb = new GUIStyle(HighLogic.Skin.verticalSliderThumb);
            QDebug.Log("Init", "QLevel");
        }

        internal void OnHover() {
            if (!QSettings.Instance.Level) {
                return;
            }
            Show();
            QDebug.Log("OnHover", "QLevel");
        }

        internal void Hide() {
            Hide(false);
        }

        internal void Hide(bool force) {
            if (!window) {
                return;
            }
            window = false;
            if (force) {
                keep = false;
            }
            QDebug.Log("Hide force: " + force, "QLevel");
        }

        internal void Show() {
            if (window) {
                return;
            }
            window = true;
            if (QSettings.Instance.StockToolBar) {
                keep = true;
            }
            QDebug.Log("Show", "QLevel");
        }

        public void Render() {
            if (!window) {
                return;
            }
            GUI.skin = HighLogic.Skin;
            //GUI.color = color;
            if (QStock.Instance.isActive) {
                if (!QStock.Instance.isHovering && !keep) {
                    Hide();
                    return;
                }
            }
            Dim = GUILayout.Window(1584665, Dim, Draw, "Level", styleWindow);
            if (keep) {
                if ((DateTime.Now - keepDate).TotalSeconds > 5) {
                    keep = false;
                }
                if (QStock.Instance.isHovering) {
                    keepDate = DateTime.Now;
                }
            }
        }

        void Draw(int id) {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.VerticalSlider(100, 100, 0, styleSlider, styleThumb, GUILayout.Width(38));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}