/* 
QuickContracts
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

using KSP.UI.Screens;
using System;
using UnityEngine;

namespace QuickContracts {
	public partial class QGUI {
		
		public static QuickContracts Instance {
			get;
			private set;
		}

		bool windowSettings;
		Rect rectButton = new Rect ();
		Rect rectSettings = new Rect ();
		string settingsTexturePath = relativePath + "/Textures/Settings";

		Texture2D settingsTexture {
			get {
				return GameDatabase.Instance.GetTexture (settingsTexturePath, false);
			}
		}

		static bool isMissionControl {
			get {
				return MissionControl.Instance != null;
			}
		}

		protected override void Awake() {
			if (Instance != null || HighLogic.CurrentGame.Mode != Game.Modes.CAREER) {
				Warning ("Destroy");
				Destroy (this);
				return;
			}
			Instance = this;
			string[] _keys = Enum.GetNames (typeof (Key));
			int _length = _keys.Length;
			for (int _key = 1; _key < _length; _key++) {
				Key _GetKey = (Key)_key;
				SetCurrentKey (_GetKey, DefaultKey (_GetKey));
			}
		}

		protected override void Start() {
			rectSetKey = new Rect ((Screen.width - 300) / 2, (Screen.height - 100) / 2, 300, 100);
			rectButton = new Rect (Screen.width - 150, 0, 40, 40);
			rectSettings = new Rect ((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300);
			VerifyKey ();
			GameEvents.onGUIMissionControlDespawn.Add (OnGUIMissionControlDespawn);
			GameEvents.Contract.onDeclined.Add (OnDeclined);
		}


		protected override void OnDestroy() {
			GameEvents.onGUIMissionControlDespawn.Remove (OnGUIMissionControlDespawn);
			GameEvents.Contract.onDeclined.Remove (OnDeclined);
		}

		void Lock(bool activate, ControlTypes Ctrl) {
			if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					EditorLogic.fetch.Lock(true, true, true, "EditorLock" + MOD);
				} else {
					EditorLogic.fetch.Unlock ("EditorLock" + MOD);
				}
			}
			if (activate) {
				InputLockManager.SetControlLock (Ctrl, "Lock" + MOD);
			} else {
				InputLockManager.RemoveControlLock ("Lock" + MOD);
			}
			if (InputLockManager.GetControlLock ("Lock" + MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("Lock" + MOD);
			}
			if (InputLockManager.GetControlLock ("EditorLock" + MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("EditorLock" + MOD);
			}
			Log ("Lock " + activate, "QGUI");
		}

		public void Settings() {
			windowSettings = !windowSettings;
			Lock (windowSettings, ControlTypes.KSC_ALL);
			if (!windowSettings) {
				QSettings.Instance.Save ();
			}
			Log ("Settings", "QGUI");
		}

		void Update() {
			if (MissionControl.Instance == null) {
				return;
			}
			if (SetKey != Key.None) {
				if (Event.current.isKey) {
					KeyCode _key = Event.current.keyCode;
					if (_key != KeyCode.None) {
						SetCurrentKey (SetKey, _key);
						SetKey = Key.None;
					}
				}
				return;
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyAcceptSelectedContract)) {
				Accept ();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyDeclineSelectedContract)) {
				Decline ();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyDeclineAllContracts)) {
				DeclineAll ();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyDeclineAllTest)) {
				DeclineAll (typeof (Contracts.Templates.PartTest));
			}
		}

		void OnGUI() {
			if (!isMissionControl) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			if (SetKey != Key.None) {
				rectSetKey = GUILayout.Window (1545146, rectSetKey, DrawSetKey, string.Format (QLang.translate("Set Key:") + " {0}", GetText (SetKey)), GUILayout.ExpandHeight (true));
				return;
			}
			if (windowSettings) {
				rectSettings = GUILayout.Window (1545147, rectSettings, DrawSettings, MOD + " " + VERSION, GUILayout.ExpandHeight (true));
				return;
			}
			GUILayout.BeginArea (rectButton);
			if (GUILayout.Button (new GUIContent (settingsTexture, "QuickContracts"), GUILayout.ExpandHeight (true), GUILayout.ExpandWidth (true))) {
				Settings ();
			}
			GUILayout.EndArea ();
		}

		void DrawSettings(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Shortcuts"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			DrawConfigKey (Key.AcceptSelectedContract);
			DrawConfigKey (Key.DeclineSelectedContract);
			DrawConfigKey (Key.DeclineAllContracts);
			DrawConfigKey (Key.DeclineAllTest);
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.EnableMessage = GUILayout.Toggle (QSettings.Instance.EnableMessage, QLang.translate ("Enable message for declined contracts"), GUILayout.Width (250));
			GUILayout.EndHorizontal ();
			QLang.DrawLang ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (QLang.translate ("Close"))) {
				Settings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}
	}
}