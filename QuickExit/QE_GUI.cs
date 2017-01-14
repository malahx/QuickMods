/* 
QuickExit
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
					_text = string.Format (QLang.translate ("Exit in {0}s{1}Push on {2} to abort the operation."), count, Environment.NewLine, QSettings.Instance.Key);
					if (needToSavegame) {
						if (!saveDone) {
							_text += string.Format ("{0}{1} {2}", Environment.NewLine, MOD, QLang.translate ("can't savegame, are you sure you want to exit?"));
						}
					}
				} else {
					_text = QLang.translate ("Exiting, bye ...");
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
			string _count = (QSettings.Instance.CountDown ? QLang.translate ("in") + " 5s" : ((needToSavegame && !CanSavegame) ? QLang.translate ("in") + " 10s" : QLang.translate ("now")));
			PopupDialog.SpawnPopupDialog (new Vector2 (0.5f, 0.5f), new Vector2 (0.5f, 0.5f), 
			    new MultiOptionDialog (QLang.translate ("Are you sure you want to exit KSP?"), MOD, HighLogic.UISkin, new DialogGUIBase[] {
				new DialogGUIButton (QLang.translate ("Oh noooo!"), () => HideSettings ()),
				new DialogGUIButton (QLang.translate ("Settings"), () => ShowSettings ()),
				new DialogGUIButton (string.Format ("{0}, {1}! ({2} + {3})", QLang.translate ("Exit"), _count, GameSettings.MODIFIER_KEY.primary.ToString (), QSettings.Instance.Key), () => TryExit (true))
				}), 
				true, HighLogic.UISkin);
			Log ("Dialog", "QExit");
		}

		void OnGUI() {
			if (WindowSettings) {
				GUI.skin = HighLogic.Skin;
				RectSettings = GUILayout.Window (1248597845, RectSettings, DrawSettings, MOD + " " + VERSION);
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
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, QLang.translate ("Use the Stock Toolbar"), GUILayout.Width(400));
			GUILayout.EndHorizontal();
			if (QSettings.Instance.StockToolBar) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (30);
				QSettings.Instance.StockToolBar_ModApp = !GUILayout.Toggle (!QSettings.Instance.StockToolBar_ModApp, QLang.translate ("Put QuickExit in Stock"), GUILayout.Width (370));
				GUILayout.EndHorizontal ();
			}
			if (QBlizzyToolbar.isAvailable) {
				GUILayout.BeginHorizontal();
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, QLang.translate ("Use the Blizzy Toolbar"), GUILayout.Width(400));
				GUILayout.EndHorizontal();
			}
			GUILayout.BeginHorizontal();
			QSettings.Instance.AutomaticSave = GUILayout.Toggle (QSettings.Instance.AutomaticSave, QLang.translate ("Automatic save before exit"), GUILayout.Width(400));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			QSettings.Instance.CountDown = GUILayout.Toggle (QSettings.Instance.CountDown, QLang.translate ("Count Down"), GUILayout.Width(400));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label (QLang.translate ("Key to exit") + ": ", GUILayout.ExpandWidth(true));
			QSettings.Instance.Key = GUILayout.TextField (QSettings.Instance.Key, GUILayout.Width (100));
			GUILayout.EndHorizontal();
			QLang.DrawLang ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (QLang.translate ("Exit"), GUILayout.Height(30))) {
				Settings();
				TryExit ();
			}
			if (GUILayout.Button (QLang.translate ("Close"), GUILayout.Height(30))) {
				try {
					Input.GetKey(QSettings.Instance.Key);
				} catch {
					Log ("Wrong key: " + QSettings.Instance.Key);
					QSettings.Instance.Key = "f7";
				}
				Settings ();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
	}
}