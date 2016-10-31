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
	public class QKey {
		#if SHORTCUT
		internal static Key SetKey = Key.None;
		internal static Rect RectSetKey = new Rect();

		internal enum Key {
			None,
			ModKeyFilterWheel,
			ModKeyCategoryWheel,
			ModKeyShortCut,
			FilterPrevious,
			FilterNext,
			CategoryPrevious,
			CategoryNext,
			PagePrevious,
			PageNext,
			Pods,
			FuelTanks,
			Engines,
			CommandNControl,
			Structural,
			Aerodynamics,
			Utility,
			Sciences
		}					
	
		internal static KeyCode DefaultKey(Key key) {
			switch (key) {
			case Key.ModKeyFilterWheel:
				return KeyCode.LeftShift;
			case Key.ModKeyCategoryWheel:
				return KeyCode.LeftControl;
			case Key.ModKeyShortCut:
				return KeyCode.KeypadEnter;
			case Key.FilterPrevious:
				return KeyCode.PageUp;
			case Key.FilterNext:
				return KeyCode.PageDown;
			case Key.CategoryPrevious:
				return KeyCode.UpArrow;
			case Key.CategoryNext:
				return KeyCode.DownArrow;
			case Key.PagePrevious:
				return KeyCode.LeftArrow;
			case Key.PageNext:
				return KeyCode.RightArrow;
			case Key.Pods:
				return KeyCode.Keypad1;
			case Key.FuelTanks:
				return KeyCode.Keypad2;
			case Key.Engines:
				return KeyCode.Keypad3;
			case Key.CommandNControl:
				return KeyCode.Keypad4;
			case Key.Structural:
				return KeyCode.Keypad5;
			case Key.Aerodynamics:
				return KeyCode.Keypad6;
			case Key.Utility:
				return KeyCode.Keypad7;
			case Key.Sciences:
				return KeyCode.Keypad8;
			}
			return KeyCode.None;
		}

		internal static string GetText(Key key) {
			switch (key) {
			case Key.ModKeyFilterWheel:
				return "Filter Scrolling";
			case Key.ModKeyCategoryWheel:
				return "Subcategory Scrolling";
			case Key.ModKeyShortCut:
				return "Modifier Keyboard";
			case Key.FilterPrevious:
				return "Previous Filter";
			case Key.FilterNext:
				return "Next Filter";
			case Key.CategoryPrevious:
				return "Previous Category";
			case Key.CategoryNext:
				return "Next Category";
			case Key.PagePrevious:
				return "Previous Parts Page";
			case Key.PageNext:
				return "Next Parts Page";
			case Key.Pods:
				return "Pods Category";
			case Key.FuelTanks:
				return "Fuel Tanks Category";
			case Key.Engines:
				return "Engines Category";
			case Key.CommandNControl:
				return "Com. and Ctrl Category";
			case Key.Structural:
				return "Structural Category";
			case Key.Aerodynamics:
				return "Aerodynamics Category";
			case Key.Utility:
				return "Utility Category";
			case Key.Sciences:
				return "Sciences Category";
			}
			return string.Empty;
		}

		internal static KeyCode CurrentKey(Key key) {
			switch (key) {
			case Key.ModKeyFilterWheel:
				return QSettings.Instance.ModKeyFilterWheel;
			case Key.ModKeyCategoryWheel:
				return QSettings.Instance.ModKeyCategoryWheel;
			case Key.ModKeyShortCut:
				return QSettings.Instance.ModKeyShortCut;
			case Key.FilterPrevious:
				return QSettings.Instance.KeyFilterPrevious;
			case Key.FilterNext:
				return QSettings.Instance.KeyFilterNext;
			case Key.CategoryPrevious:
				return QSettings.Instance.KeyCategoryPrevious;
			case Key.CategoryNext:
				return QSettings.Instance.KeyCategoryNext;
			/*case Key.PagePrevious:
				return QSettings.Instance.KeyPagePrevious;
			case Key.PageNext:
				return QSettings.Instance.KeyPageNext;*/
			case Key.Pods:
				return QSettings.Instance.KeyPods;
			case Key.FuelTanks:
				return QSettings.Instance.KeyFuelTanks;
			case Key.Engines:
				return QSettings.Instance.KeyEngines;
			case Key.CommandNControl:
				return QSettings.Instance.KeyCommandNControl;
			case Key.Structural:
				return QSettings.Instance.KeyStructural;
			case Key.Aerodynamics:
				return QSettings.Instance.KeyAerodynamics;
			case Key.Utility:
				return QSettings.Instance.KeyUtility;
			case Key.Sciences:
				return QSettings.Instance.KeySciences;
			}
			return KeyCode.None;
		}

		internal static void VerifyKey(Key key) {
			try {
				Input.GetKey(CurrentKey(key));
			} catch {
				QuickScroll.Warning ("Wrong key: " + CurrentKey(key),"QKey");
				SetCurrentKey (key, DefaultKey(key));
			}
		}

		internal static void VerifyKey() {
			string[] _keys = Enum.GetNames (typeof(Key));
			int _length = _keys.Length;
			for (int _key = 1; _key < _length; _key++) {
				VerifyKey ((Key)_key);
			}
		}

		internal static void SetCurrentKey(Key key, KeyCode CurrentKey) {
			switch (key) {
			case Key.ModKeyFilterWheel:
				QSettings.Instance.ModKeyFilterWheel = CurrentKey;
				break;
			case Key.ModKeyCategoryWheel:
				QSettings.Instance.ModKeyCategoryWheel = CurrentKey;
				break;
			case Key.ModKeyShortCut:
				QSettings.Instance.ModKeyShortCut = CurrentKey;
				break;
			case Key.FilterPrevious:
				QSettings.Instance.KeyFilterPrevious = CurrentKey;
				break;
			case Key.FilterNext:
				QSettings.Instance.KeyFilterNext = CurrentKey;
				break;
			case Key.CategoryPrevious:
				QSettings.Instance.KeyCategoryPrevious = CurrentKey;
				break;
			case Key.CategoryNext:
				QSettings.Instance.KeyCategoryNext = CurrentKey;
				break;
			/*case Key.PagePrevious:
				QSettings.Instance.KeyPagePrevious = CurrentKey;
				break;
			case Key.PageNext:
				QSettings.Instance.KeyPageNext = CurrentKey;
				break;*/
			case Key.Pods:
				QSettings.Instance.KeyPods = CurrentKey;
				break;
			case Key.FuelTanks:
				QSettings.Instance.KeyFuelTanks = CurrentKey;
				break;
			case Key.Engines:
				QSettings.Instance.KeyEngines = CurrentKey;
				break;
			case Key.CommandNControl:
				QSettings.Instance.KeyCommandNControl = CurrentKey;
				break;
			case Key.Structural:
				QSettings.Instance.KeyStructural = CurrentKey;
				break;
			case Key.Aerodynamics:
				QSettings.Instance.KeyAerodynamics = CurrentKey;
				break;
			case Key.Utility:
				QSettings.Instance.KeyUtility = CurrentKey;
				break;
			case Key.Sciences:
				QSettings.Instance.KeySciences = CurrentKey;
				break;
			}
		}
		#endif
		#if GUI
		internal static void DrawSetKey(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (string.Format("Press a key to select the <color=#FFFFFF><b>{0}</b></color>",GetText(SetKey)));
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("Clear Assignment", GUILayout.ExpandWidth(true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, KeyCode.None);
				SetKey = Key.None;
				QGUI.WindowSettings = true;
			}
			if (GUILayout.Button ("Default Assignment", GUILayout.ExpandWidth(true), GUILayout.Height (30))) {
				SetCurrentKey (SetKey, DefaultKey(SetKey));
				SetKey = Key.None;
				QGUI.WindowSettings = true;
			}
			if (GUILayout.Button ("Cancel Assignment", GUILayout.ExpandWidth(true), GUILayout.Height (30))) {
				SetKey = Key.None;
				QGUI.WindowSettings = true;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.EndVertical ();
		}

		internal static void DrawConfigKey(Key key) {
			GUILayout.BeginHorizontal();
			GUILayout.Label (string.Format("{0}: <color=#FFFFFF><b>{1}</b></color>", GetText(key), CurrentKey(key)), GUILayout.Width (250));
			GUILayout.Space(5);
			if (GUILayout.Button ("Set", GUILayout.Width (25), GUILayout.Height (20))) {
				SetKey = key;
				QGUI.WindowSettings = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
		}
		#endif
	}
}