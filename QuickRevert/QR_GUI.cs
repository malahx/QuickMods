/* 
QuickRevert
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

namespace QuickRevert {

	public partial class QGUI {

		public static QGUI Instance;

		//[KSPField(isPersistant = true)] private static QBlizzyToolbar BlizzyToolbar;

		bool WindowSettings = false;
		Rect rectSettings = new Rect (0, 0, 0, 0);
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

		protected override void Awake() {
			if (HighLogic.LoadedScene != GameScenes.SPACECENTER) {
				Warning ("QGUI needs to be load only in the SpaceCenter.", "QGUI");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				Warning ("There's already an Instance of QGUI", "QGUI");
				Destroy (this);
				return;
			}
			Instance = this;
			//if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzyToolbar ();
			Log ("Awake", "QGUI");
		}

		protected override void Start() {
			if (!QFlight.data.hasLoaded) {
				QFlight.data.Load ();
			}
			Log ("Start", "QGUI");
		}

		protected override void OnDestroy() {
			Log ("OnDestroy", "QGUI");
            Instance = null;
		}

		void Lock(bool activate, ControlTypes Ctrl) {
			if (HighLogic.LoadedSceneIsEditor) {
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
			Log ("Lock " + activate, "QGUI");
		}

		public void Settings() {
			SettingsSwitch ();
			if (!WindowSettings) {
				QStockToolbar.Instance.Reset();
				//BlizzyToolbar.Reset();
				QSettings.Instance.Save ();
			}
			Log ("Settings", "QGUI");
		}

		private void SettingsSwitch() {
			WindowSettings = !WindowSettings;
			QStockToolbar.Instance.Set (WindowSettings);
			Lock (WindowSettings, ControlTypes.KSC_ALL);
			Log ("SettingsSwitch", "QGUI");
		}

		void OnGUI() {
			if (!WindowSettings) {
				return;
			}
			GUI.skin = HighLogic.Skin;
            RectSettings = ClickThruBlocker.GUILayoutWindow(1584652, RectSettings, DrawSettings, RegisterToolbar.MOD  + " " + RegisterToolbar.VERSION);
		}

		void DrawSettings(int id) {
            Debug.Log("DrawSettings 1");

            GUILayout.BeginVertical();
		
			if (QFlight.data.PostInitStateIsSaved) {
				if (QFlight.data.VesselExists) {
					GUILayout.BeginHorizontal();
					GUILayout.Box("Revert Saved",GUILayout.Height(30));
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					GUILayout.Label (Localizer.Format("quickrevert_revertLastV", QFlight.data.pVessel.vesselType, QFlight.data.pVessel.vesselName), GUILayout.Width (400));
                    Debug.Log("DrawSettings 2");
                    //					GUILayout.FlexibleSpace ();
                    if (GUILayout.Button (Localizer.Format("quickrevert_loseIt"))) {
						QFlight.data.Reset ();
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal ();
                    Debug.Log("DrawSettings 3");

                    //					GUILayout.FlexibleSpace ();
                    if (GUILayout.Button (Localizer.Format("quickrevert_gotoV"))) {
						Settings ();
						string _saveGame = GamePersistence.SaveGame ("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
						FlightDriver.StartAndFocusVessel (_saveGame, QFlight.data.currentActiveVesselIdx);
						InputLockManager.ClearControlLocks ();
					}
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);
					if (!QFlight.data.PreLaunchStateIsSaved) {
						GUILayout.BeginHorizontal ();
						GUILayout.Label (Localizer.Format("quickrevert_onlyRevertLaunch"), GUILayout.Width (500));
						GUILayout.EndHorizontal ();
					}
				}
			}

			GUILayout.BeginHorizontal();
			GUILayout.Box("Options",GUILayout.Height(30));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
            QSettings.Instance.EnableRevertLoss = GUILayout.Toggle (QSettings.Instance.EnableRevertLoss, Planetarium.fetch.Home.atmosphere ? Localizer.Format("quickrevert_revertLossAtm") : Localizer.Format("quickrevert_revertLossSOI"), GUILayout.Width (450));
			GUILayout.EndHorizontal();

            Debug.Log("DrawSettings 4");

            //			GUILayout.FlexibleSpace ();
            GUILayout.BeginHorizontal ();
            Debug.Log("DrawSettings 5");

            //			GUILayout.FlexibleSpace ();
            if (GUILayout.Button (Localizer.Format("quickrevert_close"), GUILayout.Height(30))) {
				Settings ();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
            Debug.Log("DrawSettings 6");
		}
        
	}
}