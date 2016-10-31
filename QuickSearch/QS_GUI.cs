/* 
QuickSearch
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

using System.Text.RegularExpressions;
using UnityEngine;

namespace QuickSearch {

	public partial class QuickSearch {

		protected GUIStyle TextField;
		bool WindowSettings = false;
		Rect rectSettings = new Rect (0, 0, 615, 0);
		Rect RectSettings {
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

		static void Lock(bool activate, ControlTypes Ctrl) {
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
		}

		internal static void instancedSettings() {
			if (QEditor.Instance != null) {
				QEditor.Instance.Settings ();
				return;
			}
			if (QRnD.Instance != null) {
				QRnD.Instance.Settings ();
				return;
			}
			Log ("No instance");
		}

		void Settings() {
			WindowSettings = !WindowSettings;
			Switch (WindowSettings);
			if (!WindowSettings) {
				Save ();
			}
			Log ("Settings", "QGUI");
		}

		void Switch(bool set) {
			QStockToolbar.Instance.Set (WindowSettings);
			Lock (WindowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
		}

		void HideSettings() {
			WindowSettings = false;
			Switch (false);
			Save ();
			Log ("HideSettings", "QGUI");
		}

		void ShowSettings() {
			WindowSettings = true;
			Lock (WindowSettings, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
			Log ("ShowSettings", "QGUI");
		}

		void Save() {
			QStockToolbar.Instance.Reset ();
			BlizzyToolbar.Reset ();
			if (QEditor.Instance != null) {
				QEditor.Instance.Refresh ();
			}
			QSettings.Instance.Save ();
		}

		protected virtual void OnGUI() {
			if (!WindowSettings) {
				return;
			}
			GUI.skin = HighLogic.Skin;
			if (!WindowSettings) {
				return;
			}
			RectSettings = GUILayout.Window (1545146, RectSettings, DrawSettings, MOD + " " + VERSION, GUILayout.Width (RectSettings.width), GUILayout.ExpandHeight (true));
		}

		// Panneau de configuration
		void DrawSettings(int id) {
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Box ("Options", GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, "Use the Stock ToolBar", GUILayout.Width (275));
			GUILayout.FlexibleSpace ();
			if (QBlizzyToolbar.isAvailable) {
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, "Use the Blizzy ToolBar", GUILayout.Width (225));
			} else {
				GUILayout.Space (225);
			}
			GUILayout.FlexibleSpace ();
			QSettings.Instance.Debug = GUILayout.Toggle (QSettings.Instance.Debug, "Enable Debug", GUILayout.Width (225));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			GUILayout.BeginHorizontal ();
			QSettings.Instance.EditorSearch = GUILayout.Toggle (QSettings.Instance.EditorSearch, "Enable editor search" + (HighLogic.LoadedSceneIsEditor ? "(switch scene needed)" : string.Empty), GUILayout.Width (455));
			GUILayout.FlexibleSpace ();
			QSettings.Instance.RnDSearch = GUILayout.Toggle (QSettings.Instance.RnDSearch, "Enable RnD search", GUILayout.Width (225));
			GUILayout.EndHorizontal ();
			GUILayout.Space (5);

			if (QSettings.Instance.EditorSearch || QSettings.Instance.RnDSearch) {

				GUILayout.BeginHorizontal ();
				QSettings.Instance.enableSearchExtension = GUILayout.Toggle (QSettings.Instance.enableSearchExtension, "Enable the extended search", GUILayout.Width (225));
				GUILayout.EndHorizontal ();
				GUILayout.Space (5);

				if (QSettings.Instance.enableSearchExtension) {

					GUILayout.BeginHorizontal ();
					GUILayout.Box ("Search Shortcut", GUILayout.Height (30));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("NOT: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchNOT = cleanString (GUILayout.TextField (QSettings.Instance.searchNOT, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("AND: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchAND = cleanString (GUILayout.TextField (QSettings.Instance.searchAND, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("OR: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchOR = cleanString (GUILayout.TextField (QSettings.Instance.searchOR, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Word begin: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchBegin = cleanString (GUILayout.TextField (QSettings.Instance.searchBegin, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Word end: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchEnd = cleanString (GUILayout.TextField (QSettings.Instance.searchEnd, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Word: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchWord = cleanString (GUILayout.TextField (QSettings.Instance.searchWord, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Name: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchName = cleanString (GUILayout.TextField (QSettings.Instance.searchName, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Title: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchTitle = cleanString (GUILayout.TextField (QSettings.Instance.searchTitle, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Description: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchDescription = cleanString (GUILayout.TextField (QSettings.Instance.searchDescription, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Author: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchAuthor = cleanString (GUILayout.TextField (QSettings.Instance.searchAuthor, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Manufacturer: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchManufacturer = cleanString (GUILayout.TextField (QSettings.Instance.searchManufacturer, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Tag: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchTag = cleanString (GUILayout.TextField (QSettings.Instance.searchTag, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Resources: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchResourceInfos = cleanString (GUILayout.TextField (QSettings.Instance.searchResourceInfos, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Tech Required: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchTechRequired = cleanString (GUILayout.TextField (QSettings.Instance.searchTechRequired, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Part size: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchPartSize = cleanString (GUILayout.TextField (QSettings.Instance.searchPartSize, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);

					GUILayout.BeginHorizontal ();
					GUILayout.Label ("Module: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchModule = cleanString (GUILayout.TextField (QSettings.Instance.searchModule, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label ("Regex: ", GUILayout.Width (100));
					GUILayout.Space (5);
					QSettings.Instance.searchRegex = cleanString (GUILayout.TextField (QSettings.Instance.searchRegex, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Space (185);
					GUILayout.EndHorizontal ();
					GUILayout.Space (5);
				}
			}
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Close", GUILayout.Width (100))) {
				HideSettings ();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical ();
		}

		string cleanString (string text) {
			string _str = clean(text);
			if (string.IsNullOrEmpty (_str)) {
				return string.Empty;
			}
			return _str.Substring (_str.Length - 1, 1);
		}
		string clean (string text) {
			return Regex.Replace (text, @"[^%ù£¤'#~&`_²\{\}!\.@\-|&/\(\)\[\]\+?,;:/\*µ\^\$=\ ""]", string.Empty);
		}
	}
}