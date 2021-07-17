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
using KSP.Localization;
using UnityEngine;

using ClickThroughFix;

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
                    _text = Localizer.Format("quickexit_exitIn", count) + Environment.NewLine + Localizer.Format("quickexit_abort", QSettings.Instance.Key);
					if (needToSavegame) {
						if (!saveDone) {
							_text += Environment.NewLine + Localizer.Format("quickexit_cantSave", RegisterToolbar.MOD) + " " + Localizer.Format("quickexit_sureExit");
						}
					}
				} else {
					_text = Localizer.Format("quickexit_exiting");
				}
				return _text;
			}
		}

		void Lock(bool activate, ControlTypes Ctrl) {
			if (HighLogic.LoadedSceneIsFlight) {
				FlightDriver.SetPause (activate);
				if (activate) {
					InputLockManager.SetControlLock (ControlTypes.CAMERACONTROLS | ControlTypes.MAP, "Lock" + RegisterToolbar.MOD);
					return;
				}
			} else if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					EditorLogic.fetch.Lock(true, true, true, "EditorLock" + RegisterToolbar.MOD);
					return;
				} else {
					EditorLogic.fetch.Unlock ("EditorLock" + RegisterToolbar.MOD);
				}
			}
			if (activate) {
				InputLockManager.SetControlLock (Ctrl, "Lock" + RegisterToolbar.MOD);
				return;
			} else {
				InputLockManager.RemoveControlLock ("Lock" + RegisterToolbar.MOD);
			}
			if (InputLockManager.GetControlLock ("Lock" + RegisterToolbar.MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("Lock" + RegisterToolbar.MOD);
			}
			if (InputLockManager.GetControlLock ("EditorLock" + RegisterToolbar.MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("EditorLock" + RegisterToolbar.MOD);
			}
			Log ("Lock " + activate, "QExit");
		}

		void Settings() {
			SettingsSwitch ();
			if (!WindowSettings) {
				QStockToolbar.Instance.Reset ();
				//QExit.BlizzyToolbar.Reset ();
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
			string _count = (QSettings.Instance.CountDown ? Localizer.Format("quickexit_in", 5) : ((needToSavegame && !CanSavegame) ? Localizer.Format("quickexit_in", 10) : Localizer.Format("quickexit_now")));
			PopupDialog.SpawnPopupDialog (
                new Vector2 (0.5f, 0.5f), 
                new Vector2 (0.5f, 0.5f),
                new MultiOptionDialog(RegisterToolbar.MOD, Localizer.Format("quickexit_sureExit"), RegisterToolbar.MOD, HighLogic.UISkin,
                                       new DialogGUIBase[] {
                                        new DialogGUIButton (Localizer.Format("quickexit_ohNo"), () => HideSettings ()),
                                        new DialogGUIButton (Localizer.Format("quickexit_settings"), () => ShowSettings ()),
                                        new DialogGUIButton (string.Format ("{0}, {1}! ({2} + {3})", Localizer.Format("quickexit_exit"), _count, GameSettings.MODIFIER_KEY.primary.ToString (), QSettings.Instance.Key), () => TryExit (true))
                }),
                true, HighLogic.UISkin);
			Log ("Dialog", "QExit");
		}

		void OnGUI() {
			if (WindowSettings) {
				GUI.skin = HighLogic.Skin;
				RectSettings = ClickThruBlocker.GUILayoutWindow (1248597845, RectSettings, DrawSettings, RegisterToolbar.MOD + " " + RegisterToolbar.VERSION);
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
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, Localizer.Format("quickexit_stockTB"), GUILayout.Width(400));
			GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
			QSettings.Instance.AutomaticSave = GUILayout.Toggle (QSettings.Instance.AutomaticSave, Localizer.Format("quickexit_automaticSave"), GUILayout.Width(400));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			QSettings.Instance.CountDown = GUILayout.Toggle (QSettings.Instance.CountDown, Localizer.Format("quickexit_count"), GUILayout.Width(400));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label (Localizer.Format("quickexit_key"), GUILayout.ExpandWidth(true));
			QSettings.Instance.Key = GUILayout.TextField (QSettings.Instance.Key, GUILayout.Width (100));
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (Localizer.Format("quickexit_exit"), GUILayout.Height(30))) {
				Settings();
				TryExit ();
			}
			if (GUILayout.Button (Localizer.Format("quickexit_close"), GUILayout.Height(30))) {
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