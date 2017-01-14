/* 
QuickScience
Copyright 2016 Malah

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
using System.Collections;
using UnityEngine;

namespace QuickScience {
	public class QKey : QuickScience {
	
		internal static Key SetKey = Key.None;

		internal enum Key {
			None,
			TestAll,
			CollectAll
		}					
	
		internal static KeyCode DefaultKey(Key key) {
			switch (key) {
				case Key.TestAll:
					return KeyCode.None;
				case Key.CollectAll:
					return KeyCode.None;
			}
			return KeyCode.None;
		}

		internal static bool isKeyDown(Key key) {
			return Input.GetKeyDown (CurrentKey (key));
		}

		internal static string GetText(Key key) {
			switch (key) {
				case Key.TestAll:
					return QLang.translate ("Test All");
				case Key.CollectAll:
					return QLang.translate ("Collect All");
			}
			return string.Empty;
		}

		internal static KeyCode CurrentKey(Key key) {
			switch (key) {
				case Key.TestAll:
					return QSettings.Instance.KeyTestAll;
				case Key.CollectAll:
					return QSettings.Instance.KeyCollectAll;
			}
			return KeyCode.None;
		}

		internal static void VerifyKey(Key key) {
			try {
				Input.GetKey(CurrentKey(key));
			} catch {
				Warning ("Wrong key: " + CurrentKey(key), "QKey");
				SetCurrentKey (key, DefaultKey(key));
			}
		}

		internal static void VerifyKey() {
			string[] _keys = Enum.GetNames (typeof (Key));
			int _length = _keys.Length;
			for (int _key = 1; _key < _length; _key++) {
				Key _getKey = (Key)_key;
				VerifyKey (_getKey);
			}
			Log ("VerifyKey", "QKey");
		}

		internal static void SetCurrentKey(Key key, KeyCode currentKey) {
			switch (key) {
				case Key.TestAll:
					QSettings.Instance.KeyTestAll = currentKey;
					break;
				case Key.CollectAll:
					QSettings.Instance.KeyCollectAll = currentKey;
					break;
			}
			Log (string.Format("SetCurrentKey({0}): {1}", GetText(key), currentKey), "QKey");
		}

		internal static void DrawKeys() {
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Keyboard shortcuts"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			string[] _keys = Enum.GetNames (typeof (Key));
			int _length = _keys.Length;
			for (int _key = 1; _key < _length; _key++) {
				Key _getKey = (Key)_key;
				DrawConfigKey (_getKey);
			}
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

		static void DrawConfigKey(Key key) {
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