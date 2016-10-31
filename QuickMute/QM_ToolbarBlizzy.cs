/* 
QuickMute
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

namespace QuickMute {
	public class QBlizzyToolbar {
	
		internal bool Enabled {
			get {
				return QSettings.Instance.BlizzyToolBar;
			}
		}
		private string TexturePathSound = QuickMute.relativePath + "/Textures/BlizzyToolBar_sound";
		private string TexturePathMute = QuickMute.relativePath + "/Textures/BlizzyToolBar_mute";

		private void OnClick() { 
			QuickMute.Instance.Mute ();
		}

		private string TexturePath {
			get {
				return (QSettings.Instance.Muted ? TexturePathMute : TexturePathSound);
			}
		}

		private IButton Button;

		internal static bool isAvailable {
			get {
				return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
			}
		}

		internal void Start() {
			if (!HighLogic.LoadedSceneIsGame || !isAvailable || !Enabled || Button != null) {
				return;
			}
			Button = ToolbarManager.Instance.add (QuickMute.MOD, QuickMute.MOD);
			Button.TexturePath = TexturePath;
			Button.ToolTip = QuickMute.MOD;
			Button.OnClick += (e) => OnClick ();
		}

		internal void OnDestroy() {
			if (!isAvailable || Button == null) {
				return;
			}
			Button.Destroy ();
			Button = null;
		}

		internal void Refresh() {
			if (!isAvailable || Button == null) {
				return;
			}
			Button.TexturePath = TexturePath;
		}

		internal void Reset() {
			if (Enabled) {
				Start ();
			} else {
				OnDestroy ();
			}
		}
	}
}