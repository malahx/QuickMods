/* 
QuickContracts
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
using UnityEngine;

namespace QuickContracts {
	public class QSettings : QuickContracts {

		[KSPField (isPersistant = true)]
		static readonly QSettings instance = new QSettings ();
		public static QSettings Instance {
			get {
				if (!instance.isLoaded) {
					instance.Load ();
				}
				return instance;
			}
		}
		string FileConfig = PATH + "/Config.txt";

		[KSPField (isPersistant = true)] private bool isLoaded = false;

		[Persistent] internal bool Debug = true;
		[Persistent] internal KeyCode KeyDeclineSelectedContract;
		[Persistent] internal KeyCode KeyDeclineAllContracts;
		[Persistent] internal KeyCode KeyDeclineAllTest;
		[Persistent] internal KeyCode KeyAcceptSelectedContract;
		[Persistent] internal bool EnableMessage = true;
		[Persistent] internal string Lang = "EN";

		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(FileConfig);
			Log ("Settings Saved", "QSettings", true);
		}
		public void Load() {
			if (File.Exists (FileConfig)) {
				try {
					ConfigNode _temp = ConfigNode.Load (FileConfig);
					ConfigNode.LoadObjectFromConfig (this, _temp);
				}
				catch {
					Save ();
				}
				Log ("Settings Loaded", "QSettings", true);
			}
			else {
				Save ();
			}
			isLoaded = true;
		}
	}
}