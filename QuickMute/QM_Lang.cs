/* 
QuickMute
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

using LanguagePatches;
using UnityEngine;

namespace QuickMute {
	public class QLang : QuickMute {

		static readonly string[] langs = {
			"EN",
			"FR"
		};

		public static string translate(string text) {
			return translate (QSettings.Instance.Lang, text);
		}

		public static string translate (string lang, string text) {
			text = LanguageAPI.Translate (lang + "_" + text);
			if (text.Substring (0, lang.Length + 1) == lang + "_") {
				text = text.Substring (lang.Length + 1, text.Length - lang.Length - 1);
			}
			return text;
		}

		public static void DrawLang() {
			if (!LanguageAPI.hasLanguagePatches) {
				return;
			}
			GUILayout.BeginHorizontal ();
			GUILayout.Box (QLang.translate ("Languages"), GUILayout.Height (30));
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			for (int i = 0; i < langs.Length; i++) {
				string lang = langs[i];
				if (GUILayout.Toggle (QSettings.Instance.Lang == lang, LanguageAPI.Translate (lang), GUILayout.Width (140))) {
					QSettings.Instance.Lang = lang;
				}
				GUILayout.FlexibleSpace ();
				if ((i % 2) == 1) {
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
				}
			}
			GUILayout.EndHorizontal ();
		}
	}
}