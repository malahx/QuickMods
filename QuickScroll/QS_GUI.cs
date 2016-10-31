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

using System;
using UnityEngine;

namespace QuickScroll {

	public class QGUI {

		internal static bool WindowSettings = false;
		private static Rect rectSettings;
		internal static Rect RectSettings {
			get {
				Rect _rect = rectSettings;
				if (!QSettings.Instance.StockToolBar) {
					_rect.x = (Screen.width - _rect.width) / 2;
					_rect.y = (Screen.height - _rect.height) / 2;
				} else {
					_rect.x = Screen.width - _rect.width - 75;
					_rect.y = Screen.height - _rect.height - 40;
				}
				return _rect;
			}
			set {
				rectSettings = value;
			}
		}

		internal static void Awake() {
			RectSettings = new Rect (0, 0, 615, 0);
		}

		private static void Lock(bool activate, ControlTypes Ctrl = ControlTypes.None) {
			if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					EditorLogic.fetch.Lock (true, true, true, "Lock" + QuickScroll.MOD);
				} else {
					EditorLogic.fetch.Unlock ("Lock" + QuickScroll.MOD);
				}
			}
			if (!activate) {
				InputLockManager.RemoveControlLock ("Lock" + QuickScroll.MOD);
			}
		}

		public static void Settings() {
			WindowSettings = !WindowSettings;
			Switch (WindowSettings);
			if (!WindowSettings) {
				Save ();
			}
			QuickScroll.Log ("Settings", "QGUI");
		}

		internal static void Switch(bool set) {
			QStockToolbar.Instance.Set (WindowSettings);
			Lock (WindowSettings);
		}

		internal static void HideSettings() {
			WindowSettings = false;
			Switch (false);
			Save ();
			QuickScroll.Log ("HideSettings", "QGUI");
		}

		internal static void ShowSettings() {
			WindowSettings = true;
			Lock (true);
			QuickScroll.Log ("ShowSettings", "QGUI");
		}

		private static void Save() {
			//QCategory.PartListTooltipsTWEAK (false);
			QStockToolbar.Instance.Reset ();
			QuickScroll.BlizzyToolbar.Reset ();
			QSettings.Instance.Save ();
		}

		internal static void OnGUI() {
			if (!WindowSettings) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			if (!QStockToolbar.Instance.isTrue && !QStockToolbar.Instance.isHovering) {
				HideSettings ();
				return;
			}
			RectSettings = GUILayout.Window (1545145, RectSettings, DrawSettings, QuickScroll.MOD + " " + QuickScroll.VERSION, GUILayout.Width (RectSettings.width), GUILayout.ExpandHeight (true));
		}

		// Panneau de configuration
		private static void DrawSettings(int id) {
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Box ("Options", GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			bool _enableWheelScroll = GUILayout.Toggle (QSettings.Instance.EnableWheelScroll, "Enable Wheel Scroll", GUILayout.Width (300));
			bool _enableWheelShortCut = GUILayout.Toggle (QSettings.Instance.EnableWheelShortCut, "Enable Shortcut with Wheel Scroll", GUILayout.Width (300));
			if (_enableWheelScroll && _enableWheelShortCut != QSettings.Instance.EnableWheelShortCut) {
				rectSettings.height = 0;
			}
			if (_enableWheelScroll != QSettings.Instance.EnableWheelScroll) {
				QSettings.Instance.EnableWheelScroll = _enableWheelScroll;
				rectSettings.height = 0;
			}
			if (_enableWheelShortCut != QSettings.Instance.EnableWheelShortCut) {
				QSettings.Instance.EnableWheelShortCut = _enableWheelShortCut;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			if (QSettings.Instance.EnableWheelScroll) {
				GUILayout.BeginHorizontal ();
				QSettings.Instance.EnableWheelBlockTopEnd = GUILayout.Toggle (QSettings.Instance.EnableWheelBlockTopEnd, "Enable the blocking of scrolling at the beggining or the end of a category", GUILayout.Width (300));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
			}

			GUILayout.BeginHorizontal ();
			bool _enableKeyShortCut = GUILayout.Toggle (QSettings.Instance.EnableKeyShortCut, "Enable Keyboard Shortcut", GUILayout.Width (300));
			if (_enableKeyShortCut != QSettings.Instance.EnableKeyShortCut) {
				QSettings.Instance.EnableKeyShortCut = _enableKeyShortCut;
				rectSettings.height = 0;
			}
			//QSettings.Instance.EnableTWEAKPartListTooltips = GUILayout.Toggle (QSettings.Instance.EnableTWEAKPartListTooltips, "Enable the tweak for PartListTooltips", GUILayout.Width (300));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, "Use the Stock Toolbar", GUILayout.Width (300));
			if (QBlizzyToolbar.isAvailable) {
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, "Use the Blizzy's Toolbar", GUILayout.Width (300));
			}
			/*if (QSettings.Instance.StockToolBar) {
				if (QBlizzyToolbar.isAvailable) {
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);
					GUILayout.BeginHorizontal ();
				}
				QSettings.Instance.StockToolBarHovering = GUILayout.Toggle (QSettings.Instance.StockToolBarHovering, "Enable the Stock Toolbar Hovring", GUILayout.Width (300));
			}*/
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			if (QSettings.Instance.EnableWheelScroll && QSettings.Instance.EnableWheelShortCut) {

				GUILayout.BeginHorizontal ();
				GUILayout.Box ("Modifier Keyboard for Scrolling", GUILayout.Height (30));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.ModKeyFilterWheel);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.ModKeyCategoryWheel);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

			}
			if (QSettings.Instance.EnableKeyShortCut) {

				GUILayout.BeginHorizontal ();
				GUILayout.Box ("Keyboard ShortCuts", GUILayout.Height (30));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.ModKeyShortCut);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.FilterPrevious);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.FilterNext);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.CategoryPrevious);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.CategoryNext);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				/*GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.PagePrevious);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.PageNext);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);*/

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Pods);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.FuelTanks);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Engines);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.CommandNControl);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Structural);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.Aerodynamics);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);
				GUILayout.BeginHorizontal ();
				QShortCuts.DrawConfigKey (QShortCuts.Key.Utility);
				GUILayout.Space (5);
				QShortCuts.DrawConfigKey (QShortCuts.Key.Sciences);
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

			}
			if (GUILayout.Button ("Close", GUILayout.Height (30))) {
				QShortCuts.VerifyKey ();
				HideSettings ();
			}
			GUILayout.EndVertical ();
		}
	}
}