/* 
QuickScience
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

using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;

namespace QuickScience
{
	public partial class QStockToolbar
	{

		QBlizzyToolbar BlizzyToolbar;

		internal static bool Enabled {
			get {
				return QSettings.Instance.StockToolBar;
			}
		}

		static bool CanUseIt {
			get {
				return HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedSceneIsFlight;
			}
		}

		ApplicationLauncher.AppScenes AppScenes = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.FLIGHT;

		public static string TexturePathNoTest = relativePath + "/Textures/StockToolBarNoTest";
		public static string TexturePathTest = relativePath + "/Textures/StockToolBarTest";
		public static string TexturePathConf = relativePath + "/Textures/StockToolBarConf";

		public static string TexturePath {
			get {
				return HighLogic.LoadedSceneIsFlight ? 
					            QScience.Instance.Experiments.hasEmptyTest () ?
								TexturePathTest :
								TexturePathNoTest :
					            TexturePathConf;
			}
		}

		void OnClick() {
			if (HighLogic.LoadedSceneIsFlight) {
				QScience.Instance.TestAll ();
				Set (false);
				return;
			}
			QGUI.Instance.Settings ();
		}

		Texture2D GetTexture {
			get {
				return GameDatabase.Instance.GetTexture(TexturePath, false);
			}
		}

		ApplicationLauncherButton appLauncherButton;

		internal static bool isAvailable {
			get {
				return ApplicationLauncher.Ready && ApplicationLauncher.Instance != null;
			}
		}

		internal static QStockToolbar Instance {
			get;
			private set;
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
			GameEvents.onLevelWasLoadedGUIReady.Add (AppLauncherDestroyed);
			if (BlizzyToolbar == null) {
				BlizzyToolbar = new QBlizzyToolbar ();
			}
			Log ("Awake", "QStockToolbar");
		}

		protected override void Start() {
			if (BlizzyToolbar != null) {
				BlizzyToolbar.Init ();
			}
			Log ("Start", "QStockToolbar");
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
			Log ("AppLauncherDestroyed", "QStockToolbar");
		}

		void AppLauncherDestroyed() {
			Destroy ();
			Log ("AppLauncherDestroyed", "QStockToolbar");
		}

		protected override void OnDestroy() {
			GameEvents.onGUIApplicationLauncherReady.Remove (AppLauncherReady);
			GameEvents.onGUIApplicationLauncherDestroyed.Remove (AppLauncherDestroyed);
			GameEvents.onLevelWasLoadedGUIReady.Remove (AppLauncherDestroyed);
			if (BlizzyToolbar != null) {
				BlizzyToolbar.Destroy ();
			}
			Log ("OnDestroy", "QStockToolbar");
		}

		void Init() {
			if (!isAvailable || !CanUseIt) {
				return;
			}
			if (appLauncherButton == null) {
				appLauncherButton = ApplicationLauncher.Instance.AddModApplication (OnClick, OnClick, null, null, null, null, AppScenes, GetTexture);
				appLauncherButton.onRightClick = delegate { QScience.Instance.CollectAll (); };
			}
			Log ("Init", "QStockToolbar");
		}

		void Destroy() {
			if (appLauncherButton != null) {
				ApplicationLauncher.Instance.RemoveModApplication (appLauncherButton);
				appLauncherButton = null;
			}
			Log ("Destroy", "QStockToolbar");
		}

		internal void Set(bool SetTrue, bool force = false) {
			if (!isAvailable) {
				return;
			}
			if (appLauncherButton != null) {
				if (SetTrue) {
					if (appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.False) {
						appLauncherButton.SetTrue (force);
					}
				} else {
					if (appLauncherButton.toggleButton.CurrentState == KSP.UI.UIRadioButton.State.True) {
						appLauncherButton.SetFalse (force);
					}
				}
			}
			Log ("Set: " + SetTrue + " " + force, "QStockToolbar");
		}

		internal void Reset() {
			if (BlizzyToolbar != null) {
				BlizzyToolbar.Reset ();
			}
			if (appLauncherButton != null) {
				Set (false);
				if (!Enabled) {
					Destroy ();
				}
			}
			if (Enabled) {
				Init ();
			}
			Log ("Reset", "QStockToolbar");
		}

		internal void Refresh() {
			if (BlizzyToolbar != null) {
				BlizzyToolbar.Refresh ();
			}
			if (!isAvailable) {
				return;
			}
			appLauncherButton.SetTexture (GetTexture);
			Set (false);
			Log ("Refresh", "QStockToolbar");
		}
	}
}