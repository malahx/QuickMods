/* 
ZeroMiniAVC
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

namespace ZeroMiniAVC {
	
	[KSPAddon (KSPAddon.Startup.Instantly, true)]
	public class ZeroMiniAVC : MonoBehaviour {
		
		static ZeroMiniAVC Instance;

		readonly string pruneExt = ".pruned";
		readonly string cfgNode = "ZeroMiniAVC";
		readonly string configPath = KSPUtil.ApplicationRootPath + "GameData/ZeroMiniAVC/Config.cfg";

		ConfigNode config;
		bool disabled = false;
		bool prune = true;
		bool delete = false;
		bool noMessage = false;

		ConfigNode loadConfig() {
			ConfigNode[] _nodes = GameDatabase.Instance.GetConfigNodes (cfgNode);
			if (_nodes.Length > 0) {
				ConfigNode _node = _nodes[_nodes.Length - 1];
				if (_node.HasValue ("disabled")) {
					disabled = bool.Parse (_node.GetValue ("disabled"));
				}
				if (_node.HasValue ("prune")) {
					prune = bool.Parse (_node.GetValue ("prune"));
				}
				if (_node.HasValue ("delete")) {
					delete = bool.Parse (_node.GetValue ("delete"));
				}
				if (_node.HasValue ("noMessage")) {
					noMessage = bool.Parse (_node.GetValue ("noMessage"));
				}
				return _node;
			}
			return new ConfigNode();
		}

		string mod(string path) {
			string[] _splitedPath = path.Split (new char[2] { '/', '\\' });
			string _mod = _splitedPath[_splitedPath.IndexOf ("GameData") + 1];
			return _mod;
		}

		void Awake() {
			if (Instance != null) {
				Destroy (this);
				return;
			}
			Instance = this;
			DontDestroyOnLoad (Instance);
			config = loadConfig ();
			Debug.Log ("ZeroMiniAVC: Awake");
		}

		void Start() {
			if (disabled) {
				Debug.LogWarning ("ZeroMiniAVC: Disabled ... destroy.");
				Destroy (this);
				return;
			}
			screenMsg ("ZeroMiniAVC started ...");

			cleanMiniAVC ();

			if (!prune && !delete) {
				cleanData ();
			}

			ConfigNode _config = new ConfigNode ();
			_config.AddNode (config);
			_config.Save (configPath);

			screenMsg ("ZeroMiniAVC destroyed...");
			Destroy (this);
		}

		void cleanMiniAVC() {
			AssemblyLoader.LoadedAssembyList _assemblies = AssemblyLoader.loadedAssemblies;
			for (int _i = _assemblies.Count - 1; _i >= 0; --_i) {
				AssemblyLoader.LoadedAssembly _assembly = _assemblies[_i];
				if (_assembly.name == "MiniAVC") {
					_assembly.Unload ();
					AssemblyLoader.loadedAssemblies.RemoveAt (_i);
					string _mod = mod (_assembly.path);
					string _prunePath = _assembly.path + pruneExt;
					if (File.Exists (_prunePath)) {
						File.Delete (_prunePath);
					}
					if (prune) {
						File.Move (_assembly.path, _prunePath);
						ConfigNode _cfgMod = config.AddNode ("mod");
						_cfgMod.AddValue ("name", _mod);
						_cfgMod.AddValue ("pruned", _prunePath);
						screenMsg ("MiniAVC pruned for " + _mod);
					}
					else if (delete) {
						File.Delete (_assembly.path);
						screenMsg ("MiniAVC deleted for " + _mod);
					}
					else {
						screenMsg ("MiniAVC disabled for " + _mod);
					}
				}
			}
		}

		void cleanData() {
			ConfigNode[] _cfgMods = config.GetNodes ("mod");
			for (int _i = _cfgMods.Length - 1; _i >= 0; --_i) {
				ConfigNode _cfgMod = _cfgMods[_i];
				string _prunedPath = _cfgMod.GetValue ("pruned");
				string _mod = _cfgMod.GetValue ("name");
				if (File.Exists (_prunedPath)) {
					string _unprunedPath = _prunedPath.Substring (0, _prunedPath.Length - pruneExt.Length);
					if (File.Exists (_unprunedPath)) {
						File.Delete (_prunedPath);
						screenMsg ("MiniAVC deleted prune duplication for " + _mod);
					}
					else {
						File.Move (_prunedPath, _unprunedPath);
						screenMsg ("MiniAVC unpruned for " + _mod);
					}
				}
				else {
					screenMsg ("MiniAVC data removed for " + _mod);
				}
				config.RemoveNode (_cfgMod);
			}
		}

		void screenMsg(string msg) {
			Debug.LogWarning (msg);
			if (noMessage) {
				return;
			}
			ScreenMessages.PostScreenMessage (msg, 10);
		}
	}
}

// From MiniAVC GPLv3 Copyright (C) 2014 CYBUTEK
namespace MiniAVC {
	public class Logger : MonoBehaviour {
		void Awake() {
			Debug.Log ("MiniAVC.Logger: Destroy");
			Destroy (this);
		}
	}
	public class Starter : MonoBehaviour {
		void Awake() {
			Debug.Log ("MiniAVC.Starter: Destroy");
			Destroy (this);
		}
	}
}