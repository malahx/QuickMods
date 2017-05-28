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
using UnityEngine;

namespace QuickMute.Object {
	public class QKey {

        internal enum Key {
            None,
            Mute
        }

        Key SetKey = Key.None;

        Rect rectSetKey = new Rect();
        Rect RectSetKey {
            get {
                if (rectSetKey.IsEmpty()) {
                    rectSetKey.x = (Screen.width - rectSetKey.width) / 2;
                    rectSetKey.y = (Screen.height - rectSetKey.height) / 2;
                }
                return rectSetKey;
            }
            set {
                rectSetKey = value;
            }
        }
	
		internal static KeyCode DefaultKey(Key key) {
			return KeyCode.F6;
		}

        internal bool isHovering {
            get {
                return SetKey != Key.None && RectSetKey.Contains(Mouse.screenPos);
            }
        }

		bool isKeyDown(Key key) {
			return Input.GetKeyDown (CurrentKey (key));
		}

		string GetText(Key key) {
            return Localizer.Format("quickmute_mute");
		}

		KeyCode CurrentKey(Key key) {
			return QSettings.Instance.KeyMute;
		}

		void VerifyKey() {
			try {
				Input.GetKey (CurrentKey (Key.Mute));
			}
			catch {
				QDebug.Warning ("Wrong key: " + CurrentKey (Key.Mute), "QKey");
				SetCurrentKey (Key.Mute, DefaultKey (Key.Mute));
			}
			QDebug.Log ("VerifyKey", "QKey");
		}

		void SetCurrentKey(Key key, KeyCode currentKey) {
			QSettings.Instance.KeyMute = currentKey;
			QDebug.Log (string.Format("SetCurrentKey({0}): {1}", GetText(key), currentKey), "QKey");
		}

		void DrawSetKey(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (Localizer.Format("quickmute_pressKey", GetText (SetKey)));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (Localizer.Format("quickmute_clearAssign"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, KeyCode.None);
				SetKey = Key.None;
			}
			if (GUILayout.Button (Localizer.Format("quickmute_defaultAssign"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, DefaultKey (SetKey));
				SetKey = Key.None;
			}
			if (GUILayout.Button (Localizer.Format("quickmute_cancelAssign"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetKey = Key.None;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.EndVertical ();
		}

		internal void DrawConfigKey(Key key) {
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format ("{0}: <color=#FFFFFF><b>{1}</b></color>", GetText (key), CurrentKey (key)), GUILayout.Width (350));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button (Localizer.Format("quickmute_set"), GUILayout.ExpandWidth (true), GUILayout.Height (20))) {
				SetKey = key;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
		}

        internal void Update() {
            if (SetKey != Key.None) {
                if (Event.current.isKey) {
                    KeyCode _key = Event.current.keyCode;
                    if (_key != KeyCode.None) {
                        SetCurrentKey(SetKey, _key);
                        SetKey = Key.None;
                    }
                }
                return;
            }
            if (isKeyDown(Key.Mute)) {
                QuickMute.Instance.Mute();
            }
        }

        internal bool Render() {
            if (SetKey == Key.None) {
                return false;
            }
            RectSetKey = GUILayout.Window(1545156, RectSetKey, DrawSetKey, Localizer.Format("quickmute_setKey", GetText(SetKey)), GUILayout.ExpandHeight(true));
            return true;
        }
	}
}