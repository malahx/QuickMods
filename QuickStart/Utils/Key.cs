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

using KSP.Localization;
using UnityEngine;

using ClickThroughFix;

namespace QuickStart.QUtils {
    static class QKey {

        internal static Key setKey = Key.None;

        static Rect rectSetKey = new Rect();
        static Rect RectSetKey {
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


        internal enum Key {
            None,
            Escape
        }

        static string GetText(Key key) {
            switch (key) {
                case Key.Escape:
                    return Localizer.Format("quickstart_stop", QuickStart.MOD);
            }
            return string.Empty;
        }

        static KeyCode CurrentKey(Key key) {
            switch (key) {
                case Key.Escape:
                    return QSettings.Instance.KeyEscape;
            }
            return KeyCode.None;
        }

        static void SetCurrentKey(Key key, KeyCode currentKey) {
            switch (key) {
                case Key.Escape:
                    QSettings.Instance.KeyEscape = currentKey;
                    break;
            }
            QDebug.Log(string.Format("SetCurrentKey({0}): {1}", GetText(key), currentKey), "QKey");
        }

        static void VerifyKey(Key key) {
            try {
                Input.GetKey(CurrentKey(key));
            } catch {
                QDebug.Warning("Wrong key: " + CurrentKey(key), "QKey");
                SetCurrentKey(key, DefaultKey(key));
            }
        }

        internal static void VerifyKey() {
            VerifyKey(Key.Escape);
            QDebug.Log("VerifyKey", "QKey");
        }

        internal static KeyCode DefaultKey(Key key) {
            switch (key) {
                case Key.Escape:
                    return KeyCode.Escape;
            }
            return KeyCode.None;
        }

        internal static bool isKeyDown(Key key) {
            return Input.GetKeyDown(CurrentKey(key));
        }

        internal static bool SetKey() {
            if (setKey == Key.None) {
                return false;
            }
            if (Event.current.isKey) {
                KeyCode key = Event.current.keyCode;
                if (key != KeyCode.None) {
                    SetCurrentKey(setKey, key);
                    setKey = Key.None;
                }
            }
            return true;
        }

        internal static void DrawSetKey() {
            if (setKey == Key.None) {
                return;
            }
            RectSetKey = ClickThruBlocker.GUILayoutWindow(1545156, RectSetKey, DrawSetKey, Localizer.Format("quickstart_setKey", GetText(setKey)), GUILayout.ExpandHeight(true));
        }

        static void DrawSetKey(int id) {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(Localizer.Format("quickstart_pressKey", GetText(setKey)));
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Localizer.Format("quickstart_clearAssign"), GUILayout.ExpandWidth(true), GUILayout.Height(30))) {
                SetCurrentKey(setKey, KeyCode.None);
                setKey = Key.None;
            }
            if (GUILayout.Button(Localizer.Format("quickstart_defaultAssign"), GUILayout.ExpandWidth(true), GUILayout.Height(30))) {
                SetCurrentKey(setKey, DefaultKey(setKey));
                setKey = Key.None;
            }
            if (GUILayout.Button(Localizer.Format("quickstart_cancelAssign"), GUILayout.ExpandWidth(true), GUILayout.Height(30))) {
                setKey = Key.None;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        internal static void DrawConfigKey() {
            DrawConfigKey(Key.Escape);
        }

        static void DrawConfigKey(Key key) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format("{0}: <color=#FFFFFF><b>{1}</b></color>", GetText(key), CurrentKey(key)), GUILayout.Width(350));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(Localizer.Format("quickstart_set"), GUILayout.ExpandWidth(true), GUILayout.Height(20))) {
                setKey = key;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }
    }
}