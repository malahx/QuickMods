/* 
QuickStart
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
using System.Reflection;

namespace QuickStart {
	public class QSettings : QuickStart {

		static readonly List<string> longWait = new List<string>(){
			"EVEManager"
		};

		[KSPField(isPersistant = true)] static readonly QSettings instance = new QSettings ();
		public static QSettings Instance {
			get {
				if (!instance.isLoaded) {
					instance.Load ();
				}
				return instance;
			}
		}

		internal static readonly string FileConfig = PATH + "/Config.txt";

		public static bool needLongWait() {
			AssemblyLoader.LoadedAssembyList _assemblies = AssemblyLoader.loadedAssemblies;
			for (int _i = _assemblies.Count - 1; _i >= 0; --_i) {
				AssemblyLoader.LoadedAssembly _assembly = _assemblies[_i];
				if (longWait.Contains(_assembly.name)) {
					return true;
				}
			}
			return false;
		}

		[KSPField(isPersistant = true)] bool isLoaded = false;

		[Persistent] internal bool Enabled = true;
		[Persistent] internal bool Debug = true;
		[Persistent] internal float WaitLoading = needLongWait() ? 5f : 0.5f;
		[Persistent] internal float editorTimeToSave = 30;
		[Persistent] internal bool enableEditorAutoSaveShip = true;
		[Persistent] internal bool enableEditorLoadAutoSave = true;
		[Persistent] internal bool enablePauseOnFlight = true;
		[Persistent] internal int gameScene = (int)GameScenes.SPACECENTER;
		[Persistent] internal int editorFacility = (int)EditorFacility.VAB;

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