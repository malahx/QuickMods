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

using System;
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
	}
}

