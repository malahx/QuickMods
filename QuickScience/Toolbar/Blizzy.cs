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

using KSP.Localization;
using QuickMute.Utils;

namespace QuickScience.Toolbar {
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
                button = ToolbarManager.Instance.add(QVars.MOD, QVars.MOD);
                button.TexturePath = QTexture.BlizzyTexturePath;
                button.ToolTip = QVars.MOD;
                button.OnClick += (e) => OnClick();
            }
            QDebug.Log("Start", "QBlizzyToolbar");
        }

        internal void Destroy() {
            if (!isAvailable) {
                return;
            }
            if (button != null) {
                button.Destroy();
                button = null;
            }
            QDebug.Log("OnDestroy", "QBlizzyToolbar");
        }

        internal void Refresh() {
            if (!isAvailable || button == null) {
                return;
            }
            button.TexturePath = QTexture.BlizzyTexturePath;
            QDebug.Log("Refresh", "QBlizzyToolbar");
        }

        internal void Reset() {
            if (Enabled) {
                Init();
            } else {
                Destroy();
            }
            QDebug.Log("Reset", "QBlizzyToolbar");
        }
    }
}