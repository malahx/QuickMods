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

using QuickMute.Toolbar;
using UnityEngine;

namespace QuickMute.Object {
    public class QGui {

        bool windowSettings = false;
        internal bool draw = false;
        internal QLevel level;
        internal QKey qKey;

        public QGui(QKey qKey, QLevel level) {
            this.qKey = qKey;
            this.level = level; 
        }

        Rect rectSettings = new Rect();
        Rect RectSettings {
            get {
                if (rectSettings.IsEmpty()) {
                    rectSettings.x = (Screen.width - rectSettings.width) / 2;
                    rectSettings.y = (Screen.height - rectSettings.height) / 2;
                }
                return rectSettings;
            }
            set {
                rectSettings = value;
            }
        }

        internal bool isHovering {
            get {
                return windowSettings && (RectSettings.Contains(Mouse.screenPos) || qKey.isHovering);
            }
        }

        public void Settings() {
            if (windowSettings) {
                HideSettings();
            } else {
                ShowSettings();
            }
            QDebug.Log("Settings", "QGui");
        }

        internal void ShowSettings() {
            windowSettings = true;
            QStock.Instance.Set(windowSettings, false);
            QDebug.Log("ShowSettings", "QGui");
        }

        internal void HideSettings() {
            windowSettings = false;
            QStock.Instance.Set(windowSettings, false);
            Save();
            QDebug.Log("HideSettings", "QGui");
        }

        void Save() {
            QStock.Instance.Reset();
            QuickMute.BlizzyToolbar.Reset();
            QSettings.Instance.Save();
            QDebug.Log("Save", "QGui");
        }

        internal void Render() {
            if (QRender.isHide) {
                return;
            }
            GUI.skin = HighLogic.Skin;
            level.Render();
            if (QSettings.Instance.MuteIcon && draw) {
                GUILayout.BeginArea(new Rect((Screen.width - 96) / 2, (Screen.height - 96) / 2, 96, 96), QTexture.IconTexture);
                GUILayout.EndArea();
            }
            if (!windowSettings || qKey.Render()) {
                return;
            }
            RectSettings = GUILayout.Window(1545175, RectSettings, DrawSettings, QVars.MOD + " " + QVars.VERSION);
        }

        void DrawSettings(int id) {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box(QLang.translate("Toolbars"), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            QSettings.Instance.StockToolBar = GUILayout.Toggle(QSettings.Instance.StockToolBar, QLang.translate("Use the Stock Toolbar"), GUILayout.Width(400));
            GUILayout.EndHorizontal();

            if (QBlizzy.isAvailable) {

                GUILayout.BeginHorizontal();
                QSettings.Instance.BlizzyToolBar = GUILayout.Toggle(QSettings.Instance.BlizzyToolBar, QLang.translate("Use the Blizzy Toolbar"), GUILayout.Width(400));
                GUILayout.EndHorizontal();

            }

            GUILayout.BeginHorizontal();
            GUILayout.Box(QLang.translate("Options"), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            QSettings.Instance.MuteIcon = GUILayout.Toggle(QSettings.Instance.MuteIcon, QLang.translate("Show Mute Icon"), GUILayout.Width(400));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(QLang.translate("Keyboard shortcuts"), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            qKey.DrawConfigKey(QKey.Key.Mute);
            QLang.DrawLang();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(QLang.translate("Close"), GUILayout.Height(30))) {
                HideSettings();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}