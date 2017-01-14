/* 
QuickRevert
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

using System.Reflection;
using UnityEngine;

namespace QuickRevert {

	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public partial class QStockToolbar : QuickRevert  { }

	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public partial class QDataReset : QuickRevert { }

	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public partial class QGUI : QuickRevert { }

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public partial class QFlight : QuickRevert { }

	public class QuickRevert : MonoBehaviour {

		public readonly static string VERSION = Assembly.GetExecutingAssembly ().GetName ().Version.Major + "." + Assembly.GetExecutingAssembly ().GetName ().Version.Minor + Assembly.GetExecutingAssembly ().GetName ().Version.Build;
		public readonly static string MOD = Assembly.GetExecutingAssembly ().GetName ().Name;
		public readonly static string relativePath = "QuickMods/" + MOD;
		public readonly static string PATH = KSPUtil.ApplicationRootPath + "GameData/" + relativePath;

		internal static void Log(string String, string Title = null, bool force = false) {
			if (!force) {
				if (!QSettings.Instance.Debug) {
					return;
				}
			}
			if (Title == null) {
				Title = MOD;
			} else {
				Title = string.Format ("{0}({1})", MOD, Title);
			}
			Debug.Log (string.Format ("{0}[{1}]: {2}", Title, VERSION, String));
		}
		protected static void Warning(string String, string Title = null) {
			if (Title == null) {
				Title = MOD;
			} else {
				Title = string.Format ("{0}({1})", MOD, Title);
			}
			Debug.LogWarning (string.Format ("{0}[{1}]: {2}", Title, VERSION, String));
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