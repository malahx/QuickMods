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

using QuickMute.QUtils;
using QuickMute.Toolbar;
using UnityEngine;

namespace QuickMute {
    public class QGUI {

        internal bool windowSettings = false;
        internal bool draw = false;
        internal QLevel level = new QLevel();

        Rect rectSettings = new Rect();
        Rect RectSettings {
            get {
                if (rectSettings.position == Vector2.zero && rectSettings.size != Vector2.zero) {
                    rectSettings.x = (Screen.width - rectSettings.width) / 2;
                    rectSettings.y = (Screen.height - rectSettings.height) / 2;
                }
                return rectSettings;
            }
            set {
                rectSettings = value;
            }
        }

        Rect rectSetKey = new Rect();
        Rect RectSetKey {
            get {
                if (rectSetKey.position == Vector2.zero && rectSetKey.size != Vector2.zero) {
                    rectSetKey.x = (Screen.width - rectSetKey.width) / 2;
                    rectSetKey.y = (Screen.height - rectSetKey.height) / 2;
                }
                return rectSetKey;
            }
            set {
                rectSetKey = value;
            }
        }

        public void Settings() {
            if (windowSettings) {
                HideSettings();
            } else {
                ShowSettings();
            }
            QDebug.Log("Settings", "QGUI");
        }

        internal void ShowSettings() {
            windowSettings = true;
            Switch(true);
            QDebug.Log("ShowSettings", "QGUI");
        }

        internal void HideSettings() {
            windowSettings = false;
            Switch(false);
            Save();
            QDebug.Log("HideSettings", "QGUI");
        }

        internal void Switch(bool set) {
            QStock.Instance.Set(windowSettings, false);
            QRender.Lock(windowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
            QDebug.Log("Switch: " + set, "QGUI");
        }

        void Save() {
            QStock.Instance.Reset();
            QuickMute.BlizzyToolbar.Reset();
            QSettings.Instance.Save();
            QDebug.Log("Save", "QGUI");
        }

        internal void Render() {
            GUI.skin = HighLogic.Skin;
            if (QSettings.Instance.MuteIcon && draw) {
                GUILayout.BeginArea(new Rect((Screen.width - 96) / 2, (Screen.height - 96) / 2, 96, 96), QTexture.IconTexture);
                GUILayout.EndArea();
            }
            level.Render();
            if (!windowSettings) {
                return;
            }
            if (QKey.SetKey != QKey.Key.None) {
                RectSetKey = GUILayout.Window(1545156, RectSetKey, QKey.DrawSetKey, string.Format("{0} {1}", QLang.translate("Set Key:"), QKey.GetText(QKey.SetKey)), GUILayout.ExpandHeight(true));
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

            QKey.DrawConfigKey(QKey.Key.Mute);
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