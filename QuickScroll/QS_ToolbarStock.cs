/* 
QuickScroll
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

using KSP.UI.Screens;
using System;
using System.Collections;
using UnityEngine;

namespace QuickScroll {
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class QStockToolbar : MonoBehaviour {

		internal static bool Enabled {
			get {
				return QSettings.Instance.StockToolBar;
			}
		}

		private static bool CanUseIt {
			get {
				return HighLogic.LoadedSceneIsGame;
			}
		}
		
		private ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
		private static string TexturePath = QuickScroll.MOD + "/Textures/StockToolBar";

		private void OnTrue () {
			if (QGUI.WindowSettings) {
				return;
			}
			QGUI.ShowSettings ();
			QuickScroll.Log ("OnTrue", "QStockToolbar");
		}

		private void OnFalse () {
			if (!QGUI.WindowSettings) {
				return;
			}
			QGUI.HideSettings ();
			QuickScroll.Log ("OnFalse", "QStockToolbar");
		}

		private void OnHover () {
			if (QGUI.WindowSettings || !QSettings.Instance.StockToolBarHovering) {
				return;
			}
			QGUI.ShowSettings ();
			QuickScroll.Log ("OnHover", "QStockToolbar");
		}

		private void OnHoverOut () {
			if (!QGUI.WindowSettings || !QSettings.Instance.StockToolBarHovering) {
				return;
			}
			if (!isTrue && !isHovering) {
				QGUI.HideSettings ();
			}
			QuickScroll.Log ("OnHoverOut", "QStockToolbar");
		}
			
		private Texture2D GetTexture {
			get {
				return GameDatabase.Instance.GetTexture(TexturePath, false);
			}
		}

		private ApplicationLauncherButton appLauncherButton;

		internal static bool isAvailable {
			get {
				return ApplicationLauncher.Ready && ApplicationLauncher.Instance != null;
			}
		}

		internal bool isActive {
			get {
				return appLauncherButton != null && isAvailable;
			}
		}

		internal bool isHovering {
			get {
				if (appLauncherButton == null || !QGUI.WindowSettings) {
					return false;
				}
				Rect _rect = QGUI.RectSettings;
				_rect.height += 3;
				return appLauncherButton.IsHovering || _rect.Contains (Mouse.screenPos);
			}
		}

		internal bool isTrue {
			get {
				if (appLauncherButton == null) {
					return false;
				}
				return appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True;
			}
		}

		internal bool isFalse {
			get {
				if (appLauncherButton == null) {
					return false;
				}
				return appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.False;
			}
		}

		internal static QStockToolbar Instance {
			get;
			private set;
		}

		private void Awake() {
			if (Instance != null) {
				Destroy (this);
				return;
			}
			Instance = this;
			DontDestroyOnLoad (Instance);
			GameEvents.onGUIApplicationLauncherReady.Add (AppLauncherReady);
			GameEvents.onGUIApplicationLauncherDestroyed.Add (AppLauncherDestroyed);
			GameEvents.onLevelWasLoadedGUIReady.Add (AppLauncherDestroyed);
			QuickScroll.Log ("Awake", "QStockToolbar");
		}
			
		private void AppLauncherReady() {
			QSettings.Instance.Load ();
			if (!Enabled) {
				return;
			}
			Init ();
			QuickScroll.Log ("AppLauncherReady", "QStockToolbar");
		}

		private void AppLauncherDestroyed(GameScenes gameScene) {
			if (CanUseIt) {
				return;
			}
			Destroy ();
			QuickScroll.Log ("onLevelWasLoadedGUIReady", "QStockToolbar");
		}
		
		private void AppLauncherDestroyed() {
			Destroy ();
			QuickScroll.Log ("onGUIApplicationLauncherDestroyed", "QStockToolbar");
		}

		private void OnDestroy() {
			GameEvents.onGUIApplicationLauncherReady.Remove (AppLauncherReady);
			GameEvents.onGUIApplicationLauncherDestroyed.Remove (AppLauncherDestroyed);
			GameEvents.onLevelWasLoadedGUIReady.Remove (AppLauncherDestroyed);
			QuickScroll.Log ("OnDestroy", "QStockToolbar");
		}

		private void Init() {
			if (!isAvailable || !CanUseIt) {
				return;
			}
			if (appLauncherButton == null) {
				appLauncherButton = ApplicationLauncher.Instance.AddModApplication (OnTrue, OnFalse, OnHover, OnHoverOut, null, null, AppScenes, GetTexture);
			}
			QuickScroll.Log ("Init", "QStockToolbar");
		}

		private void Destroy() {
			if (appLauncherButton != null) {
				ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
				appLauncherButton = null;
			}
			QuickScroll.Log ("Destroy", "QStockToolbar");
		}

		internal void Set(bool SetTrue, bool force = false) {
			if (!isAvailable) {
				return;
			}
			if (appLauncherButton != null) {
				if (SetTrue) {
					if (isFalse) {
						appLauncherButton.SetTrue (force);
					}
				} else {
					if (isTrue) {
						appLauncherButton.SetFalse (force);
					}
				}
			}
			QuickScroll.Log ("Set", "QStockToolbar");
		}

		internal void Reset() {
			if (appLauncherButton != null) {
				Set (false);
				if (!Enabled) {
					Destroy ();
				}
			}
			if (Enabled) {
				Init ();
			}
			QuickScroll.Log ("Reset", "QStockToolbar");
		}
	}
}