/* 
QuickScience
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

namespace QuickScience
{
	public class QBlizzyToolbar
	{

		internal bool Enabled {
			get {
				return QSettings.Instance.BlizzyToolBar;
			}
		}

		string TexturePathCollect = QuickScience.relativePath + "/Textures/BlizzyToolBarCollect";
		string TexturePathTest = QuickScience.relativePath + "/Textures/BlizzyToolBarTest";
		string TexturePathNoTest = QuickScience.relativePath + "/Textures/BlizzyToolBarNoTest";
		public string TexturePath {
			get {
				return HighLogic.LoadedScene == GameScenes.SPACECENTER || QScience.Instance.Experiments.hasEmptyTest () ?
								TexturePathTest:
								TexturePathNoTest;
			}
		}

		IButton Button;
		IButton ButtonCollect;

		public GameScenes[] AppScenes = {
			GameScenes.SPACECENTER,
			GameScenes.FLIGHT
		};
		public GameScenes[] AppScenesCollect = {
			GameScenes.FLIGHT
		};

		internal static bool isAvailable {
			get {
				return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
			}
		}
						
		void OnClick () {
			if (HighLogic.LoadedSceneIsFlight) {
				QScience.Instance.TestAll ();
			}
			else {
				QGUI.Instance.Settings ();
			}
		}

		void Collect() {
			QScience.Instance.CollectAll ();
		}

		internal void Init () {
			if (!HighLogic.LoadedSceneIsGame || !QBlizzyToolbar.isAvailable || !Enabled) {
				return;
			}
			if (Button == null) {
				Button = ToolbarManager.Instance.add (QuickScience.MOD, QuickScience.MOD + "TestAll");
				Button.TexturePath = TexturePath;
				Button.ToolTip = QuickScience.MOD + ": " + QLang.translate (HighLogic.LoadedScene == GameScenes.SPACECENTER ? "Settings" : "Test All");
				Button.OnClick += (e) => OnClick ();
				Button.Visibility = new GameScenesVisibility (AppScenes);
			}
			if (ButtonCollect == null) {
				ButtonCollect = ToolbarManager.Instance.add (QuickScience.MOD, QuickScience.MOD + "CollectAll");
				ButtonCollect.TexturePath = TexturePathCollect;
				ButtonCollect.ToolTip = QuickScience.MOD + ": " + QLang.translate ("Collect All");
				ButtonCollect.OnClick += (e) => Collect ();
				ButtonCollect.Visibility = new GameScenesVisibility (AppScenesCollect);
			}
			QuickScience.Log ("Init", "QBlizzyToolbar");
		}

		internal void Destroy () {
			if (!QBlizzyToolbar.isAvailable) {
				return;
			}
			if (Button != null) {
				Button.Destroy ();
				Button = null;
			}
			if (ButtonCollect != null) {
				ButtonCollect.Destroy ();
				ButtonCollect = null;
			}
			QuickScience.Log ("Destroy", "QBlizzyToolbar");
		}

		internal void Reset () {
			if (Enabled) {
				Init ();
			}
			else {
				Destroy ();
			}
			QuickScience.Log ("Reset", "QBlizzyToolbar");
		}

		internal void Refresh() {
			if (Button != null) {
				Button.TexturePath = TexturePath;
			}
			QuickScience.Log ("Refresh", "QBlizzyToolbar");
		}
	}
}