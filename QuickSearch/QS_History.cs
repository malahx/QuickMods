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

using System;
using System.Collections.Generic;
using System.IO;
using KSP.UI.Screens;
using UnityEngine;

namespace QuickSearch {
	public class QHistory {

		public enum SortBy {
			NAME = 0,
			COUNT = 1,
			DATE = 2
		}

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
		readonly List<Search> history;
		int index;

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

		public QHistory() {
			history = new List<Search> ();
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
				long date = 0;
				if (node.TryGetValue ("text", ref text) && node.TryGetValue ("count", ref count) && node.TryGetValue("date", ref date)) {
					history.Add (new Search(text, count, new DateTime(date)));
				}
			}
			history.SortBy (QSettings.Instance.historySortby);
		}

		public void Add(string t) {
			Search s = history.Get (t);
			if (s != null) {
				s.count += 1;
			}
			else {
				history.Add (new Search (t, 1, DateTime.Now));
			}
			history.SortBy (QSettings.Instance.historySortby);
			index = -1;
			Save ();
		}

		void Save() {
			ConfigNode node = new ConfigNode ();
			for (int i = history.Count - 1; i >= 0; i--) {
				Search s = history[i];
				ConfigNode n = node.AddNode (cfgNode);
				n.AddValue ("text", s.text);
				n.AddValue ("count", s.count);
				n.AddValue ("date", s.date.Ticks);
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
				PartCategorizer.Instance.searchField.text = history[index].text;
			}
		}

		public void Draw(int id) {
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Last Search");
			GUILayout.FlexibleSpace ();
			GUILayout.Label (QSettings.Instance.historySortby == (int)SortBy.COUNT ? "Count" : "Date");
			GUILayout.EndHorizontal ();
			for (int i = 0, count = history.Count; i < count; i++) {
				if (i >= QSettings.Instance.historyIndex) {
					break;
				}
				GUILayout.BeginHorizontal ();
				Search s = history[i];
				GUIStyle st = index == i ? LblActive : GUI.skin.label;
				GUILayout.Label (s.text, st);
				GUILayout.FlexibleSpace ();
				GUILayout.Label (QSettings.Instance.historySortby == (int)SortBy.COUNT ? s.count.ToString() : s.getDate(), st);
				GUILayout.EndHorizontal ();
			}
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

		public class Search {
			public string text;
			public int count;
			public DateTime date;

			public Search(string text, int count, DateTime date) {
				this.text = text;
				this.count = count;
				this.date = date;
			}

			public string getDate() {
				string s = "";
				DateTime today = DateTime.Today;
				if (date.ToLongDateString () == today.ToLongDateString ()) {
					s = date.ToLongTimeString ();
				}
				else if (date.ToLongDateString () == today.AddDays (-1).ToLongDateString ()) {
					s = "Yesterday";
				}
				else {
					s = date.ToShortDateString ();
				}
				return s;
			}
		}
	}
}