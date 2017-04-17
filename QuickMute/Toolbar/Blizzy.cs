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

using QuickMute.Object;

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

		IButton button;
        IButton buttonVol;
        IButton buttonConf;

		internal static bool isAvailable {
			get {
				return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
			}
		}

		internal void Init() {
			if (!HighLogic.LoadedSceneIsGame || !isAvailable || !Enabled) {
				return;
			}
			if (button == null) {
				button = ToolbarManager.Instance.add (QVars.MOD, QVars.MOD);
                button.TexturePath = QTexture.BlizzyTexturePath;
				button.ToolTip = QVars.MOD;
				button.OnClick += (e) => OnClick ();
			}
            if (buttonVol == null) {
                buttonVol = ToolbarManager.Instance.add(QVars.MOD + "Vol", QVars.MOD + "Vol");
                buttonVol.TexturePath = QTexture.BLIZZY_PATH_VOL;
                buttonVol.ToolTip = QVars.MOD + ": " + QLang.translate("Volume");
                buttonVol.OnClick += (e) => QuickMute.Instance.gui.level.Show();
            }
			if (buttonConf == null) {
				buttonConf = ToolbarManager.Instance.add (QVars.MOD + "Conf", QVars.MOD + "Conf");
				buttonConf.TexturePath = QTexture.BLIZZY_PATH_CONF;
				buttonConf.ToolTip = QVars.MOD + ": " + QLang.translate ("Settings");
				buttonConf.OnClick += (e) => QuickMute.Instance.gui.Settings ();
			}
			QDebug.Log ("Start", "QBlizzyToolbar");
		}

		internal void Destroy() {
			if (!isAvailable) {
				return;
			}

			if (button != null) {
				button.Destroy ();
				button = null;
			}

            if (buttonVol != null) {
                buttonVol.Destroy();
                buttonVol = null;
            }

			if (buttonConf != null) {
				buttonConf.Destroy ();
				buttonConf = null;
			}
			QDebug.Log ("OnDestroy", "QBlizzyToolbar");
		}

		internal void Refresh() {
			if (!isAvailable || button == null) {
				return;
			}
			button.TexturePath = QTexture.BlizzyTexturePath;
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