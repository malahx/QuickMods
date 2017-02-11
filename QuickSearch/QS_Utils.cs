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
	internal static class QS_Utils {
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

		public static void SortBy(this List<QHistory.Search> h, int type) {
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

		public static bool Contains(this List<QHistory.Search> h, string t) {
			return h.Get (t) != null;
		}

		public static QHistory.Search Get(this List<QHistory.Search> h, string text) {
			for (int i = h.Count - 1; i >= 0; i--) {
				QHistory.Search s = h[i];
				if (s.text == text) {
					return s;
				}
			}
			return null;
		}
	}
}

