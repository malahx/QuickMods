/* 
QuickHide
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

using System.Collections.Generic;
using System.IO;

namespace QuickHide {
	public class QSettings : QuickHide {

		[KSPField (isPersistant = true)] static readonly QSettings instance = new QSettings ();
		public static QSettings Instance {
			get {
				if (!instance.isLoaded) {
					instance.Load ();
				}
				return instance;
			}
		}
		new internal static string FileConfig = RegisterToolbar.PATH + "/Config.txt";

		[KSPField (isPersistant = true)] bool isLoaded = false;

		[Persistent] public bool Debug = false;
		[Persistent] public bool isHidden = false;
		[Persistent] public bool HideAppLauncher = true;
		[Persistent] public bool HideStage = true;
		[Persistent] public int TimeToKeep = 2;
		[Persistent] public bool StockToolBar = true;
		[Persistent] public bool BlizzyToolBar = true;
		[Persistent] public bool StockToolBar_ModApp = true;
		[Persistent] public List<string> CanPin = new List<string>(); 
		[Persistent] public List<string> CanHide = new List<string>();
		[Persistent] public List<string> CanSetFalse = new List<string>();
		[Persistent] public List<string> ModHasFirstConfig = new List<string>();

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
					Log ("Settings Loaded", "QSettings", true);
				}
				catch {
					Save ();
				}
			}
			else {
				Save ();
			}
			isLoaded = true;
		}
	}
}