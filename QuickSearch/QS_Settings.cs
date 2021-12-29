/* 
QuickSearch
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
using QuickSearch.QUtils;
using UnityEngine;

namespace QuickSearch {
	public class QSettings {

		[KSPField (isPersistant = true)] static readonly QSettings instance = new QSettings ();
		public static QSettings Instance {
			get {
				if (!instance.isLoaded) {
					instance.Load ();
				}
				return instance;
			}
		}

		readonly string FileConfig = RegisterToolbar.PATH + "/Config.txt";

		[KSPField (isPersistant = true)] bool isLoaded = false;

		[Persistent] internal bool Debug = true;
        

		[Persistent] internal bool EditorSearch = true;
		[Persistent] internal bool RnDSearch = true;
		[Persistent] internal bool enableSearchExtension = true;

		[Persistent] internal bool enableEnterToSearch = false;
		[Persistent] internal float timeToWaitBeforeSearch = 0.5f;
		[Persistent] internal bool enableHistory = true;
		[Persistent] internal int historyIndex = 10;
		[Persistent] internal int historySortby = (int)QHistory.SortBy.COUNT;

		[Persistent] internal string searchAND = 			"&";
		[Persistent] internal string searchOR = 			"|";
		[Persistent] internal string searchNOT = 			"!";
		[Persistent] internal string searchWord = 			"\"";
		[Persistent] internal string searchBegin = 			"(";
		[Persistent] internal string searchEnd = 			")";
		[Persistent] internal string searchRegex = 			"/";
		[Persistent] internal string searchTag = 			"%";
		[Persistent] internal string searchName = 			";";
		[Persistent] internal string searchTitle = 			":";
		[Persistent] internal string searchDescription = 	"-";
		[Persistent] internal string searchAuthor = 		",";
		[Persistent] internal string searchManufacturer = 	"?";
		[Persistent] internal string searchPartSize = 		".";
		[Persistent] internal string searchResourceInfos = 	"+";
		[Persistent] internal string searchTechRequired = 	"@";
		[Persistent] internal string searchModule = 		"_";

		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(QuickSearch.FileConfig);
			QDebug.Log ("Settings Saved", "QSettings", true);
		}
		public void Load() {
			if (File.Exists (QuickSearch.FileConfig)) {
				try {
					ConfigNode _temp = ConfigNode.Load (QuickSearch.FileConfig);
					ConfigNode.LoadObjectFromConfig (this, _temp);
				} catch {
					Save ();
				}
				QDebug.Log ("Settings Loaded", "QSettings", true);
			} else {
				Save ();
			}
			isLoaded = true;
		}
	}
}