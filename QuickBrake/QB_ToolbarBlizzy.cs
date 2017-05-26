/* 
QuickBrake
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

namespace QuickBrake
{
	public class QBlizzyToolbar
	{

		internal bool Enabled {
			get {
				return QSettings.Instance.BlizzyToolBar;
			}
		}

		string TexturePath = QuickBrake.relativePath + "/Textures/BlizzyToolBar";

		IButton Button;

		public static GameScenes[] AppScenes = {
			GameScenes.SPACECENTER
		};

		internal static bool isAvailable {
			get {
				return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
			}
		}
						
		void OnClick () {
			QGUI.Instance.Settings ();
		}


		internal void Start () {
			if (!HighLogic.LoadedSceneIsGame || !QBlizzyToolbar.isAvailable || !Enabled || Button != null) {
				return;
			}
			Button = ToolbarManager.Instance.add (QuickBrake.MOD, QuickBrake.MOD);
			Button.TexturePath = TexturePath;
            Button.ToolTip = QuickBrake.MOD + ": " + Localizer.Format("quickbrake_settings");
			Button.OnClick += (e) => OnClick ();
			Button.Visibility = new GameScenesVisibility (QBlizzyToolbar.AppScenes);
		}

		internal void OnDestroy () {
			if (!QBlizzyToolbar.isAvailable || Button == null) {
				return;
			}
			Button.Destroy ();
			Button = null;
		}

		internal void Reset () {
			if (Enabled) {
				Start ();
			}
			else {
				OnDestroy ();
			}
		}
	}
}