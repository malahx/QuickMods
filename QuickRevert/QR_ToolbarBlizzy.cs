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

using KSP.Localization;

namespace QuickRevert {
	public class QBlizzyToolbar {
	
		internal bool Enabled {
			get {
				return QSettings.Instance.BlizzyToolBar;
			}
		}
		string TexturePath = QuickRevert.relativePath + "/Textures/BlizzyToolBar";
		public static GameScenes[] AppScenes = {
			GameScenes.SPACECENTER
		};
		void OnClick() { 
			QGUI.Instance.Settings ();
			QuickRevert.Log ("OnClick", "QBlizzyToolbar");
		}

		IButton Button;

		internal static bool isAvailable {
			get {
				return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
			}
		}

		internal void Init() {
			if (!HighLogic.LoadedSceneIsGame || !isAvailable || !Enabled || Button != null) {
				return;
			}
			Button = ToolbarManager.Instance.add (QuickRevert.MOD, QuickRevert.MOD);
			Button.TexturePath = TexturePath;
			Button.ToolTip = QuickRevert.MOD + ": " + Localizer.Format("quickrevert_settings");
			Button.OnClick += (e) => OnClick ();
			Button.Visibility = new GameScenesVisibility(AppScenes);
			QuickRevert.Log ("Init", "QBlizzyToolbar");
		}

		internal void Destroy() {
			if (!isAvailable || Button == null) {
				return;
			}
			Button.Destroy ();
			Button = null;
			QuickRevert.Log ("Destroy", "QBlizzyToolbar");
		}

		internal void Reset() {
			if (Enabled) {
				Init ();
			} else {
				Destroy ();
			}
			QuickRevert.Log ("Reset", "QBlizzyToolbar");
		}
	}
}