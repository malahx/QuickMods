/* 
QuickGoTo
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

using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;

namespace QuickGoTo {

	public partial class QStockToolbar {

		ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.TRACKSTATION | ApplicationLauncher.AppScenes.VAB;
		internal ApplicationLauncherButton appLauncherButton;

		internal bool isActive {
			get {
				return appLauncherButton != null && isAvailable;
			}
		}

		internal bool isHovering {
			get {
				if (!QSettings.Instance.StockToolBar_OnHover || appLauncherButton == null || QGoTo.Instance == null) {
					return false;
				}
				if (QGoTo.Instance.RectGoTo == new Rect ()) {
					return false;
				}
				return appLauncherButton.IsHovering || QGoTo.Instance.RectGoTo.Contains (Mouse.screenPos) || QGoTo.Instance.RectButton.Contains (Mouse.screenPos);
			}
		}

		internal bool isTrue {
			get {
				if (appLauncherButton == null || QGoTo.Instance == null) {
					return false;
				}
				if (QGoTo.Instance.RectGoTo == new Rect ()) {
					return false;
				}
				return appLauncherButton.toggleButton.CurrentState == UIRadioButton.State.True;
			}
		}

		internal bool isFalse {
			get {
				if (appLauncherButton == null || QGoTo.Instance == null) {
					return false;
				}
				if (QGoTo.Instance.RectGoTo == new Rect ()) {
					return false;
				}
				return appLauncherButton.toggleButton.CurrentState == UIRadioButton.State.False;
			}
		}

		internal Rect Position {
			get {
				if (appLauncherButton == null || !isAvailable) {
					return new Rect ();
				}
				Camera _camera = UIMainCamera.Camera;
				Vector3 _pos = _camera.WorldToScreenPoint (appLauncherButton.GetAnchorUL());
				return new Rect (_pos.x, Screen.height - _pos.y, 41, 41);
			}
		}


		public static QStockToolbar Instance {
			get;
			private set;
		}

		internal static bool Enabled {
			get {
				return QSettings.Instance.StockToolBar;
			}
		}
			
		static bool ModApp {
			get {
				return QSettings.Instance.StockToolBar_ModApp;
			}
		}

		static bool CanUseIt {
			get {
				return HighLogic.LoadedSceneIsGame;
			}
		}
		
		static string TexturePath = relativePath + "/Textures/StockToolBar";

		internal static Texture2D GetTexture {
			get {
				return GameDatabase.Instance.GetTexture(TexturePath, false);
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
			
		protected override void Awake() {
			if (Instance != null) {
				Destroy (this);
				return;
			}
			Instance = this;
			DontDestroyOnLoad (Instance);
			GameEvents.onGUIApplicationLauncherReady.Add (AppLauncherReady);
			GameEvents.onGUIApplicationLauncherDestroyed.Add (AppLauncherDestroyed);
			GameEvents.onGUIApplicationLauncherUnreadifying.Add (AppLauncherUnreadifying);
			GameEvents.onLevelWasLoadedGUIReady.Add (AppLauncherDestroyed);
			Log ("Awake", "QStockToolbar");
		}

		protected override void Start() {
			Log ("Start", "QStockToolbar");
		}

		protected override void OnDestroy() {
			GameEvents.onGUIApplicationLauncherReady.Remove (AppLauncherReady);
			GameEvents.onGUIApplicationLauncherDestroyed.Remove (AppLauncherDestroyed);
			GameEvents.onGUIApplicationLauncherUnreadifying.Remove (AppLauncherUnreadifying);
			GameEvents.onLevelWasLoadedGUIReady.Remove (AppLauncherDestroyed);
			Log ("OnDestroy", "QStockToolbar");
		}

		void OnTrue () {
			if (QGoTo.Instance == null) {
				Warning ("No QGoTo Instance", "QStockToolbar");
				return;
			}
			if (QGoTo.Instance.WindowSettings) {
				return;
			}
			QGoTo.Instance.ShowGoTo ();
			Log ("OnTrue", "QStockToolbar");
		}

		void OnFalse () {
			if (QGoTo.Instance == null) {
				Warning ("No QGoTo Instance", "QStockToolbar");
				return;
			}
			if (QGoTo.Instance.WindowSettings) {
				QGoTo.Instance.Settings ();
				return;
			}
			QGoTo.Instance.HideGoTo ();
			Log ("OnFalse", "QStockToolbar");
		}

		void OnHover () {
			if (QGoTo.Instance == null) {
				Warning ("No QGoTo Instance", "QStockToolbar");
				return;
			}
			if (!QSettings.Instance.StockToolBar_OnHover || QGoTo.Instance.WindowSettings) {
				return;
			}
			QGoTo.Instance.ShowGoTo ();
			Log ("OnHover", "QStockToolbar");
		}

		void OnHoverOut () {
			if (QGoTo.Instance == null) {
				Warning ("No QGoTo Instance", "QStockToolbar");
				return;
			}
			if (!QSettings.Instance.StockToolBar_OnHover || QGoTo.Instance.WindowSettings || !QGoTo.Instance.WindowGoTo) {
				return;
			}
			if (QGoTo.Instance.RectGoTo == new Rect ()) {
				QGoTo.Instance.HideGoTo (true);
				return;
			}
			if (!isTrue && !isHovering) {
				QGoTo.Instance.HideGoTo ();
			}
			Log ("OnHoverOut", "QStockToolbar");
		}

		void OnEnable () {
			Log ("OnEnable", "QStockToolbar");
		}

		void OnDisable () {
			if (QGoTo.Instance == null) {
				Warning ("No QGoTo Instance", "QStockToolbar");
				return;
			}
			QGoTo.Instance.HideGoTo ();
			Log ("OnDisable", "QStockToolbar");
		}

		void AppLauncherReady() {
			if (!Enabled) {
				return;
			}
			Init ();
			Log ("AppLauncherReady", "QStockToolbar");
		}

		void AppLauncherDestroyed(GameScenes gameScene) {
			if (CanUseIt) {
				return;
			}
			Destroy ();
		}
		
		void AppLauncherDestroyed() {
			Destroy ();
			Log ("AppLauncherDestroyed", "QStockToolbar");
		}

		void AppLauncherUnreadifying(GameScenes gameScenes) {
			Set (false, true);
			Log ("AppLauncherUnreadifying", "QStockToolbar");
		}

		void Init() {
			if (!isAvailable || !CanUseIt) {
				return;
			}
			Destroy ();
			if (appLauncherButton == null) {
				if (ModApp) {
					appLauncherButton = ApplicationLauncher.Instance.AddModApplication (OnTrue, OnFalse, OnHover, OnHoverOut, OnEnable, OnDisable, AppScenes, GetTexture);
					ApplicationLauncher.Instance.EnableMutuallyExclusive (appLauncherButton);
				} else {
					appLauncherButton = ApplicationLauncher.Instance.AddApplication (OnTrue, OnFalse, OnHover, OnHoverOut, OnEnable, OnDisable, GetTexture);
					appLauncherButton.VisibleInScenes = AppScenes;
				}
				ApplicationLauncher.Instance.AddOnHideCallback (OnHide);
			}
			Log ("Init", "QStockToolbar");
		}

		void OnShow() {
			QGoTo.Instance.ShowGoTo ();
			ApplicationLauncher.Instance.RemoveOnShowCallback (OnShow);
			Log ("OnShow", "QStockToolbar");
		}

		void OnHide() {
			if (!isAvailable || QGoTo.Instance == null) {
				return;
			}
			if (QGoTo.Instance.WindowSettings || QGoTo.Instance.WindowGoTo) {
				ApplicationLauncher.Instance.AddOnShowCallback (OnShow);
			}
			if (QGoTo.Instance.WindowSettings) {
				QGoTo.Instance.SettingsSwitch ();
			} else if (QGoTo.Instance.WindowGoTo) {
				QGoTo.Instance.HideGoTo (true);
			}
			Log ("OnHide", "QStockToolbar");
		}

		void Destroy() {
			if (appLauncherButton != null) {
				ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
				ApplicationLauncher.Instance.RemoveApplication (appLauncherButton);
				ApplicationLauncher.Instance.RemoveOnHideCallback (OnHide);
				ApplicationLauncher.Instance.RemoveOnShowCallback (OnShow);
				appLauncherButton = null;
				Log ("Destroy", "QStockToolbar");
			}
		}

		internal void Set(bool SetTrue, bool force = false) {
			if (!isActive) {
				return;
			}
			if (SetTrue) {
				if (isFalse) {
					appLauncherButton.SetTrue (force);
				}
			} else {
				if (isTrue) {
					appLauncherButton.SetFalse (force);
				}
			}
			Log ("Set", "QStockToolbar");
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
	}
}