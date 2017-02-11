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
using KSP.UI.Screens;
using UnityEngine;

namespace QuickSearch {
	public class QHistory {

		[KSPField (isPersistant = true)]
		static QHistory instance;
		public static QHistory Instance {
			get {
				if (instance == null) {
					instance = new QHistory ();
				}
				return instance;
			}
		}

		readonly string cfgNode = "SearchHistory";
		readonly string configPath = QuickSearch.PATH + "/History.cfg";
		readonly Dictionary<string, int> history;
		GUIStyle areaBackground;
		int index;

		List<string> cachedKey;
		List<string> CachedKey {
			get {
				if (cachedKey == null) {
					cachedKey = new List<string> (history.Keys);
					cachedKey.Sort ((a, b) => history[b].CompareTo (history[a]));
				}
				return cachedKey;
			}
		}

		GUIStyle lblActive;
		GUIStyle LblActive {
			get {
				if (lblActive == null) {
					lblActive = new GUIStyle (GUI.skin.label);
					lblActive.normal.textColor = Color.red;
				}
				return lblActive;
			}
		}

		Rect area {
			get {
				if (HighLogic.LoadedSceneIsEditor) {
					return new Rect (50, 5 + PartCategorizer.Instance.searchField.textViewport.rect.height, PartCategorizer.Instance.searchField.textViewport.rect.width, 20 * QSettings.Instance.historyIndex);
				}
				else {
					return new Rect (0, 0, Screen.width, Screen.height);
				}
			}
		}

		public QHistory() {
			history = new Dictionary<string, int> ();
			index = -1;
			if (!File.Exists (configPath)) {
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
			index = history.Count -1;
			areaBackground = new GUIStyle ();
			areaBackground.normal.background = QS_Utils.ColorToTex (area.size, new Color (0, 0, 0, 0.5f));
		}

		public void Add(string s) {
			if (history.ContainsKey (s)) {
				history[s]++;
			}
			else {
				history.Add (s, 1);
			}
			cachedKey = null;
			index = -1;
			ConfigNode node = new ConfigNode ();
			List<string> keys = new List<string> (history.Keys);
			for (int i = history.Count - 1; i >= 0; --i) {
				string text = keys[i];
				int count = history[text];
				ConfigNode n = node.AddNode (cfgNode);
				n.AddValue ("text", text);
				n.AddValue ("count", count);
			}
			node.Save (configPath);
		}

		public void Keys() {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				PreviousIndex ();
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				NextIndex ();
			}
			if (Input.GetKeyDown (KeyCode.Return) && index > -1) {
				PartCategorizer.Instance.searchField.text = CachedKey[index];
			}
		}

		public void Draw() {
			GUILayout.BeginArea (area, areaBackground);
			for (int i = 0; i < CachedKey.Count; i++) {
				if (i >= QSettings.Instance.historyIndex) {
					break;
				}
				string key = CachedKey[i];
				GUILayout.Label (key, index == i ? LblActive : GUI.skin.label);
			}
			GUILayout.EndArea ();
		}

		public void NextIndex() {
			if (index >= history.Count -1 || index >= QSettings.Instance.historyIndex -1) {
				index = -1;
			}
			index++;
		}

		public void PreviousIndex() {
			if (index <= 0) {
				index = history.Count > QSettings.Instance.historyIndex ? QSettings.Instance.historyIndex : history.Count;
			}
			index--;
		}
	}
}