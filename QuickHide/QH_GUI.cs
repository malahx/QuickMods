/* 
QuickHide
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
using KSP.Localization;

using ClickThroughFix;

namespace QuickHide {

	public partial class QHide {
		
		public static QHide Instance;

		[KSPField (isPersistant = true)] internal QBlizzyToolbar BlizzyToolbar;

		internal bool WindowSettings = false;

		Coroutine coroutineMods;
		Rect rectSettings = new Rect (0, 0, 900, 0);
		internal Rect RectSettings {
			get {
				rectSettings.x = (Screen.width - rectSettings.width) / 2;
				rectSettings.y = (Screen.height - rectSettings.height) / 2;
				return rectSettings;
			}
			private set {
				rectSettings = value;
			}
		}

		GUIStyle LabelStyle;
		Vector2 scrollPosition = new Vector2 ();

		protected override void Awake() {
			if (Instance != null || !HighLogic.LoadedSceneIsGame) {
				Destroy (this);
				return;
			}
			Instance = this;
			if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzyToolbar ();
			GameEvents.onShowUI.Add (OnShowUI);
			GameEvents.onHideUI.Add (OnHideUI);
			GameEvents.onGUIAdministrationFacilitySpawn.Add (OnHideUI);
			GameEvents.onGUIMissionControlSpawn.Add (OnHideUI);
			GameEvents.onGUIRnDComplexSpawn.Add (OnHideUI);
			GameEvents.onGUIAstronautComplexSpawn.Add (OnHideUI);
			GameEvents.onGUIAdministrationFacilityDespawn.Add (OnShowUI);
			GameEvents.onGUIMissionControlDespawn.Add (OnShowUI);
			GameEvents.onGUIRnDComplexDespawn.Add (OnShowUI);
			GameEvents.onGUIAstronautComplexDespawn.Add (OnShowUI);
			GameEvents.onGameSceneLoadRequested.Add (OnGameSceneLoadRequested);
			Log ("Awake", "QHide");
		}

		protected override void Start() {
			BlizzyToolbar.Start ();
			coroutineMods = StartCoroutine (UpdateMods ());
			StartCoroutine (AfterAllAppAdded ());
			Log ("Start", "QHide");
		}

		protected override void OnDestroy() {
			if (BlizzyToolbar != null) {
				BlizzyToolbar.OnDestroy ();
			}
			GameEvents.onShowUI.Remove (OnShowUI);
			GameEvents.onHideUI.Remove (OnHideUI);
			GameEvents.onGUIAdministrationFacilitySpawn.Remove (OnHideUI);
			GameEvents.onGUIMissionControlSpawn.Remove (OnHideUI);
			GameEvents.onGUIRnDComplexSpawn.Remove (OnHideUI);
			GameEvents.onGUIAstronautComplexSpawn.Remove (OnHideUI);
			GameEvents.onGUIAdministrationFacilityDespawn.Remove (OnShowUI);
			GameEvents.onGUIMissionControlDespawn.Remove (OnShowUI);
			GameEvents.onGUIRnDComplexDespawn.Remove (OnShowUI);
			GameEvents.onGUIAstronautComplexDespawn.Remove (OnShowUI);
			GameEvents.onGameSceneLoadRequested.Remove (OnGameSceneLoadRequested);
			StopCoroutine (UpdateMods ());
			coroutineMods = null;
			Log ("OnDestroy", "QHide");
		}

		internal void RefreshStyle(bool force = false) {
			if (LabelStyle == null || force) {
				LabelStyle = new GUIStyle (GUI.skin.label);
				LabelStyle.stretchWidth = true;
				LabelStyle.stretchHeight = true;
				LabelStyle.alignment = TextAnchor.MiddleCenter;
				LabelStyle.fontStyle = FontStyle.Bold;
				LabelStyle.normal.textColor = Color.white;
				Log ("RefreshStyle " + force, "QHide");
			}
		}

		void Lock(bool activate, ControlTypes Ctrl) {
			if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					EditorLogic.fetch.Lock(true, true, true, "EditorLock" + QuickHide.MOD);
				} else {
					EditorLogic.fetch.Unlock ("EditorLock" + QuickHide.MOD);
				}
			}
			if (activate) {
				InputLockManager.SetControlLock (Ctrl, "Lock" + QuickHide.MOD);
			} else {
				InputLockManager.RemoveControlLock ("Lock" + QuickHide.MOD);
			}
			if (InputLockManager.GetControlLock ("Lock" + QuickHide.MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("Lock" + QuickHide.MOD);
			}
			if (InputLockManager.GetControlLock ("EditorLock" + QuickHide.MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("EditorLock" + QuickHide.MOD);
			}
			Log ("Lock " + activate, "QHide");
		}

		public void Settings() {
			ToggleSettings ();
			if (!WindowSettings) {
				QStockToolbar.Instance.Reset ();
				BlizzyToolbar.Reset ();
				QSettings.Instance.Save ();
			}
			Log ("Settings", "QHide");
		}

		internal void ToggleSettings() {
			WindowSettings = !WindowSettings;
			Lock (WindowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
			Log ("ToggleSettings", "QHide");
		}

		internal void OnGUI() {
			if (!WindowSettings) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			RefreshStyle ();
			RectSettings = ClickThruBlocker.GUILayoutWindow (1584654, RectSettings, DrawSettings, MOD + " " + VERSION, GUILayout.Width (RectSettings.width), GUILayout.ExpandHeight (true));
		}

		void DrawSettings(int id) {
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Box (Localizer.Format("quickhide_options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, Localizer.Format("quickhide_stockTB"), GUILayout.Width (400));
			GUILayout.FlexibleSpace ();
			if (QSettings.Instance.StockToolBar) {
				QSettings.Instance.StockToolBar_ModApp = !GUILayout.Toggle (!QSettings.Instance.StockToolBar_ModApp, Localizer.Format("quickhide_instock", MOD), GUILayout.Width (400));
			}
			GUILayout.FlexibleSpace ();
			if (QBlizzyToolbar.isAvailable) {
                QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, Localizer.Format("quickhide_blizzyTB"), GUILayout.Width (400));
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
            QSettings.Instance.HideAppLauncher = GUILayout.Toggle (QSettings.Instance.HideAppLauncher, Localizer.Format("quickhide_hideTB"), GUILayout.Width (450));
			GUILayout.FlexibleSpace ();
			QSettings.Instance.HideStage = GUILayout.Toggle (QSettings.Instance.HideStage, Localizer.Format("quickhide_hideStages"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();
			if (QSettings.Instance.HideAppLauncher) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label (Localizer.Format("quickhide_time"), GUILayout.Width (490));

                string timeToKeep = GUILayout.TextField(QSettings.Instance.TimeToKeep.ToString(), GUILayout.Width(100));
                if (timeToKeep != "")
                {
                    bool rc = int.TryParse(timeToKeep, out QSettings.Instance.TimeToKeep);
                    //QSettings.Instance.TimeToKeep = int.Parse(GUILayout.TextField(QSettings.Instance.TimeToKeep.ToString(), GUILayout.Width(100)));
                }
                else QSettings.Instance.TimeToKeep = 0;


                GUILayout.EndHorizontal ();
			}

			GUILayout.BeginHorizontal ();
			GUILayout.Box (Localizer.Format("quickhide_mods"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();

			scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.ExpandWidth(true), GUILayout.Height (300));
			DrawAppLauncherButtons ();
            GUILayout.EndScrollView();

			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
            if (GUILayout.Button (Localizer.Format("quickhide_close"), GUILayout.Height (30))) {
				Settings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical();
		}
	}
}