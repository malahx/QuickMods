/* 
QuickIVA
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

namespace QuickIVA {
	public partial class QGUI {

		internal static bool WindowSettings = false;
		static Rect RectSettings = new Rect();
		internal static QBlizzyToolbar BlizzyToolbar;

		protected override void Awake() {
			RectSettings = new Rect ((Screen.width - 515)/2, (Screen.height - 450)/2, 515, 450);
			if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzyToolbar ();
			Log ("Awake", "QGUI");
		}

		protected override void Start() {
			BlizzyToolbar.Init ();
			Log ("Start", "QGUI");
		}

		protected override void OnDestroy() {
			BlizzyToolbar.Destroy ();
			Log ("OnDestroy", "QGUI");
		}

		static void Lock(bool activate, ControlTypes Ctrl = ControlTypes.None) {
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
			Log ("Lock: " + activate, "QGUI");
		}

		public static void Settings() {
			SettingsSwitch ();
			if (!WindowSettings) {
				QSettings.Instance.Save ();
				BlizzyToolbar.Reset ();
				QStockToolbar.Instance.Reset ();
			}
		}

		internal static void SettingsSwitch() {
			WindowSettings = !WindowSettings;
			QStockToolbar.Instance.Set (WindowSettings);
			Lock (WindowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
			Log ("SettingsSwitch", "QGUI");
		}

		internal void OnGUI() {
			if (WindowSettings) {
				GUI.skin = HighLogic.Skin;
				RectSettings = GUILayout.Window (1584653, RectSettings, DrawSettings, MOD + " " + VERSION, GUILayout.Width (RectSettings.width), GUILayout.ExpandHeight(true));
			}
		}

		void DrawSettings(int id) {
			int _rect = 145;
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.Box("General Options",GUILayout.Height(30));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal ();
			QSettings.Instance.Enabled = GUILayout.Toggle (QSettings.Instance.Enabled, "Enable Automatic IVA", GUILayout.Width (275));
			QSettings.Instance.KeyEnabled = GUILayout.Toggle (QSettings.Instance.KeyEnabled, "Enable Keyboard shortcuts", GUILayout.Width (225));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			if (QBlizzyToolbar.isAvailable) {
				_rect += 39;
				GUILayout.BeginHorizontal ();
				QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, "Use the Stock ToolBar", GUILayout.Width (275));
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, "Use the Blizzy ToolBar", GUILayout.Width (225));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
			}
			if (QSettings.Instance.Enabled) {
				_rect += 140;
				GUILayout.BeginHorizontal();
				GUILayout.Box("IVA Options",GUILayout.Height(30));
				GUILayout.EndHorizontal();
				GUILayout.Space(5);
				GUILayout.BeginHorizontal ();
				QSettings.Instance.IVAatLaunch = GUILayout.Toggle (QSettings.Instance.IVAatLaunch, "IVA at Launch (if disabled, it will be IVA at the Loading)", GUILayout.Width (250));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QSettings.Instance.AutoHideUI = GUILayout.Toggle (QSettings.Instance.AutoHideUI, "Automatic Hide UI on IVA", GUILayout.Width (275));
				if (QSettings.Instance.AutoHideUI) {
					QSettings.Instance.DisableShowUIonIVA = GUILayout.Toggle (QSettings.Instance.DisableShowUIonIVA, "Disable UI on IVA", GUILayout.Width (225));
				} else {
					QSettings.Instance.DisableShowUIonIVA = false;
				}
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QSettings.Instance.DisableThirdPersonVessel = GUILayout.Toggle (QSettings.Instance.DisableThirdPersonVessel, "Disable 3rd person view on vessel", GUILayout.Width (275));
				if (QSettings.Instance.DisableThirdPersonVessel && QSettings.Instance.DisableShowUIonIVA) {
					QSettings.Instance.DisableMapView = GUILayout.Toggle (QSettings.Instance.DisableMapView, "Disable MAP View shortcut", GUILayout.Width (225));
				} else {
					QSettings.Instance.DisableMapView = false;
				}
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
			}
			if (QSettings.Instance.KeyEnabled) {
				_rect += 101;
				GUILayout.BeginHorizontal();
				GUILayout.Box("Keyboard Shortcuts",GUILayout.Height(30));
				GUILayout.EndHorizontal();
				GUILayout.Space(5);
				GUILayout.BeginHorizontal();
				GUILayout.Label ("Key to recovery: ", GUILayout.ExpandWidth(true));
				GUILayout.Space(5);
				QSettings.Instance.KeyRecovery = GUILayout.TextField (QSettings.Instance.KeyRecovery, GUILayout.Width (225));
				GUILayout.EndHorizontal();
				GUILayout.Space(5);
				GUILayout.BeginHorizontal();
				GUILayout.Label ("Key to EVA: ", GUILayout.ExpandWidth(true));
				GUILayout.Space(5);
				QSettings.Instance.KeyEVA = GUILayout.TextField (QSettings.Instance.KeyEVA, GUILayout.Width (225));
				GUILayout.EndHorizontal();
				GUILayout.Space(5);
			}
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("Close",GUILayout.Height(30))) {
				try {
					Input.GetKey(QSettings.Instance.KeyRecovery);
				} catch {
					Warning ("Wrong key: " + QSettings.Instance.KeyRecovery, "QGUI");
					QSettings.Instance.KeyRecovery = "end";
				}
				try {
					Input.GetKey(QSettings.Instance.KeyEVA);
				} catch {
					Warning ("Wrong key: " + QSettings.Instance.KeyEVA, "QGUI");
					QSettings.Instance.KeyEVA = "home";
				}
				Settings ();
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.EndVertical();
			RectSettings.height = _rect;
		}
	}
}