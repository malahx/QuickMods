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
using UnityEngine;

namespace QuickScroll {
	public class QShortCuts : QKey {
		internal static void Awake() {
			RectSetKey = new Rect ((Screen.width - 300) / 2, (Screen.height - 100) / 2, 300, 100);
		}

		internal static void Update() {
			if (SetKey != Key.None) {
				if (Event.current.isKey) {
					KeyCode _key = Event.current.keyCode;
					if (_key != KeyCode.None) {
						SetCurrentKey (SetKey, _key);
						QSettings.Instance.Save ();
						SetKey = Key.None;
						QGUI.WindowSettings = true;
					}
				}
				return;
			}
			if (EditorLogic.fetch.editorScreen != EditorScreen.Parts) {
				return;
			}
			if (!QSettings.Instance.EnableKeyShortCut) {
				return;
			}
			bool _ModKey = (QSettings.Instance.ModKeyShortCut == KeyCode.None ? true : Input.GetKey (QSettings.Instance.ModKeyShortCut));
			if (!_ModKey) {
				return;
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyFilterPrevious)) {
				PartCategorizer.Instance.SetAdvancedMode ();
				QCategory.SelectPartFilter (true);
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyFilterNext)) {
				PartCategorizer.Instance.SetAdvancedMode ();
				QCategory.SelectPartFilter (false);
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyCategoryPrevious)) {
				QCategory.SelectPartCategory (true);
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyCategoryNext)) {
				QCategory.SelectPartCategory (false);
			}
			/*if (Input.GetKeyDown (QSettings.Instance.KeyPagePrevious)) {
				QCategory.SelectPartPage (true);
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyPageNext)) {
				QCategory.SelectPartPage (false);
			}*/
			if (Input.GetKeyDown (QSettings.Instance.KeyPods)) {
				PartCategorizer.SetPanel_FunctionPods();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyFuelTanks)) {
				PartCategorizer.SetPanel_FunctionFuelTank();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyEngines)) {
				PartCategorizer.SetPanel_FunctionEngine();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyCommandNControl)) {
				PartCategorizer.SetPanel_FunctionControl();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyStructural)) {
				PartCategorizer.SetPanel_FunctionStructural();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyAerodynamics)) {
				PartCategorizer.SetPanel_FunctionAero();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeyUtility)) {
				PartCategorizer.SetPanel_FunctionUtility();
			}
			if (Input.GetKeyDown (QSettings.Instance.KeySciences)) {
				PartCategorizer.SetPanel_FunctionScience();
			}
		}
		internal static void OnGUI() {
			if (SetKey != Key.None) {
				RectSetKey = GUILayout.Window (1545146, RectSetKey, DrawSetKey, string.Format ("Set Key: {0}", GetText (SetKey)), GUILayout.Width (RectSetKey.width), GUILayout.ExpandHeight (true));
			}
		}
	}
}