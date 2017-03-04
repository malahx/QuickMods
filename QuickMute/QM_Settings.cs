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

using System.IO;
using QuickMute.QUtils;
using UnityEngine;

namespace QuickMute {
	public class QSettings {

		[KSPField(isPersistant = true)] static readonly QSettings instance = new QSettings ();
		public static QSettings Instance {
			get {
				if (!instance.isLoaded) {
					instance.Load ();
				}
				return instance;
			}
		}

		internal static string FileConfig = QVars.PATH + "/Config.txt";

		[KSPField(isPersistant = true)]	bool isLoaded = false;

		[Persistent] internal bool Debug = true;

		[Persistent] internal KeyCode KeyMute = QKey.DefaultKey (QKey.Key.Mute);
		[Persistent] internal bool Muted = false;
        [Persistent] internal bool MuteIcon = true;

		[Persistent] internal bool StockToolBar = true;
		[Persistent] internal bool BlizzyToolBar = true;

		[Persistent] internal float AMBIENCE_VOLUME = 0;
		[Persistent] internal float MUSIC_VOLUME = 0;
		[Persistent] internal float SHIP_VOLUME = 0;
		[Persistent] internal float UI_VOLUME = 0;
		[Persistent] internal float VOICE_VOLUME = 0;

		[Persistent] internal string Lang = "EN";

		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(FileConfig);
			QDebug.Log ("Settings Saved", "QSettings",  true);
		}
		public void Load() {
			if (File.Exists (FileConfig)) {
				try {
					ConfigNode _temp = ConfigNode.Load (FileConfig);
					ConfigNode.LoadObjectFromConfig (this, _temp);
				} catch {
					Save ();
				}
				QDebug.Log ("Settings Loaded", "QSettings",  true);
			} else {
				Save ();
			}
			isLoaded = true;
		}
	}
}