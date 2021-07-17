/* 
QuickGoTo
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

namespace QuickGoTo {
	public class QSettings : QuickGoTo {
		
		[KSPField(isPersistant = true)] static readonly QSettings instance = new QSettings ();
		public static QSettings Instance {
			get {
				if (!instance.isLoaded) {
					instance.Load ();
				}
				return instance;
			}
		}
		new internal static string FileConfig = RegisterToolbar.PATH + "/Config.txt";

		[KSPField(isPersistant = true)] bool isLoaded = false;

		[Persistent] public bool Debug = true;

		[Persistent] public bool EnableGoToTrackingStation = true;
		[Persistent] public bool EnableGoToSpaceCenter = true;
		[Persistent] public bool EnableGoToMissionControl = true;
		[Persistent] public bool EnableGoToAdministration = true;
		[Persistent] public bool EnableGoToRnD = true;
		[Persistent] public bool EnableGoToAstronautComplex = true;
		[Persistent] public bool EnableGoToVAB = true;
		[Persistent] public bool EnableGoToSPH = true;
		[Persistent] public bool EnableGoToLastVessel = true;
		[Persistent] public bool EnableGoToRecover = true;
		[Persistent] public bool EnableGoToRevert = true;
		[Persistent] public bool EnableGoToRevertToEditor = true;
		[Persistent] public bool EnableGoToRevertToSpaceCenter = true;
		[Persistent] public bool EnableGoToMainMenu = false;
		[Persistent] public bool EnableGoToSettings = false;
		[Persistent] public bool EnableSettings = false;

		[Persistent] public bool StockToolBar = true;
		[Persistent] public bool BlizzyToolBar = true;
		[Persistent] public bool StockToolBar_ModApp = true;
		[Persistent] public bool StockToolBar_OnHover = true;
		[Persistent] public bool EnableBatButton = true;

		//[Persistent] public bool KSPSkin = true;
		[Persistent] public bool ImageOnly = true;
		[Persistent] public bool LockHover = false;
		[Persistent] public bool CenterText = false;

		[Persistent] public bool EnableQuickExit = false;
		[Persistent] public bool EnableQuickScroll = false;
		[Persistent] public bool EnableQuickRevert = false;
		[Persistent] public bool EnableQuickHide = false;
		[Persistent] public bool EnableQuickMute = false;
		[Persistent] public bool EnableQuickIVA = false;

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