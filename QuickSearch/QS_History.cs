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

using System.Collections.Generic;
using System.IO;

namespace QuickSearch {
	public class QHistory {

		readonly string cfgNode = "SearchHistory";
		readonly string configPath = QuickSearch.PATH + "/History.cfg";
		readonly Dictionary<string, int> history;

		public QHistory() {
			history = new Dictionary<string, int> ();
			if (!File.Exists(configPath)) {
				return;
			}
			ConfigNode nodeLoaded = ConfigNode.Load (configPath);
			ConfigNode[] nodes = nodeLoaded.GetNodes (cfgNode);
			for (int i = nodes.Length - 1; i >= 0; --i) {
				ConfigNode node = nodes[i];
				string text = "";
				int count = 0;	
				if (node.TryGetValue ("text", ref text) && node.TryGetValue ("count", ref count)) {
					history.Add (text, count);
				}
			}
		}

		public void Add(string s) {
			if (history.ContainsKey (s)) {
				history[s]++;
			}
			else {
				history.Add (s, 1);
			}
			ConfigNode node = new ConfigNode ();
			List<string> keys = new List<string> (history.Keys);
			List<int> values = new List<int> (history.Values);
			for (int i = history.Count - 1; i >= 0; --i) {
				string text = keys[i];
				int count = values[i];
				ConfigNode n = node.AddNode (cfgNode);
				n.AddValue ("text", text);
				n.AddValue ("count", count);
			}
			UnityEngine.Debug.Log (node);
			node.Save (configPath);
		}
	}
}