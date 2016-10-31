/* 
QuickBrake
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

using System.IO;

namespace QuickBrake {
	public class QSettings : QuickBrake {

		[KSPField(isPersistant = true)] static readonly QSettings instance = new QSettings ();
		public static QSettings Instance {
			get {
				if (!instance.isLoaded) {
					instance.Load ();
				}
				return instance;
			}
		}
		internal static string FileConfig = PATH + "/Config.txt";

		[KSPField(isPersistant = true)] bool isLoaded = false;

		[Persistent] internal bool Debug = true;

		[Persistent] internal bool EnableBrakeAtLaunchPad = false;
		[Persistent] internal bool EnableBrakeAtRunway = true;
		[Persistent] internal bool EnableBrakeAtControlLost = true;
		[Persistent] internal bool EnableUnBrakeAtLaunch = true;
		[Persistent] internal bool AlwaysBrakeLandedRover = true;
		[Persistent] internal bool AlwaysBrakeLandedBase = false;
		[Persistent] internal bool AlwaysBrakeLandedLander = false;
		[Persistent] internal bool AlwaysBrakeLandedPlane = true;
		[Persistent] internal bool AlwaysBrakeLandedVessel = false;

		[Persistent] internal bool StockToolBar = true;
		[Persistent] internal bool BlizzyToolBar = true;

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
				} catch {
					Save ();
				}
				Log ("Settings Loaded", "QSettings", true);
			} else {
				Save ();
			}	
			isLoaded = true;
		}
	}
}