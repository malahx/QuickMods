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

		string TexturePathSound = QuickMute.relativePath + "/Textures/BlizzyToolBar_sound";
		string TexturePathMute = QuickMute.relativePath + "/Textures/BlizzyToolBar_mute";
		string TexturePathConf = QuickMute.relativePath + "/Textures/BlizzyConf";

		void OnClick() { 
			QuickMute.Instance.Mute ();
		}

		string TexturePath {
			get {
				return (QSettings.Instance.Muted ? TexturePathMute : TexturePathSound);
			}
		}

		IButton Button;
		IButton ButtonConf;

		internal static bool isAvailable {
			get {
				return ToolbarManager.ToolbarAvailable && ToolbarManager.Instance != null;
			}
		}

		internal void Start() {
			if (!HighLogic.LoadedSceneIsGame || !isAvailable || !Enabled) {
				return;
			}
			if (Button == null) {
				Button = ToolbarManager.Instance.add (QuickMute.MOD, QuickMute.MOD);
				Button.TexturePath = TexturePath;
				Button.ToolTip = QuickMute.MOD;
				Button.OnClick += (e) => OnClick ();
			}

			if (ButtonConf == null) {
				ButtonConf = ToolbarManager.Instance.add (QuickMute.MOD + "Conf", QuickMute.MOD + "Conf");
				ButtonConf.TexturePath = TexturePathConf;
				ButtonConf.ToolTip = QuickMute.MOD + ": " + QLang.translate ("Settings");
				ButtonConf.OnClick += (e) => QuickMute.Instance.Settings ();
			}
		}

		internal void OnDestroy() {
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