/* 
QuickGoTo
Copyright 2015 Malah

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

using KSP.UI;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuickGoTo {

	public partial class QGoTo {

		public static QGoTo Instance;

		public static bool Ready = false;

		[KSPField(isPersistant = true)] internal static QBlizzyToolbar BlizzyToolbar;

		GUISkin guiSkin;
		Color guiColor = new Color();
		GUIStyle GoToButtonStyle;
		GUIStyle LabelStyle;
		GUIStyle GoToWindowStyle;
		string VAB_TexturePath = MOD + "/Textures/StockVAB";
		string TS_TexturePath = MOD + "/Textures/StockTS";
		string SPH_TexturePath = MOD + "/Textures/StockSPH";
		string Sett_TexturePath = MOD + "/Textures/StockSett";
		string SC_TexturePath = MOD + "/Textures/StockSC";
		string RvSC_TexturePath = MOD + "/Textures/StockRvSC";
		string RvED_TexturePath = MOD + "/Textures/StockRvED";
		string Rv_TexturePath = MOD + "/Textures/StockRv";
		string RnD_TexturePath = MOD + "/Textures/StockRnD";
		string Rc_TexturePath = MOD + "/Textures/StockRc";
		string MI_TexturePath = MOD + "/Textures/StockMI";
		string Main_TexturePath = MOD + "/Textures/StockMain";
		string Lves_TexturePath = MOD + "/Textures/StockLves";
		string Astr_TexturePath = MOD + "/Textures/StockAstr";
		string Admi_TexturePath = MOD + "/Textures/StockAdmi";
		string Conf_TexturePath = MOD + "/Textures/StockConf";

		Texture2D VAB_Texture;
		Texture2D TS_Texture;
		Texture2D SPH_Texture;
		Texture2D Sett_Texture;
		Texture2D SC_Texture;
		Texture2D RvSC_Texture;
		Texture2D RvED_Texture;
		Texture2D Rv_Texture;
		Texture2D RnD_Texture;
		Texture2D Rc_Texture;
		Texture2D MI_Texture;
		Texture2D Main_Texture;
		Texture2D Lves_Texture;
		Texture2D Astr_Texture;
		Texture2D Admi_Texture;
		Texture2D Conf_Texture;

		bool windowGoTo = false;
		public bool WindowGoTo {
			get {
				return windowGoTo || (keepGoTo && QSettings.Instance.StockToolBar_OnHover);
			}
			set {
				if (QStockToolbar.Instance.isActive && QSettings.Instance.StockToolBar_OnHover && windowGoTo && !value) {
					keepGoTo = true;
					keepDate = DateTime.Now;
				}
				windowGoTo = value;
			}
		}

		public bool WindowSettings = false;
		bool keepGoTo = false;
		DateTime keepDate = DateTime.Now;

		Rect rectSettings = new Rect ();
		public Rect RectSettings {
			get {
				rectSettings.width = 915;
				rectSettings.x = (Screen.width - rectSettings.width) / 2;
				rectSettings.y = (Screen.height - rectSettings.height) / 2;
				return rectSettings;
			}
			set {
				rectSettings = value;
			}
		}
		Rect rectGoTo = new Rect ();
		public Rect RectGoTo {
			get {
				rectGoTo.x = (Screen.width - rectGoTo.width) / 2;
				rectGoTo.y = (Screen.height - rectGoTo.height) / 2;
				if (QStockToolbar.Enabled) {
					Rect _activeButtonPos;
					if (QStockToolbar.Instance.isActive) {
						_activeButtonPos = QStockToolbar.Instance.Position;
						if (ApplicationLauncher.Instance.IsPositionedAtTop) {
							rectGoTo.x = _activeButtonPos.x - rectGoTo.width;
							if (QSettings.Instance.CenterText) {
								rectGoTo.y = _activeButtonPos.y + _activeButtonPos.height / 2 - rectGoTo.height / 2;
							} else {
								rectGoTo.y = _activeButtonPos.y;
							}
						} else {
							if (QSettings.Instance.ImageOnly || QSettings.Instance.CenterText) {
								rectGoTo.x = _activeButtonPos.x + _activeButtonPos.width / 2 - rectGoTo.width / 2;
							} else {
								rectGoTo.x = _activeButtonPos.x + _activeButtonPos.width - rectGoTo.width;
							}
							rectGoTo.y = _activeButtonPos.y - rectGoTo.height;
						}
					} else if (needButton) {
						_activeButtonPos = RectButton;
						if (QSettings.Instance.ImageOnly || QSettings.Instance.CenterText) {
							rectGoTo.x = _activeButtonPos.x + _activeButtonPos.width / 2 - rectGoTo.width / 2;
						} else {
							rectGoTo.x = _activeButtonPos.x + _activeButtonPos.width - rectGoTo.width;
						}
						rectGoTo.y = _activeButtonPos.y + _activeButtonPos.height;
					} else {
						return rectGoTo;
					}
					if (rectGoTo.x + rectGoTo.width > Screen.width) {
						rectGoTo.x = Screen.width - rectGoTo.width;
					}
					if (rectGoTo.y < 0) {
						rectGoTo.y = 0;
					}
				}
				return rectGoTo;
			}
			set {
				rectGoTo = value;
			}
		}

		Rect rectButton = new Rect (Screen.width - 90, 0, 40, 40);
		public Rect RectButton {
			get {
				rectButton.x = Screen.width - 90;
				return rectButton;
			}
			set {
				rectButton = value;
			}
		}

		int GoToButtonHeight {
			get {
				return (QSettings.Instance.ImageOnly ? 40 : 30);
			}
		}

		bool needButton {
			get {
				if (!QSettings.Instance.EnableBatButton) {
					return false;
				}
				if (QStockToolbar.Instance != null) {
					if (QStockToolbar.Instance.isActive) {
						return false;
					}
				}
				return isAdministration || isAstronautComplex || isMissionControl || isRnD;
			}
		}

		bool isHide {
			get {
				return UIMasterController.Instance.uiCamera != null ? !UIMasterController.Instance.uiCamera.enabled : true && 
					!UIMasterController.Instance.mainCanvas.enabled &&
					!UIMasterController.Instance.appCanvas.enabled &&
					!UIMasterController.Instance.actionCanvas.enabled &&
					!UIMasterController.Instance.dialogCanvas.enabled &&
					!UIMasterController.Instance.tooltipCanvas.enabled;
			}
		}

		protected override void Awake() {
			if (!HighLogic.LoadedSceneIsGame) {
				Warning ("It needs to be a game. Destroy.", "QGUI");
				Destroy (this);
				return;
			}
			if (Instance != null) {
				Warning ("There's already an Instance of " + MOD + ". Destroy.", "QGUI");
				Destroy (this);
				return;
			}
			Instance = this;
			if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzyToolbar ();
			//GameEvents.onGUILaunchScreenSpawn.Add (OnGUILaunchScreenSpawn);
			//GameEvents.onGUILaunchScreenDespawn.Add (OnGUILaunchScreenDespawn);
			GameEvents.onGameSceneLoadRequested.Add (OnGameSceneLoadRequested);
			GameEvents.onGUIAstronautComplexSpawn.Add (AstronautComplexSpawn);
			GameEvents.onGUIAstronautComplexDespawn.Add (AstronautComplexDespawn);
			GameEvents.onGUIRnDComplexSpawn.Add (RnDComplexSpawn);
			GameEvents.onGUIRnDComplexDespawn.Add (RnDComplexDespawn);
			GameEvents.onFlightReady.Add (OnFlightReady);
			Log ("Awake", "QGUI");
		}

		protected override void Start() {
			if (HighLogic.LoadedScene == GameScenes.SPACECENTER) {
				StartCoroutine (PostInit ());
			}
			if (HighLogic.LoadedScene == GameScenes.MAINMENU) {
				LastVessels = new List<QData>();
			}
			if (BlizzyToolbar != null) {
				BlizzyToolbar.Init ();
			}
			RefreshTexture ();
			RefreshStyle (true);
			Ready = true;
			Log ("Start", "QGUI");
		}
			
		protected override void OnDestroy() {
			if (BlizzyToolbar != null) BlizzyToolbar.Destroy ();
			//GameEvents.onGUILaunchScreenSpawn.Remove (OnGUILaunchScreenSpawn);
			//GameEvents.onGUILaunchScreenDespawn.Remove (OnGUILaunchScreenDespawn);
			GameEvents.onGameSceneLoadRequested.Remove (OnGameSceneLoadRequested);
			GameEvents.onGUIAstronautComplexSpawn.Remove (AstronautComplexSpawn);
			GameEvents.onGUIAstronautComplexDespawn.Remove (AstronautComplexDespawn);
			GameEvents.onGUIRnDComplexSpawn.Remove (RnDComplexSpawn);
			GameEvents.onGUIRnDComplexDespawn.Remove (RnDComplexDespawn);
			GameEvents.onFlightReady.Remove (OnFlightReady);
			Log ("OnDestroy", "QGUI");
		}

		void OnFlightReady() {
			Vessel _vessel = FlightGlobals.ActiveVessel;
			if (_vessel != null) {
				ProtoVessel _pVessel = _vessel.protoVessel;
				if (_pVessel != null) {
					int _index;
					QData _QData = LastVesselLastIndex (out _index);
					if (_QData != null) {
						ProtoVessel _QDataPVessel = _QData.protoVessel;
						if (_QDataPVessel != null) {
							if (_QDataPVessel == _pVessel) {
								LastVessels.RemoveAt (_index);
								Warning ("Remove the current vessel from the last Vessels: " + _pVessel.vesselName);
							}
						}
					}
				}
			}
			Log ("OnFlightReady", "QGUI");
		}

		void OnGameSceneLoadRequested(GameScenes gameScenes) {
			if (HighLogic.LoadedSceneIsFlight) {
				Vessel _vessel = FlightGlobals.ActiveVessel;
				if (_vessel != null) {
					AddLastVessel(_vessel.protoVessel);
				}
			}
			HideGoTo (true);
			Log ("OnGameSceneLoadRequested", "QGUI");
		}

		void AstronautComplexSpawn() {
			isAstronautComplex = true;
			Log ("AstronautComplexSpawn", "QGUI");
		}

		void AstronautComplexDespawn() {
			isAstronautComplex = false;
			Log ("AstronautComplexDespawn", "QGUI");
		}

		void RnDComplexSpawn() {
			isRnD = true;
			Log ("RnDComplexSpawn", "QGUI");
		}

		void RnDComplexDespawn() {
			isRnD = false;
			Log ("RnDComplexDespawn", "QGUI");
		}

		/*private void OnGUILaunchScreenSpawn(GameEvents.VesselSpawnInfo vSpawnInfo) {
			isLaunchScreen = true;
		}

		private void OnGUILaunchScreenDespawn() {
			isLaunchScreen = false;
		}*/

		internal void RefreshTexture() {
			bool _ImageOnly = QSettings.Instance.ImageOnly;
			VAB_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? VAB_TexturePath : QBlizzyToolbar.VAB_TexturePath), false);
			TS_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? TS_TexturePath : QBlizzyToolbar.TS_TexturePath), false);
			SPH_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? SPH_TexturePath : QBlizzyToolbar.SPH_TexturePath), false);
			Sett_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Sett_TexturePath : QBlizzyToolbar.Sett_TexturePath), false);
			SC_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? SC_TexturePath : QBlizzyToolbar.SC_TexturePath), false);
			RvSC_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? RvSC_TexturePath : QBlizzyToolbar.RvSC_TexturePath), false);
			RvED_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? RvED_TexturePath : QBlizzyToolbar.RvED_TexturePath), false);
			Rv_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Rv_TexturePath : QBlizzyToolbar.Rv_TexturePath), false);
			RnD_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? RnD_TexturePath : QBlizzyToolbar.RnD_TexturePath), false);
			Rc_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Rc_TexturePath : QBlizzyToolbar.Rc_TexturePath), false);
			MI_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? MI_TexturePath : QBlizzyToolbar.MI_TexturePath), false);
			Main_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Main_TexturePath : QBlizzyToolbar.Main_TexturePath), false);
			Lves_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Lves_TexturePath : QBlizzyToolbar.Lves_TexturePath), false);
			Astr_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Astr_TexturePath : QBlizzyToolbar.Astr_TexturePath), false);
			Admi_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Admi_TexturePath : QBlizzyToolbar.Admi_TexturePath), false);
			Conf_Texture = GameDatabase.Instance.GetTexture((_ImageOnly ? Conf_TexturePath : QBlizzyToolbar.Conf_TexturePath), false);
			Log ("RefreshTexture", "QGUI");
		}

		internal void RefreshStyle(bool force = false) {
			if (force) {
				guiSkin = (QSettings.Instance.KSPSkin ? HighLogic.Skin : null);
			}
			if (GoToWindowStyle == null || force) {
				GoToWindowStyle = new GUIStyle (guiSkin.window);
				GoToWindowStyle.padding = new RectOffset (); 
			}
			if (GoToButtonStyle == null || force) {
				GoToButtonStyle = new GUIStyle (guiSkin.button);
				GoToButtonStyle.alignment = (QSettings.Instance.ImageOnly ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft);
				GoToButtonStyle.padding = new RectOffset ((QSettings.Instance.ImageOnly ? 0 : 10), 0, 0, 0);
				GoToButtonStyle.imagePosition = (QSettings.Instance.ImageOnly ? ImagePosition.ImageOnly : ImagePosition.ImageLeft);
			}
			if (LabelStyle == null || force) {
				LabelStyle = new GUIStyle (guiSkin.label);
				LabelStyle.stretchWidth = true;
				LabelStyle.stretchHeight = true;
				LabelStyle.alignment = TextAnchor.MiddleCenter;
				LabelStyle.fontStyle = FontStyle.Bold;
				LabelStyle.normal.textColor = Color.white;
			}
			Log ("RefreshStyle force: " + force, "QGUI");
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
			SettingsSwitch ();
			if (!WindowSettings) {
				QStockToolbar.Instance.Reset ();
				BlizzyToolbar.Reset ();
				QSettings.Instance.Save ();
				RefreshStyle (true);
				RefreshTexture ();
			}
			Log ("Settings", "QGUI");
		}

		internal void SettingsSwitch() {
			WindowSettings = !WindowSettings;
			QStockToolbar.Instance.Set (WindowSettings);
			Lock (WindowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI);
			HideGoTo (true);
			Log ("SettingsSwitch", "QGUI");
		}

		internal void ToggleGoTo() {
			WindowGoTo = !WindowGoTo;
			QStockToolbar.Instance.Set (WindowGoTo);
			Lock (WindowGoTo, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI);
			Log ("ToggleGoTo", "QGUI");
		}

		internal void HideGoTo(bool force = false) {
			if (!WindowGoTo) {
				return;
			}
			if (force) {
				QStockToolbar.Instance.Set (false);
				windowGoTo = false;
				keepGoTo = false;
			} else {
				WindowGoTo = false;
			}
			Lock (WindowGoTo, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI);
			Log ("HideGoTo force: " + force, "QGUI");
		}

		internal void ShowGoTo() {
			if (windowGoTo) {
				return;
			}
			WindowGoTo = true;
			Lock (WindowGoTo, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI);
			Log ("ShowGoTo", "QGUI");
		}

		void OnGUI() {
			if (!HighLogic.LoadedSceneIsGame || !Ready || (isHide && !needButton)) {
				return;
			}
			GUI.skin = guiSkin;
			if (needButton) {
				DrawButton (RectButton);
			}
			if (WindowSettings) {
				Rect _rect = RectSettings;
				RectSettings = GUILayout.Window (1584654, _rect, DrawSettings, MOD + " " + VERSION, GUILayout.Width (_rect.width), GUILayout.ExpandHeight (true));
			}
			if (WindowGoTo) {
				if (QStockToolbar.Instance.isActive) {
					if (!QStockToolbar.Instance.isTrue && !QStockToolbar.Instance.isHovering && !keepGoTo) {
						HideGoTo ();
						return;
					}
				}
				GUI.color = guiColor;
				RectGoTo = GUILayout.Window (1584664, RectGoTo, DrawGoTo, "GoTo", GoToWindowStyle);
				if (keepGoTo) {
					if ((DateTime.Now - keepDate).TotalSeconds > 1) {
						keepGoTo = false;
					}
					if (QStockToolbar.Instance.isHovering) {
						keepDate = DateTime.Now;
					}
				}
			}
		}

		void DrawSettings(int id) {
			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();
			GUILayout.Box("Options",GUILayout.Height(30));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, "Use the Stock ToolBar", GUILayout.Width (300));
			if (QSettings.Instance.StockToolBar) {
				QSettings.Instance.StockToolBar_ModApp = !GUILayout.Toggle (!QSettings.Instance.StockToolBar_ModApp, "Put QuickGoTo in Stock", GUILayout.Width (300));
			} else {
				GUILayout.Space (300);
			}
			if (QBlizzyToolbar.isAvailable) {
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, "Use the Blizzy ToolBar", GUILayout.Width (300));
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal (); 
			QSettings.Instance.EnableBatButton = GUILayout.Toggle (QSettings.Instance.EnableBatButton, "Enable button in the batiments", GUILayout.Width (300));
			bool _bool = GUILayout.Toggle (QSettings.Instance.ImageOnly, "Panel with only the icons", GUILayout.Width (300));
			if (_bool != QSettings.Instance.ImageOnly) {
				QSettings.Instance.ImageOnly = _bool;
				RectGoTo = new Rect (); 
			}
			if (QSettings.Instance.StockToolBar && !QSettings.Instance.ImageOnly) {
				_bool = GUILayout.Toggle (QSettings.Instance.CenterText, "Center the buttons", GUILayout.Width (300));
				if (_bool != QSettings.Instance.CenterText) {
					QSettings.Instance.CenterText = _bool;
					RectGoTo = new Rect ();
				}
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			if (QSettings.Instance.StockToolBar) {
				QSettings.Instance.StockToolBar_OnHover = GUILayout.Toggle (QSettings.Instance.StockToolBar_OnHover, "Show QuickGoTo on hovering", GUILayout.Width (300));
				if (QSettings.Instance.StockToolBar_OnHover) {
					QSettings.Instance.LockHover = GUILayout.Toggle (QSettings.Instance.LockHover, "Lock buttons on hover", GUILayout.Width (300));
				} else {
					GUILayout.Space (300);
				}
			}
			QSettings.Instance.KSPSkin = GUILayout.Toggle (QSettings.Instance.KSPSkin, "Use the KSP skin", GUILayout.Width (300));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal();
			GUILayout.Box("Enable/Disable",GUILayout.Height(30));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			QSettings.Instance.EnableGoToMainMenu = GUILayout.Toggle (QSettings.Instance.EnableGoToMainMenu, GetText(GoTo.MainMenu), GUILayout.Width (300));
			QSettings.Instance.EnableGoToSettings = GUILayout.Toggle (QSettings.Instance.EnableGoToSettings, GetText(GoTo.Settings), GUILayout.Width (300));
			QSettings.Instance.EnableGoToSpaceCenter = GUILayout.Toggle (QSettings.Instance.EnableGoToSpaceCenter, GetText(GoTo.SpaceCenter), GUILayout.Width (300));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			QSettings.Instance.EnableGoToVAB = GUILayout.Toggle (QSettings.Instance.EnableGoToVAB, GetText(GoTo.VAB), GUILayout.Width (300));
			QSettings.Instance.EnableGoToSPH = GUILayout.Toggle (QSettings.Instance.EnableGoToSPH, GetText(GoTo.SPH), GUILayout.Width (300));
			QSettings.Instance.EnableGoToTrackingStation = GUILayout.Toggle (QSettings.Instance.EnableGoToTrackingStation, GetText(GoTo.TrackingStation), GUILayout.Width (300));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			QSettings.Instance.EnableGoToMissionControl = GUILayout.Toggle (QSettings.Instance.EnableGoToMissionControl, GetText(GoTo.MissionControl), GUILayout.Width (300));
			QSettings.Instance.EnableGoToAdministration = GUILayout.Toggle (QSettings.Instance.EnableGoToAdministration, GetText(GoTo.Administration), GUILayout.Width (300));
			QSettings.Instance.EnableGoToRnD = GUILayout.Toggle (QSettings.Instance.EnableGoToRnD, GetText(GoTo.RnD), GUILayout.Width (300));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);


			GUILayout.BeginHorizontal();
			QSettings.Instance.EnableGoToAstronautComplex = GUILayout.Toggle (QSettings.Instance.EnableGoToAstronautComplex, GetText(GoTo.AstronautComplex), GUILayout.Width (300));
			QSettings.Instance.EnableGoToLastVessel = GUILayout.Toggle (QSettings.Instance.EnableGoToLastVessel, GetText (GoTo.LastVessel, true), GUILayout.Width (300));
			QSettings.Instance.EnableGoToRecover = GUILayout.Toggle (QSettings.Instance.EnableGoToRecover, GetText(GoTo.Recover), GUILayout.Width (300));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			QSettings.Instance.EnableGoToRevert = GUILayout.Toggle (QSettings.Instance.EnableGoToRevert, GetText(GoTo.Revert), GUILayout.Width (300));
			QSettings.Instance.EnableGoToRevertToEditor = GUILayout.Toggle (QSettings.Instance.EnableGoToRevertToEditor, GetText(GoTo.RevertToEditor), GUILayout.Width (300));
			QSettings.Instance.EnableGoToRevertToSpaceCenter = GUILayout.Toggle (QSettings.Instance.EnableGoToRevertToSpaceCenter, GetText(GoTo.RevertToSpaceCenter), GUILayout.Width (300));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginHorizontal();
			QSettings.Instance.EnableSettings = GUILayout.Toggle (QSettings.Instance.EnableSettings, GetText(GoTo.Configurations) + " (if disabled, it will be displayed only on the Space Center)", GUILayout.Width (900));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Close & Save", GUILayout.Height(20))) {
				Settings ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);
			GUILayout.EndVertical();
		}

		void DrawGoTo(int id) {
			bool _lockHover = QSettings.Instance.LockHover && QSettings.Instance.StockToolBar && QStockToolbar.Instance.isHovering && (!QStockToolbar.Instance.isTrue || !isBat);
			GUILayout.BeginVertical ();

			GUI.enabled = !_lockHover;

			if (!CanMainMenu) {
				GUI.enabled = false;
			}
			if (QSettings.Instance.EnableGoToMainMenu) {
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent (GetText (GoTo.MainMenu), Main_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
					HideGoTo (true);
					mainMenu ();
				}
				GUILayout.EndHorizontal ();
			}
			if (QSettings.Instance.EnableGoToSettings) {
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.Settings), Sett_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
					HideGoTo (true);
					settings ();
				}
				GUILayout.EndHorizontal ();
			}
			GUI.enabled = !_lockHover;

			if (QSettings.Instance.EnableGoToSpaceCenter) {
				if (!CanSpaceCenter) {
					GUI.enabled = false;
				}
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.SpaceCenter), SC_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
					HideGoTo (true);
					spaceCenter ();
				}
				GUILayout.EndHorizontal ();
				GUI.enabled = !_lockHover;
			}

			if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor || !HighLogic.LoadedSceneIsFlight) {
				if (QSettings.Instance.EnableGoToVAB) {
					if (!CanEditor (EditorFacility.VAB)) {
						GUI.enabled = false;
					}
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.VAB), VAB_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
						HideGoTo (true);
						VAB ();
					}
					GUILayout.EndHorizontal ();
					GUI.enabled = !_lockHover;
				}
				if (QSettings.Instance.EnableGoToSPH) {
					if (!CanEditor (EditorFacility.SPH)) {
						GUI.enabled = false;
					}
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.SPH), SPH_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
						HideGoTo (true);
						SPH ();
					}
					GUILayout.EndHorizontal ();
					GUI.enabled = !_lockHover;
				}
			}

			if (QSettings.Instance.EnableGoToTrackingStation && (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation || !HighLogic.LoadedSceneIsFlight)) {
				if (!CanTrackingStation) {
					GUI.enabled = false;
				}
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.TrackingStation), TS_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
					HideGoTo (true);
					trackingStation ();
				}
				GUILayout.EndHorizontal ();
				GUI.enabled = !_lockHover;
			}
			bool _CantGoToBat = !CanSpaceCenter && HighLogic.LoadedScene != GameScenes.SPACECENTER;
			if (CanFundBuilding) {
				if (QSettings.Instance.EnableGoToMissionControl) {
					if (_CantGoToBat || isMissionControl) {
						GUI.enabled = false;
					}
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.MissionControl), MI_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
						HideGoTo (true);
						missionControl ();
					}
					GUILayout.EndHorizontal ();
					GUI.enabled = !_lockHover;
				}

				if (QSettings.Instance.EnableGoToAdministration) {
					if (_CantGoToBat || isAdministration) {
						GUI.enabled = false;
					}
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.Administration), Admi_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
						HideGoTo (true);
						administration ();
					}
					GUILayout.EndHorizontal ();
					GUI.enabled = !_lockHover;
				}
			}

			if (CanScienceBuilding) {
				if (QSettings.Instance.EnableGoToRnD) {
					if (_CantGoToBat || isRnD) {
						GUI.enabled = false;
					}
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.RnD), RnD_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
						HideGoTo (true);
						RnD ();
					}
					GUILayout.EndHorizontal ();
					GUI.enabled = !_lockHover;
				}
			}

			if (QSettings.Instance.EnableGoToAstronautComplex) {
				if (_CantGoToBat || isAstronautComplex) {
					GUI.enabled = false;
				}
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.AstronautComplex), Astr_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
					HideGoTo (true);
					astronautComplex ();
				}
				GUILayout.EndHorizontal ();
				GUI.enabled = !_lockHover;
			}

			if (QSettings.Instance.EnableGoToLastVessel) {
				if (!CanLastVessel) {
					GUI.enabled = false;
				}
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.LastVessel), Lves_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
					HideGoTo (true);
					LastVessel ();
				}
				GUILayout.EndHorizontal ();
				GUI.enabled = !_lockHover;
			}

			if (HighLogic.LoadedSceneIsFlight) {
				if (QSettings.Instance.EnableGoToRecover && CanRecover) {
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.Recover), Rc_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
						HideGoTo (true);
						Recover ();
					}
					GUILayout.EndHorizontal ();
				}

				if (HighLogic.CurrentGame.Parameters.Flight.CanRestart && FlightDriver.CanRevertToPostInit) {
					if (QSettings.Instance.EnableGoToRevert) {
						if (!CanRevert) {
							GUI.enabled = false;
						}
						GUILayout.BeginHorizontal ();
						if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.Revert), Rv_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
							HideGoTo (true);
							Revert ();
						}
						GUILayout.EndHorizontal ();
						GUI.enabled = !_lockHover;
					}
				}

				if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor && FlightDriver.CanRevertToPrelaunch) {
					if (QSettings.Instance.EnableGoToRevertToEditor) {
						if (!CanRevertToEditor) {
							GUI.enabled = false;
						}
						GUILayout.BeginHorizontal ();
						if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.RevertToEditor), RvED_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
							HideGoTo (true);
							RevertToEditor ();
						}
						GUILayout.EndHorizontal ();
						GUI.enabled = !_lockHover;
					}
				}

				if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter && FlightDriver.CanRevertToPrelaunch) {
					if (QSettings.Instance.EnableGoToRevertToSpaceCenter) {
						if (!CanRevertToSpaceCenter) {
							GUI.enabled = false;
						}
						GUILayout.BeginHorizontal ();
						if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.RevertToSpaceCenter), RvSC_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
							HideGoTo (true);
							RevertToSpaceCenter ();
						}
						GUILayout.EndHorizontal ();
						GUI.enabled = !_lockHover;
					}
				}
			}
			if (QSettings.Instance.EnableSettings || HighLogic.LoadedScene == GameScenes.SPACECENTER) {
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button (new GUIContent (" " + GetText (GoTo.Configurations), Conf_Texture), GoToButtonStyle, GUILayout.Height(GoToButtonHeight))) {
					Settings ();
				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.EndVertical ();
		}

		void DrawButton(Rect rectBtn) {
			GUILayout.BeginArea (rectBtn);
			if (GUILayout.Button (QStockToolbar.GetTexture, GUILayout.Width(40), GUILayout.Height(40))) {
				ToggleGoTo ();
			}
			GUILayout.EndArea();
		}
	}
}