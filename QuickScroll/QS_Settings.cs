/* 
QuickScroll
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

using System;
using System.IO;
using UnityEngine;

namespace QuickScroll {
	public class QSettings {

		public static QSettings Instance = new QSettings ();

		private string FileConfig = KSPUtil.ApplicationRootPath + "GameData/" + QuickScroll.MOD + "/Config.txt";

		[Persistent] internal bool Debug = true;

		[Persistent] public bool StockToolBar = true;
		[Persistent] public bool BlizzyToolBar = true;
		[Persistent] public bool StockToolBarHovering = false;

		[Persistent] internal bool EnableWheelBlockTopEnd = true;
		//[Persistent] internal bool EnableTWEAKPartListTooltips = false;

		[Persistent] internal string KeyPartListTooltipsActivate = "mouse 1";
		[Persistent] internal string KeyPartListTooltipsDisactivate = "mouse 0";

		[Persistent] internal bool EnableWheelScroll = true;
		[Persistent] internal bool EnableWheelShortCut = true;

		[Persistent] internal bool EnableKeyShortCut = true;

		[Persistent] internal KeyCode ModKeyFilterWheel 		= QShortCuts.DefaultKey(QKey.Key.ModKeyFilterWheel);
		[Persistent] internal KeyCode ModKeyCategoryWheel 		= QShortCuts.DefaultKey(QKey.Key.ModKeyCategoryWheel);

		[Persistent] internal KeyCode ModKeyShortCut 			= QShortCuts.DefaultKey(QKey.Key.ModKeyShortCut);

		[Persistent] internal KeyCode KeyFilterPrevious 		= QShortCuts.DefaultKey(QKey.Key.FilterPrevious);
		[Persistent] internal KeyCode KeyFilterNext 			= QShortCuts.DefaultKey(QKey.Key.FilterNext);
		[Persistent] internal KeyCode KeyCategoryPrevious 		= QShortCuts.DefaultKey(QKey.Key.CategoryPrevious);
		[Persistent] internal KeyCode KeyCategoryNext 			= QShortCuts.DefaultKey(QKey.Key.CategoryNext);
		//[Persistent] internal KeyCode KeyPagePrevious 			= QShortCuts.DefaultKey(QKey.Key.PagePrevious);
		//[Persistent] internal KeyCode KeyPageNext 				= QShortCuts.DefaultKey(QKey.Key.PageNext);
		[Persistent] internal KeyCode KeyPods 					= QShortCuts.DefaultKey(QKey.Key.Pods);
		[Persistent] internal KeyCode KeyFuelTanks 				= QShortCuts.DefaultKey(QKey.Key.FuelTanks);
		[Persistent] internal KeyCode KeyEngines 				= QShortCuts.DefaultKey(QKey.Key.Engines);
		[Persistent] internal KeyCode KeyCommandNControl 		= QShortCuts.DefaultKey(QKey.Key.CommandNControl);
		[Persistent] internal KeyCode KeyStructural 			= QShortCuts.DefaultKey(QKey.Key.Structural);
		[Persistent] internal KeyCode KeyAerodynamics 			= QShortCuts.DefaultKey(QKey.Key.Aerodynamics);
		[Persistent] internal KeyCode KeyUtility 				= QShortCuts.DefaultKey(QKey.Key.Utility);
		[Persistent] internal KeyCode KeySciences 				= QShortCuts.DefaultKey(QKey.Key.Sciences);

		// GESTION DE LA CONFIGURATION
		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(FileConfig);
			QuickScroll.Log ("Settings Saved", "QSettings");
		}
		public void Load() {
			if (File.Exists (FileConfig)) {
				try {
					ConfigNode _temp = ConfigNode.Load (FileConfig);
					ConfigNode.LoadObjectFromConfig (this, _temp);
				} catch {
					Save ();
				}
				QuickScroll.Log ("Settings Loaded", "QSettings");
			} else {
				Save ();
			}
		}
	}
}