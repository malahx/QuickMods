/* 
QuickSearch
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

using System.Text.RegularExpressions;
using UnityEngine;

namespace QuickSearch {

	public partial class QuickSearch {

		protected GUIStyle TextField;
		bool WindowSettings = false;
		Rect rectSettings = new Rect (0, 0, 0, 0);
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
			RectSettings = GUILayout.Window (1545146, RectSettings, DrawSettings, MOD + " " + VERSION);
		}

		// Panneau de configuration
		void DrawSettings(int id) {
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate("Options"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			QSettings.Instance.StockToolBar = GUILayout.Toggle (QSettings.Instance.StockToolBar, QLang.translate ("Use the Stock Toolbar"), GUILayout.Width (400));
			GUILayout.FlexibleSpace ();
			if (QBlizzyToolbar.isAvailable) {
				QSettings.Instance.BlizzyToolBar = GUILayout.Toggle (QSettings.Instance.BlizzyToolBar, QLang.translate ("Use the Blizzy Toolbar"), GUILayout.Width (400));
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			QSettings.Instance.EditorSearch = GUILayout.Toggle (QSettings.Instance.EditorSearch, QLang.translate ("Replace editor search - reload needed"), GUILayout.Width (400));
			GUILayout.FlexibleSpace ();
			QSettings.Instance.RnDSearch = GUILayout.Toggle (QSettings.Instance.RnDSearch, QLang.translate ("Enable RnD search"), GUILayout.Width (400));
			GUILayout.EndHorizontal ();

			if (QSettings.Instance.EditorSearch || QSettings.Instance.RnDSearch) {

				GUILayout.BeginHorizontal ();
				QSettings.Instance.enableSearchExtension = GUILayout.Toggle (QSettings.Instance.enableSearchExtension, QLang.translate ("Enable the extended search"), GUILayout.Width (400));
				GUILayout.FlexibleSpace ();
				QSettings.Instance.enableEnterToSearch = GUILayout.Toggle (QSettings.Instance.enableEnterToSearch, QLang.translate ("Enable type Enter to search"), GUILayout.Width (400));
				GUILayout.EndHorizontal ();

				if (QSettings.Instance.enableSearchExtension) {

					GUILayout.BeginHorizontal ();
					GUILayout.Box (QLang.translate ("Search Shortcut"), GUILayout.Height (30));
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label (QLang.translate ("NOT:"), GUILayout.Width (100));
					QSettings.Instance.searchNOT = cleanString (GUILayout.TextField (QSettings.Instance.searchNOT, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("AND:"), GUILayout.Width (100));
					QSettings.Instance.searchAND = cleanString (GUILayout.TextField (QSettings.Instance.searchAND, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("OR:"), GUILayout.Width (100));
					QSettings.Instance.searchOR = cleanString (GUILayout.TextField (QSettings.Instance.searchOR, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label (QLang.translate ("Word begin:"), GUILayout.Width (100));
					QSettings.Instance.searchBegin = cleanString (GUILayout.TextField (QSettings.Instance.searchBegin, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Word end:"), GUILayout.Width (100));
					QSettings.Instance.searchEnd = cleanString (GUILayout.TextField (QSettings.Instance.searchEnd, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Word:"), GUILayout.Width (100));
					QSettings.Instance.searchWord = cleanString (GUILayout.TextField (QSettings.Instance.searchWord, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label (QLang.translate ("Name:"), GUILayout.Width (100));
					QSettings.Instance.searchName = cleanString (GUILayout.TextField (QSettings.Instance.searchName, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Title:"), GUILayout.Width (100));
					QSettings.Instance.searchTitle = cleanString (GUILayout.TextField (QSettings.Instance.searchTitle, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Description:"), GUILayout.Width (100));
					QSettings.Instance.searchDescription = cleanString (GUILayout.TextField (QSettings.Instance.searchDescription, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label (QLang.translate ("Author:"), GUILayout.Width (100));
					QSettings.Instance.searchAuthor = cleanString (GUILayout.TextField (QSettings.Instance.searchAuthor, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Manufacturer:"), GUILayout.Width (100));
					QSettings.Instance.searchManufacturer = cleanString (GUILayout.TextField (QSettings.Instance.searchManufacturer, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Tag:"), GUILayout.Width (100));
					QSettings.Instance.searchTag = cleanString (GUILayout.TextField (QSettings.Instance.searchTag, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label (QLang.translate ("Resources:"), GUILayout.Width (100));
					QSettings.Instance.searchResourceInfos = cleanString (GUILayout.TextField (QSettings.Instance.searchResourceInfos, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Tech Required:"), GUILayout.Width (100));
					QSettings.Instance.searchTechRequired = cleanString (GUILayout.TextField (QSettings.Instance.searchTechRequired, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Part size:"), GUILayout.Width (100));
					QSettings.Instance.searchPartSize = cleanString (GUILayout.TextField (QSettings.Instance.searchPartSize, TextField, GUILayout.Width (75)));
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					GUILayout.Label (QLang.translate ("Module:"), GUILayout.Width (100));
					QSettings.Instance.searchModule = cleanString (GUILayout.TextField (QSettings.Instance.searchModule, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Label (QLang.translate ("Regex:"), GUILayout.Width (100));
					QSettings.Instance.searchRegex = cleanString (GUILayout.TextField (QSettings.Instance.searchRegex, TextField, GUILayout.Width (75)));
					GUILayout.FlexibleSpace ();
					GUILayout.Space (185);
					GUILayout.EndHorizontal ();
				}
			}
			QLang.DrawLang ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (QLang.translate ("Close"), GUILayout.Width (100))) {
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