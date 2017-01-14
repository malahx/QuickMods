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

using UnityEngine;

namespace QuickMute {
	public class QKey : QuickMute {
	
		internal static Key SetKey = Key.None;

		internal enum Key {
			None,
			Mute
		}					
	
		internal static KeyCode DefaultKey(Key key) {
			return KeyCode.F6;
		}

		internal static bool isKeyDown(Key key) {
			return Input.GetKeyDown (CurrentKey (key));
		}

		internal static string GetText(Key key) {
			return QLang.translate ("Mute");
		}

		internal static KeyCode CurrentKey(Key key) {
			return QSettings.Instance.KeyMute;
		}

		internal static void VerifyKey() {
			try {
				Input.GetKey (CurrentKey (Key.Mute));
			}
			catch {
				Warning ("Wrong key: " + CurrentKey (Key.Mute), "QKey");
				SetCurrentKey (Key.Mute, DefaultKey (Key.Mute));
			}
			Log ("VerifyKey", "QKey");
		}

		internal static void SetCurrentKey(Key key, KeyCode currentKey) {
			QSettings.Instance.KeyMute = currentKey;
			Log (string.Format("SetCurrentKey({0}): {1}", GetText(key), currentKey), "QKey");
		}

		internal static void DrawSetKey(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format ("{0} <color=#FFFFFF><b>{1}</b></color>", QLang.translate ("Press a key to select the"), GetText (SetKey)));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (QLang.translate ("Clear Assignment"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, KeyCode.None);
				SetKey = Key.None;
			}
			if (GUILayout.Button (QLang.translate ("Default Assignment"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, DefaultKey (SetKey));
				SetKey = Key.None;
			}
			if (GUILayout.Button (QLang.translate ("Cancel Assignment"), GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetKey = Key.None;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.EndVertical ();
		}

		internal static void DrawConfigKey(Key key) {
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format ("{0}: <color=#FFFFFF><b>{1}</b></color>", GetText (key), CurrentKey (key)), GUILayout.Width (350));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button (QLang.translate ("Set"), GUILayout.ExpandWidth (true), GUILayout.Height (20))) {
				SetKey = key;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
		}
	}
}