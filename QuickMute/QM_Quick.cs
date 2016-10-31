/* 
QuickMute
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

using System;
using System.Reflection;
using UnityEngine;

namespace QuickMute {

	public class Quick : MonoBehaviour {

		public readonly static string VERSION = Assembly.GetAssembly(typeof(QuickMute)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickMute)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickMute)).GetName().Version.Build;
		public readonly static string MOD = Assembly.GetAssembly(typeof(QuickMute)).GetName().Name;
		private static bool isdebug = true;

		// Afficher les messages sur la console
		internal static void Log(string _string) {
			if (isdebug) {
				Debug.Log (MOD + "(" + VERSION + "): " + _string);
			}
		}
		internal static void Warning(string _string) {
			if (isdebug) {
				Debug.LogWarning (MOD + "(" + VERSION + "): " + _string);
			}
		}
	}
}