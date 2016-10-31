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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace QuickScroll {

	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public partial class QuickScroll : MonoBehaviour {

		internal readonly static string VERSION = Assembly.GetAssembly(typeof(QuickScroll)).GetName().Version.Major + "." + Assembly.GetAssembly(typeof(QuickScroll)).GetName().Version.Minor + Assembly.GetAssembly(typeof(QuickScroll)).GetName().Version.Build;
		internal readonly static string MOD = Assembly.GetAssembly(typeof(QuickScroll)).GetName().Name;

		internal static void Log(string String, string Title = null) {
			if (Title == null) {
				Title = MOD;
			} else {
				Title = string.Format ("{0}({1})", MOD, Title);
			}
			if (QSettings.Instance.Debug) {
				Debug.Log (string.Format ("{0}[{1}]: {2}", Title, VERSION, String));
			}
		}
		internal static void Warning(string String, string Title = null) {
			if (Title == null) {
				Title = MOD;
			} else {
				Title = string.Format ("{0}({1})", MOD, Title);
			}
			Debug.LogWarning (string.Format ("{0}[{1}]: {2}", Title, VERSION, String));
		}

		public static QuickScroll Instance;

		[KSPField(isPersistant = true)] internal static QBlizzyToolbar BlizzyToolbar;

		// Initialisation des modules
		private void Awake() {
			if (Instance != null) {
				Destroy (this);
				Warning ("There's already an Instance of " + MOD);
				return;
			}
			Instance = this;
			if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzyToolbar ();
			QGUI.Awake ();
			QShortCuts.Awake ();
			Log ("Awake");
		}

		// Initialisation des variables
		private void Start() {
			QSettings.Instance.Load ();
			BlizzyToolbar.Start ();
			QShortCuts.VerifyKey ();
			//QCategory.PartListTooltipsTWEAK (false);
			Log ("Start");
		}

		// Arrêter le plugin
		private void OnDestroy() {
			BlizzyToolbar.OnDestroy ();
			Log ("OnDestroy");
		}

		// Gérer les raccourcis
		private void Update() {
			QShortCuts.Update ();
			QScroll.Update ();
		}

		/*private void LateUpdate() {
			QCategory.PartListTooltipsTWEAK();
		}*/

		// Gérer l'interface
		private void OnGUI() {
			GUI.skin = HighLogic.Skin;
			QShortCuts.OnGUI ();
			QGUI.OnGUI ();
		}
	}
}
