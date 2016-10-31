/* 
QuickEngineer
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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace QuickEngineer {

	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public partial class QEditor : QuickEngineer { }

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public partial class QFlight : QuickEngineer { }

	public partial class QuickEngineer : MonoBehaviour {

		protected readonly static string VERSION = Assembly.GetAssembly(typeof(QuickEngineer)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickEngineer)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickEngineer)).GetName().Version.Build;
		protected readonly static string MOD = Assembly.GetAssembly(typeof(QuickEngineer)).GetName().Name;

		protected static void Log(string String, string Title = null) {
			if (Title == null) {
				Title = MOD;
			} else {
				Title = string.Format ("{0}({1})", MOD, Title);
			}
			if (QSettings.Instance.Debug) {
				Debug.Log (string.Format ("{0}[{1}]: {2}", Title, VERSION, String));
			}
		}
		protected static void Warning(string String, string Title = null) {
			if (Title == null) {
				Title = MOD;
			} else {
				Title = string.Format ("{0}({1})", MOD, Title);
			}
			Debug.LogWarning (string.Format ("{0}[{1}]: {2}", Title, VERSION, String));
		}

		protected static void Lock(bool activate) {
			if (!HighLogic.LoadedSceneIsEditor) {
				return;
			}
			if (activate) {
				EditorLogic.fetch.Lock (true, true, true, "Lock" + MOD);
			} else {
				EditorLogic.fetch.Unlock ("Lock" + MOD);
			}
			Log ("Lock: " + activate);
		}

		protected virtual void Awake() {
			Log ("Awake");
		}
		protected virtual void Start() {
			Log ("Start");
		}
		protected virtual void OnDestroy() {
			Log ("OnDestroy");
		}
	}
}