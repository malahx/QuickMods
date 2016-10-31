/* 
QuickEngineer
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

using System;
using System.IO;
using UnityEngine;

namespace QuickEngineer {
	public class QSettings : QuickEngineer {

		public readonly static QSettings Instance = new QSettings();

		private string FileConfig = KSPUtil.ApplicationRootPath + "GameData/" + MOD + "/Config.txt";

		[Persistent] internal bool Debug = false;

		[Persistent] internal bool AllVesselEngineer_Disable = false;
		[Persistent] internal bool FlightVesselEngineer_Disable = false;

		[Persistent] internal bool VesselEngineer_hidedeltaV = false;
		[Persistent] internal bool VesselEngineer_hideTWR = false;
		[Persistent] internal bool EditorVesselEngineer_Simple = false;

		[Persistent] internal bool VesselEngineer_hideEmptyStages = true;
		[Persistent] internal bool VesselEngineer_showEmptyTWR = true;
		[Persistent] internal bool VesselEngineer_showStageTotaldV = false;
		[Persistent] internal bool VesselEngineer_showStageInverseTotaldV = true;

		[Persistent] internal bool StockToolBar_ModApp = true;

		// GESTION DE LA CONFIGURATION
		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(FileConfig);
			Log ("Settings Saved","QSettings");
		}
		public void Load() {
			if (File.Exists (FileConfig)) {
				try {
					ConfigNode _temp = ConfigNode.Load (FileConfig);
					ConfigNode.LoadObjectFromConfig (this, _temp);
				} catch {
					Save ();
				}
				Log ("Settings Loaded","QSettings");
			} else {
				Save ();
			}
		}
	}
}