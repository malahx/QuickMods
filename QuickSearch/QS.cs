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

using System.Reflection;
using QuickSearch.QUtils;
using QuickSearch.Toolbar;
using UnityEngine;

namespace QuickSearch {
	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public partial class QRnD : QuickSearch { }

	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public partial class QEditor : QuickSearch { }

	public partial class QuickSearch : MonoBehaviour {

		public readonly static string VERSION = Assembly.GetExecutingAssembly ().GetName ().Version.Major + "." + Assembly.GetExecutingAssembly ().GetName ().Version.Minor + Assembly.GetExecutingAssembly ().GetName ().Version.Build;
		public readonly static string MOD = Assembly.GetExecutingAssembly().GetName().Name;
		public readonly static string relativePath = "QuickMods/" + MOD;
		public readonly static string PATH = KSPUtil.ApplicationRootPath + "GameData/" + relativePath;

		//[KSPField(isPersistant = true)] static QBlizzy BlizzyToolbar;

		protected virtual void Awake() {
			//if (BlizzyToolbar == null) BlizzyToolbar = new QBlizzy ();
			TextField = new GUIStyle(HighLogic.Skin.textField);
			TextField.stretchWidth = true;
			TextField.stretchHeight = true;
			TextField.alignment = TextAnchor.MiddleCenter;
			QDebug.Log ("Awake");
		}
		
		protected virtual void Start() {
			//if (BlizzyToolbar != null) BlizzyToolbar.Init ();
			QDebug.Log ("Start");
		}
		
		protected virtual void OnDestroy() {
			//if (BlizzyToolbar != null) BlizzyToolbar.Destroy ();
			QDebug.Log ("OnDestroy");
		}
		
		protected virtual void FixedUpdate() {
			if (!WindowHistory) {
				return;
			}
			QHistory.Instance.Keys ();
		}
	}
}