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

using System.Collections.Generic;
using UnityEngine;

namespace QuickSearch {
	static class QUtils {

		internal static class Texture {
			internal static readonly string SEARCH_PATH = QuickSearch.relativePath + "/Textures/search";
			internal static readonly string STOCKTOOLBAR_PATH = QuickSearch.relativePath + "/Textures/StockToolBar";
			internal static readonly string BLIZZY_PATH = QuickSearch.relativePath + "/Textures/BlizzyToolBar";
			internal static readonly string DELETE_PATH = QuickSearch.relativePath + "/Textures/delete";

			static Texture2D delete;
			internal static Texture2D Delete {
				get {
					if (delete == null) {
						delete = GameDatabase.Instance.GetTexture (DELETE_PATH, false);
					}
					return delete;
				}
			}

			static Texture2D stocktoolbar;
			internal static Texture2D Stocktoolbar {
				get {
					if (stocktoolbar == null) {
						stocktoolbar = GameDatabase.Instance.GetTexture (STOCKTOOLBAR_PATH, false);
					}
					return stocktoolbar;
				}
			}

			static Texture2D search;
			internal static Texture2D Search {
				get {
					if (search == null) {
						search = GameDatabase.Instance.GetTexture (SEARCH_PATH, false);
					}
					return search;
				}
			}
		}

		internal static Texture2D ColorToTex(Vector2 dim, Color col) {
			Color[] pix = new Color[(int)dim.x * (int)dim.y];
			for (int i = pix.Length -1; i >= 0; i--) {
				pix[i] = col;
			}
			Texture2D result = new Texture2D ((int)dim.x, (int)dim.y);
			result.SetPixels (pix);
			result.Apply ();
			return result;
		}

		internal static void SortBy(this List<QHistory.Search> h, int type) {
			switch (type) {
				case (int)QHistory.SortBy.COUNT:
					h.Sort ((a, b) => b.count.CompareTo (a.count));
					break;
				case (int)QHistory.SortBy.DATE:
					h.Sort ((a, b) => b.date.CompareTo (a.date));
					break;
				case (int)QHistory.SortBy.NAME:
					h.Sort ((a, b) => string.Compare (b.text, a.text, System.StringComparison.Ordinal));
					break;
			}
		}

		internal static bool Contains(this List<QHistory.Search> h, string t) {
			return h.Get (t) != null;
		}

		internal static QHistory.Search Get(this List<QHistory.Search> h, string text) {
			for (int i = h.Count - 1; i >= 0; i--) {
				QHistory.Search s = h[i];
				if (s.text == text) {
					return s;
				}
			}
			return null;
		}

		internal static void Lock(bool activate, ControlTypes Ctrl) {
			if (HighLogic.LoadedSceneIsEditor) {
				if (activate) {
					if (InputLockManager.GetControlLock ("EditorLock" + QuickSearch.MOD) == ControlTypes.None) {
						EditorLogic.fetch.Lock (true, true, true, "EditorLock" + QuickSearch.MOD);
					}
				}
				else {
					if (InputLockManager.GetControlLock ("EditorLock" + QuickSearch.MOD) != ControlTypes.None) {
						EditorLogic.fetch.Unlock ("EditorLock" + QuickSearch.MOD);
					}
				}
			}
			if (activate) {
				if (InputLockManager.GetControlLock ("Lock" + QuickSearch.MOD) == ControlTypes.None) {
					InputLockManager.SetControlLock (Ctrl, "Lock" + QuickSearch.MOD);
				}
				return;
			}
			if (InputLockManager.GetControlLock ("Lock" + QuickSearch.MOD) != ControlTypes.None) {
				InputLockManager.RemoveControlLock ("Lock" + QuickSearch.MOD);
			}
		}

		internal static void Lock(bool activate) {
			Lock (activate, ControlTypes.KSC_ALL | ControlTypes.TRACKINGSTATION_UI | ControlTypes.CAMERACONTROLS | ControlTypes.MAP);
		}
	}
}

