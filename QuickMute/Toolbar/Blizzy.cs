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

using QuickMute.QUtils;

namespace QuickMute.Toolbar {
	public class QBlizzy {
	
		internal bool Enabled {
			get {
				return QSettings.Instance.BlizzyToolBar;
			}
		}

		void OnClick() { 
            QuickMute.Instance.Mute();
		}

		IButton Button;
		IButton ButtonConf;

		internal static bool isAvailable {
			get {
				return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
			}
		}

		internal void Init() {
			if (!HighLogic.LoadedSceneIsGame || !isAvailable || !Enabled) {
				return;
			}
			if (Button == null) {
				Button = ToolbarManager.Instance.add (QVars.MOD, QVars.MOD);
                Button.TexturePath = QTexture.BlizzyTexturePath;
				Button.ToolTip = QVars.MOD;
				Button.OnClick += (e) => OnClick ();
			}

			if (ButtonConf == null) {
				ButtonConf = ToolbarManager.Instance.add (QVars.MOD + "Conf", QVars.MOD + "Conf");
				ButtonConf.TexturePath = QTexture.BlizzyTexturePath;
				ButtonConf.ToolTip = QVars.MOD + ": " + QLang.translate ("Settings");
				ButtonConf.OnClick += (e) => QuickMute.gui.Settings ();
			}
			QDebug.Log ("Start", "QBlizzyToolbar");
		}

		internal void Destroy() {
			if (!isAvailable) {
				return;
			}

			if (Button != null) {
				Button.Destroy ();
				Button = null;
			}

			if (ButtonConf != null) {
				ButtonConf.Destroy ();
				ButtonConf = null;
			}
			QDebug.Log ("OnDestroy", "QBlizzyToolbar");
		}

		internal void Refresh() {
			if (!isAvailable || Button == null) {
				return;
			}
			Button.TexturePath = QTexture.BlizzyTexturePath;
			QDebug.Log ("Refresh", "QBlizzyToolbar");
		}

		internal void Reset() {
			if (Enabled) {
				Init ();
			} else {
				Destroy ();
			}
			QDebug.Log ("Reset", "QBlizzyToolbar");
		}
	}
}