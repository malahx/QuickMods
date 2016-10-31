/* 
QuickEngineer
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

namespace QuickEngineer {
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class QStockToolbar : QuickEngineer {

		internal static QStockToolbar Instance {
			get;
			private set;
		}

		private static string TexturePath = MOD + "/Textures/StockToolBar";

		internal static bool Enabled {
			get {
				return !QSettings.Instance.FlightVesselEngineer_Disable && !QSettings.Instance.AllVesselEngineer_Disable;
			}
		}
			
		private static bool ModApp {
			get {
				return QSettings.Instance.StockToolBar_ModApp;
			}
		}

		private static bool CanUseIt {
			get {
				return HighLogic.LoadedSceneIsGame;
			}
		}

		internal static bool isAvailable {
			get {
				return ApplicationLauncher.Ready && ApplicationLauncher.Instance != null;
			}
		}

		internal static bool isModApp(ApplicationLauncherButton button) {
			bool _hidden;
			return ApplicationLauncher.Instance.Contains (button, out _hidden);
		}
		
		private ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.FLIGHT;
		private GameScenes gamesScenes = GameScenes.FLIGHT;

		internal ApplicationLauncherButton appLauncherButton;

		private Texture2D GetTexture {
			get {
				return GameDatabase.Instance.GetTexture(TexturePath, false);
			}
		}
			
		internal static bool isActive {
			get {
				if (QStockToolbar.Instance == null) {
					return false;
				}
				return QStockToolbar.Instance.appLauncherButton != null && isAvailable;
			}
		}

		internal bool isHovering {
			get {
				if (QFlight.Instance == null || !isActive) {
					return false;
				}
				return appLauncherButton.toggleButton.IsHovering || isHoverApp(QFlight.Instance.FlightEngineerRect);
			}
		}

		internal static bool isHoverApp(Rect appRect) {
			if (!isAvailable) {
				return false;
			}
			if (ApplicationLauncher.Instance.IsPositionedAtTop) {
				appRect.y -= 10;
				appRect.height += 10;
			} else {
				appRect.height += 10;
			}
			return appRect.Contains (Mouse.screenPos);
		}

		internal bool isTrue {
			get {
				if (appLauncherButton == null) {
					return false;
				}
				return appLauncherButton.State == RUIToggleButton.ButtonState.TRUE;
			}
		}

		internal bool isFalse {
			get {
				if (appLauncherButton == null) {
					return false;
				}
				return appLauncherButton.State == RUIToggleButton.ButtonState.FALSE;
			}
		}

		internal Rect Position {
			get {
				if (appLauncherButton == null || !isAvailable) {
					return new Rect ();
				}
				Camera _camera = UIManager.instance.uiCameras [0].camera;
				Vector3 _pos = appLauncherButton.GetAnchor ();
				Rect _rect = new Rect (0, 0, 40, 40);
				if (ApplicationLauncher.Instance.IsPositionedAtTop) {
					_rect.x = _camera.WorldToScreenPoint (_pos).x;
				} else {
					_rect.x = _camera.WorldToScreenPoint (_pos).x - 40;
					_rect.y = Screen.height - 40;
				}
				return _rect;
			}
		}

		private void OnTrue () {
			if (QFlight.Instance == null) {
				Log ("QFlight is not instanced (OnTrue)", "QStockToolbar");
				return;
			}
			QFlight.Instance.DisplayApp ();
			Log ("OnTrue", "QStockToolbar");
		}

		private void OnFalse () {
			if (QFlight.Instance == null) {
				Log ("QFlight is not instanced (OnFalse)", "QStockToolbar");
				return;
			}
			QFlight.Instance.HideApp ();
			Log ("OnFalse", "QStockToolbar");
		}

		private void OnHover () {
			if (isTrue) {
				return;
			}
			OnTrue ();
			Log ("OnHover", "QStockToolbar");
		}

		private void OnHoverOut () {
			if (QFlight.Instance == null) {
				Log ("QFlight is not instanced (OnHoverOut)", "QStockToolbar");
				return;
			}
			if (isTrue || isHoverApp(QFlight.Instance.FlightEngineerRect)) {
				return;
			}
			OnFalse ();
			Log ("OnHoverOut", "QStockToolbar");
		}

		private void OnEnable () {
			Log ("OnEnable", "QStockToolbar");
		}

		private void OnDisable () {
			OnFalse ();
			Log ("OnDisable", "QStockToolbar");
		}

		protected override void Awake() {
			if (Instance != null) {
				Destroy (this);
				Warning ("There's already an Instance of " + MOD);
				return;
			}
			Instance = this;
			DontDestroyOnLoad (Instance);
			GameEvents.onGUIApplicationLauncherReady.Add (AppLauncherReady);
			GameEvents.onGUIApplicationLauncherDestroyed.Add (AppLauncherDestroyed);
			GameEvents.onLevelWasLoadedGUIReady.Add (AppLauncherDestroyed);
			Log ("Awake", "QStockToolbar");
		}
			
		private void AppLauncherReady() {
			QSettings.Instance.Load ();
			if (!Enabled) {
				return;
			}
			Init ();
			Log ("AppLauncherReady", "QStockToolbar");
		}

		private void AppLauncherDestroyed(GameScenes gameScene) {
			if (CanUseIt) {
				return;
			}
			Destroy ();
		}
		
		private void AppLauncherDestroyed() {
			Destroy ();
			Log ("AppLauncherDestroyed", "QStockToolbar");
		}

		protected override void OnDestroy() {
			GameEvents.onGUIApplicationLauncherReady.Remove (AppLauncherReady);
			GameEvents.onGUIApplicationLauncherDestroyed.Remove (AppLauncherDestroyed);
			GameEvents.onLevelWasLoadedGUIReady.Remove (AppLauncherDestroyed);
			Log ("OnDestroy", "QStockToolbar");
		}

		private void Init() {
			if (!isAvailable || !CanUseIt) {
				Log ("Applauncher is not Active (Init)", "QStockToolbar");
				return;
			}
			if (appLauncherButton == null) {
				if (ModApp) {
					appLauncherButton = ApplicationLauncher.Instance.AddModApplication (new RUIToggleButton.OnTrue (this.OnTrue), new RUIToggleButton.OnFalse (this.OnFalse), new RUIToggleButton.OnHover (this.OnHover), new RUIToggleButton.OnHoverOut (this.OnHoverOut), new RUIToggleButton.OnEnable (this.OnEnable), new RUIToggleButton.OnDisable (this.OnDisable), AppScenes, GetTexture);
					ApplicationLauncher.Instance.EnableMutuallyExclusive (appLauncherButton);
				} else {
					appLauncherButton = ApplicationLauncher.Instance.AddApplication (new RUIToggleButton.OnTrue (this.OnTrue), new RUIToggleButton.OnFalse (this.OnFalse), new RUIToggleButton.OnHover (this.OnHover), new RUIToggleButton.OnHoverOut (this.OnHoverOut), new RUIToggleButton.OnEnable (this.OnEnable), new RUIToggleButton.OnDisable (this.OnDisable), GetTexture);
					appLauncherButton.VisibleInScenes = AppScenes;
				}
				ApplicationLauncher.Instance.AddOnHideCallback (OnHide);
			}
			Log ("Init", "QStockToolbar");
		}

		private void OnShow() {
			if (!isAvailable) {
				Log ("Applauncher is not Active (OnShow)", "QStockToolbar");
				return;
			}
			ApplicationLauncher.Instance.RemoveOnShowCallback (OnShow);
			Log ("OnShow", "QStockToolbar");
		}

		private void OnHide() {
			if (!isAvailable) {
				Log ("Applauncher is not Active (OnHide)", "QStockToolbar");
				return;
			}
			ApplicationLauncher.Instance.AddOnShowCallback (OnShow);
			OnFalse ();
			Log ("OnHide", "QStockToolbar");
		}

		private void Destroy() {
			if (appLauncherButton != null) {
				ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
				ApplicationLauncher.Instance.RemoveApplication (appLauncherButton);
				appLauncherButton = null;
			}
			Log ("Destroy", "QStockToolbar");
		}

		internal void Set(bool SetTrue, bool force = false) {
			if (!isAvailable) {
				Log ("Applauncher is not Active (Set)", "QStockToolbar");
				return;
			}
			if (appLauncherButton != null) {
				if (SetTrue) {
					if (appLauncherButton.State == RUIToggleButton.ButtonState.FALSE) {
						appLauncherButton.SetTrue (force);
					}
				} else {
					if (appLauncherButton.State == RUIToggleButton.ButtonState.TRUE) {
						appLauncherButton.SetFalse (force);
					}
				}
			}
			Log ("Set " + SetTrue, "QStockToolbar");
		}

		internal void Reset() {
			if (appLauncherButton != null) {
				Set (false);
				if (!Enabled || (Enabled && (ModApp && !isModApp(appLauncherButton)) || (!ModApp && isModApp(appLauncherButton)))) {
					Destroy ();
				}
			}
			if (Enabled) {
				Init ();
			}
			Log ("Reset", "QStockToolbar");
		}

		private void Update() {
			if (HighLogic.LoadedScene != gamesScenes) {
				return;
			}
			if (QFlight.appIsLive && !isHovering) {
				OnHoverOut ();
			}
		}
	}
}