/* 
QuickContracts
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

using UnityEngine;

namespace QuickContracts {
	public partial class QGUI {
	
		Key SetKey = Key.None;
		Rect rectSetKey = new Rect();

		enum Key {
			None,
			DeclineSelectedContract,
			DeclineAllContracts,
			DeclineAllTest,
			AcceptSelectedContract
		}					
	
		KeyCode DefaultKey(Key key) {
			switch (key) {
			case Key.DeclineSelectedContract:
				return KeyCode.X;
			case Key.DeclineAllContracts:
				return KeyCode.C;
			case Key.DeclineAllTest:
				return KeyCode.V;
			case Key.AcceptSelectedContract:
				return KeyCode.A;
			}
			return KeyCode.None;
		}

		string GetText(Key key) {
			switch (key) {
			case Key.DeclineSelectedContract:
				return "Decline Selected Contract";
			case Key.DeclineAllContracts:
				return "Decline All Contracts";
			case Key.DeclineAllTest:
				return "Decline All Test";
			case Key.AcceptSelectedContract:
				return "Accept Selected Contract";
			}
			return string.Empty;
		}

		KeyCode CurrentKey(Key key) {
			switch (key) {
			case Key.DeclineSelectedContract:
				return QSettings.Instance.KeyDeclineSelectedContract;
			case Key.DeclineAllContracts:
				return QSettings.Instance.KeyDeclineAllContracts;
			case Key.DeclineAllTest:
				return QSettings.Instance.KeyDeclineAllTest;
			case Key.AcceptSelectedContract:
				return QSettings.Instance.KeyAcceptSelectedContract;
			}
			return KeyCode.None;
		}

		void VerifyKey(Key key) {
			try {
				Input.GetKey(CurrentKey(key));
			} catch {
				Warning ("Wrong key: " + CurrentKey(key), "QGUI");
				SetCurrentKey (key, DefaultKey(key));
			}
		}

		void VerifyKey() {
			VerifyKey (Key.DeclineSelectedContract);
			VerifyKey (Key.DeclineAllContracts);
			VerifyKey (Key.DeclineAllTest);
			VerifyKey (Key.AcceptSelectedContract);
		}

		void SetCurrentKey(Key key, KeyCode currentKey) {
			switch (key) {
			case Key.DeclineSelectedContract:
				QSettings.Instance.KeyDeclineSelectedContract = currentKey;
				break;
			case Key.DeclineAllContracts:
				QSettings.Instance.KeyDeclineAllContracts = currentKey;
				break;
			case Key.DeclineAllTest:
				QSettings.Instance.KeyDeclineAllTest = currentKey;
				break;
			case Key.AcceptSelectedContract:
				QSettings.Instance.KeyAcceptSelectedContract = currentKey;
				break;
			}
		}

		void DrawSetKey(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format ("Press a key to select the <color=#FFFFFF><b>{0}</b></color>", GetText (SetKey)));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Clear Assignment", GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, KeyCode.None);
				SetKey = Key.None;
				windowSettings = true;
			}
			if (GUILayout.Button ("Default Assignment", GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, DefaultKey (SetKey));
				SetKey = Key.None;
				windowSettings = true;
			}
			if (GUILayout.Button ("Cancel Assignment", GUILayout.ExpandWidth (true), GUILayout.Height (30))) {
				SetKey = Key.None;
				windowSettings = true;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.EndVertical ();
		}

		void DrawConfigKey(Key key) {
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format ("{0}: <color=#FFFFFF><b>{1}</b></color>", GetText (key), CurrentKey (key)), GUILayout.Width (250));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button ("Set", GUILayout.ExpandWidth (true), GUILayout.Height (20))) {
				SetKey = key;
				windowSettings = false;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
		}
	}
}