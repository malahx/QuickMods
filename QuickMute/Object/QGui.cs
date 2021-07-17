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

using KSP.Localization;
//using QuickMute.Toolbar;
using UnityEngine;

using ClickThroughFix;

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
            //QuickMute.BlizzyToolbar.Reset();
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
            RectSettings = ClickThruBlocker.GUILayoutWindow(1545175, RectSettings, DrawSettings, RegisterToolbar.MOD + " " + RegisterToolbar.VERSION);
        }

        void DrawSettings(int id) {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Box(Localizer.Format("quickmute_toolbars"), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(Localizer.Format("quickmute_options"), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            QSettings.Instance.MuteIcon = GUILayout.Toggle(QSettings.Instance.MuteIcon, Localizer.Format("quickmute_muteIcon"), GUILayout.Width(400));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box(Localizer.Format("quickmute_keyShortcuts"), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            qKey.DrawConfigKey(QKey.Key.Mute);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(Localizer.Format("quickmute_close"), GUILayout.Height(30))) {
                HideSettings();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}