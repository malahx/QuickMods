/* 
QuickExit
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
using UnityEngine;

namespace QuickExit {

	public partial class QExit {

		GUIStyle labelStyle;
		
		bool WindowSettings = false;

		Rect rectSettings = new Rect();
		Rect RectSettings {
			get {
				rectSettings.x = (Screen.width - rectSettings.width) / 2;
				rectSettings.y = (Screen.height - rectSettings.height) / 2;
				return rectSettings;
			}
			set {
				rectSettings = value;
			}
		}

		string exitText {
			get {
				string _text = string.Empty;
				if (count > 0) {
					_text = string.Format ("Exit in {0}s{1}Push on {2} to abort the operation.", count, Environment.NewLine, QSettings.Instance.Key);
					if (needToSavegame) {
						if (!saveDone) {
							_text += string.Format ("{0}{1} can't savegame, are you sure you want to exit?", Environment.NewLine, MOD);
						}
					}
				} else {
					_text = "Exiting, bye ...";
				}
				return _text;
			}
		}

		void Lock(bool activate, ControlTypes Ctrl) {
			if (HighLogic.LoadedSceneIsFlight) {
				FlightDriver.SetPause (activate);
				if (activate) {
					InputLockManager.SetControlLock (ControlTypes.CAMERACONTROLS | ControlTypes.MAP, "Lock" + MOD);
					return;
				}
			} else if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					EditorLogic.fetch.Lock(true, true, true, "EditorLock" + MOD);
					return;
				} else {
					EditorLogic.fetch.Unlock ("EditorLock" + MOD);
				}
			}
			if (activate) {
				InputLockManager.SetControlLock (Ctrl, "Lock" + MOD);
				return;
			} else {
				InputLockManager.RemoveControlLock ("Lock" + MOD);
			}
			if (InputLockManager.GetControlLock ("Lock" + MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("Lock" + MOD);
			}
			if (InputLockManager.GetControlLock ("EditorLock" + MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("EditorLock" + MOD);
			}
			Log ("Lock " + activate, "QExit");
		}

		void Settings() {
			SettingsSwitch ();
			if (!WindowSettings) {
				QStockToolbar.Instance.Reset ();
				QExit.BlizzyToolbar.Reset ();
				QSettings.Instance.Save ();
			}
			Log ("Settings", "QExit");
		}

		void SettingsSwitch() {
			WindowSettings = !WindowSettings;
			Lock (WindowSettings, ControlTypes.All);
			Log ("SettingsSwitch", "QExit");
		}

		void ShowSettings() {
			WindowSettings = true;
			Lock (WindowSettings, ControlTypes.All);
			Log ("ShowSettings", "QExit");
		}

		void HideSettings() {
			WindowSettings = false;
			Lock (WindowSettings, ControlTypes.All);
			IsTryExit = false;
			Log ("HideSettings", "QExit");
		}

		public void Dialog() {
			if (QStockToolbar.Instance != null) QStockToolbar.Instance.Set (false);
			string _count = (QSettings.Instance.CountDown ? "in 5s" : ((needToSavegame && !CanSavegame) ? "in 10s" : "now"));
			PopupDialog.SpawnPopupDialog (new Vector2 (0.5f, 0.5f), new Vector2 (0.5f, 0.5f), 
				new MultiOptionDialog ("Are you sure you want to exit KSP?", MOD, HighLogic.UISkin, new DialogGUIBase[] {
					new DialogGUIButton ("Oh noooo!", () => HideSettings ()),
					new DialogGUIButton ("Configurations!", () => ShowSettings ()),
					new DialogGUIButton (string.Format ("Exit, {0}! ({1} + {2})", _count, GameSettings.MODIFIER_KEY.primary.ToString (), QSettings.Instance.Key), () => TryExit (true))
				}), 
				true, HighLogic.UISkin);
			Log ("Dialog", "QExit");
		}

		void OnGUI() {
			if (WindowSettings) {
				GUI.skin = HighLogic.Skin;
				RectSettings = GUILayout.Window (1248597845, RectSettings, DrawSettings, MOD + VERSION, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			} 
			if (IsTryExit) {
				GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height), labelStyle);
				GUILayout.Label (exitText, labelStyle);
				GUILayout.EndArea ();
			}
		}

		void DrawSettings(int id) {
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, "Use the Stock ToolBar", GUILayout.Width(210));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			if (QSettings.Instance.StockToolBar) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30);
				QSettings.Instance.StockToolBar_ModApp = !GUILayout.Toggle (!QSettings.Instance.StockToolBar_ModApp, "Put QuickExit in Stock", GUILayout.Width (180));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
			}
			if (QBlizzyToolbar.isAvailable) {
				GUILayout.BeginHorizontal();
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, "Use the Blizzy ToolBar", GUILayout.Width(210));
				GUILayout.EndHorizontal();
				GUILayout.Space(5);
			}
			GUILayout.BeginHorizontal();
			QSettings.Instance.AutomaticSave = GUILayout.Toggle (QSettings.Instance.AutomaticSave, "Automatic save before exit", GUILayout.Width(210));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			QSettings.Instance.CountDown = GUILayout.Toggle (QSettings.Instance.CountDown, "Count Down", GUILayout.Width(210));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Key to exit: ", GUILayout.ExpandWidth(true));
			GUILayout.Space(5);
			QSettings.Instance.Key = GUILayout.TextField (QSettings.Instance.Key, GUILayout.Width (100));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("Exit", GUILayout.Width(40), GUILayout.Height(30))) {
				Settings();
				TryExit ();
			}
			GUILayout.Space(5);
			if (GUILayout.Button ("Close", GUILayout.ExpandWidth(true) ,GUILayout.Height(30))) {
				try {
					Input.GetKey(QSettings.Instance.Key);
				} catch {
					QuickExit.Log ("Wrong key: " + QSettings.Instance.Key);
					QSettings.Instance.Key = "f7";
				}
				Settings ();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.EndVertical();
		}
	}
}